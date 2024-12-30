import { Router } from "express";
import { authenticateToken } from "./jwt";
import { authRouter } from "./route/auth";
import {SavedStops} from './db_shema';


import specs from "./swagger-specs";
import mongoose, { get } from "mongoose";

const dotenv = require('dotenv');
const cors = require('cors');
const bodyParser = require("body-parser"),
  swaggerUi = require("swagger-ui-express");

dotenv.config();

const mongoDB = process.env.MONGODB_URI;
const port = process.env.PORT;

mongoose.connect(mongoDB!);

var express = require("express");
var app = express();
app.use(cors());
app.use(express.json());


const getCurrentDate = (): string => {
    const date = new Date();
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-based
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
};

const fs = require('fs');
var allStops: any;
var allRoutes: any;
var currentStops: any;
var currentRoutes: any;
var stopsDict: Record<number, any> = {};
var routesDict: Record<number, any> = {};

async function fetchStops() {
    console.log("Fetching stops");
    const url = "https://ckan.multimediagdansk.pl/dataset/c24aa637-3619-4dc2-a171-a23eec8f2172/resource/4c4025f0-01bf-41f7-a39f-d156d201b82b/download/stops.json";
    const response = await fetch(url);
    const data = await response.json();
    allStops = data;

    fs.writeFileSync('stops.json', JSON.stringify(data));
}

async function fetchRoutes() {
    console.log("Fetching stops");
    const url = "https://ckan.multimediagdansk.pl/dataset/c24aa637-3619-4dc2-a171-a23eec8f2172/resource/22313c56-5acf-41c7-a5fd-dc5dc72b3851/download/routes.json";
    const response = await fetch(url);
    const data = await response.json();
    allRoutes = data;

    fs.writeFileSync('routes.json', JSON.stringify(data));
}
async function getCurrentStops() {
    const currentDate = getCurrentDate();

    if(allStops === undefined){
        if(!fs.existsSync('stops.json')) {
            await fetchStops();
        } else {
            console.log("Reading stops from cached file");
            allStops = JSON.parse(fs.readFileSync('stops.json', 'utf8'));
        }
    }

    if (!(currentDate in allStops)) {
        await fetchStops();
    }
    return allStops[currentDate]['stops'];
}

async function getCurrentRoutes() {
    const currentDate = getCurrentDate();

    if(allRoutes === undefined){
        if(!fs.existsSync('routes.json')) {
            await fetchRoutes();
        } else {
            console.log("Reading routes from cached file");
            allRoutes = JSON.parse(fs.readFileSync('routes.json', 'utf8'));
        }
    }

    if (!(currentDate in allRoutes)) {
        await fetchRoutes();
    }
    return allRoutes[currentDate]['routes'];
}

getCurrentStops().then((stops: any) => {
    currentStops = stops;
    stopsDict = currentStops.reduce((dict: any, stop: any) => {
        dict[stop['stopId']] = stop;
        return dict;
        }, {} as Record<number, any>
    );

    app.listen(port, () => {
        console.log(`Server is running on port ${port}`);
    });
});

getCurrentRoutes().then((routes: any) => {
    currentRoutes = routes;
    routesDict = currentRoutes.reduce((dict: any, route: any) => {
        dict[route['routeId']] = route;
        return dict;
        }, {} as Record<number, any>);
});


app.get("/", (req: any, res: any) => {
    res.json({app: "Hello World!"});
});

let api = Router();
api.use('/auth', authRouter);
app.use("/api", api);


api.get("/delays/:id", (req: any, res: any) => {
    const url = `https://ckan2.multimediagdansk.pl/delays?stopId=${req.params.id}`;
    fetch(url).then(response => response.json()).then(data => {
        data.delay.forEach((delay: any) => {
            delay.routeShortName = routesDict[delay.routeId].routeShortName;
        });
        res.json(data);
    }).catch(err => {
        console.log(err);
        res.sendStatus(500);
    });
});

let stops = Router();

api.use("/stops", stops);

stops.get("/", (req: any, res: any) => {
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;

    const startIndex = (page - 1) * limit;
    const total = currentStops.length;

    const stops = currentStops.slice(startIndex, startIndex + limit);

    res.json({
        totalPages: Math.ceil(total / limit),
        page: page,
        stops: stops,
    });
});
stops.get("/:id", (req: any, res: any) => {
    res.json(stopsDict[req.params.id]);
});

let savedStops = Router();
api.use("/saved", savedStops);
savedStops.get("/", authenticateToken, async (req: any, res: any) => {
    const stops = await SavedStops.find({username: req.user.username});
    res.json(stops.map((stop: any) => stop.stopId));
});

savedStops.post("/add/:id", authenticateToken, (req: any, res: any) => {
    let stop = new SavedStops({username: req.user.username, stopId: req.params.id});
    stop.save().then(() => {
        res.sendStatus(200);
    }).catch((err: any) => {
        console.log(err);
        res.sendStatus(400);
    });
});

savedStops.delete("/remove", authenticateToken, (req: any, res: any) => {
    SavedStops.deleteOne({username: req.user.username, stopId: req.body.stopId}).then(() => {
        res.sendStatus(200);
    }).catch((err: any) => {
        console.log(err);
        res.sendStatus(400);
    });
});

app.use(
    "/api-docs",
    swaggerUi.serve,
    swaggerUi.setup(specs)
  );