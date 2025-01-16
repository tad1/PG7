//@ts-nocheck

import { Library, mainLibrary } from "./library";
import { getBook, getBooks } from "./presence";
import { FastifyInstance } from "fastify";

var http = require('http');
var ODataServer = require('simple-odata-server');


var model = {
    namespace: "library",
    entityTypes: {
    Book: {
        id: { type: "Edm.String", key: true },
        title: { type: "Edm.String" },
        author: { type: "Edm.String" },
        publisher: { type: "Edm.String", nullable: true },
        publish_year: { type: "Edm.String", nullable: true },
        ISBN_10: { type: "Edm.String", nullable: true },
        ISBN_13: { type: "Edm.String", nullable: true },
        language: { type: "Edm.String", nullable: true },
        category: { type: "Edm.String", nullable: true },
        genre: { type: "Edm.String", nullable: true },
        keywords: { type: "Edm.String", nullable: true }
    },
    LibraryBook: {
        id: { type: "Edm.String", key: true },
        book_id: { type: "Edm.String" },
    }
    },
    entitySets: {
        Books: {
            entityType: "library.Book"
        },
        LibraryBook: {
            entityType: "library.LibraryBook"
        }
    }
};

const getCollectionFunctions = {
    "Books": async()=>{
        return mainLibrary.getAllBookInfos();
    },
    "LibraryBook": async()=>{
        return Array.from(mainLibrary.books.values());
    }
}

var odataServer = ODataServer("http://localhost:1337")
    .model(model);

function update (collection, query, update, req, cb) {
    cb("Operation not supported");
}
    
function remove (collection, query, req, cb) {
    cb("Operation not supported");
}
    
function insert (collection, doc, req, cb) {
    cb("Operation not supported");
}
    
function query (collection_name, query, req, cb) {
    const getCollection = getCollectionFunctions[collection_name];
    if(!getCollection) {
        cb("no such collection");
        return;
    }

    const collection = getCollection();

    console.log("filter: ", query.$filter);
    const filterKeys = Object.keys(query.$filter);

    collection.then((books) => {
        let qr = books;
        
        filterKeys.forEach(filterKey => {
            const filterValue = query.$filter[filterKey];
            if(filterKey === "_id") filterKey = "id"; // oauth uses _id, we use id
            qr = qr.filter((book) => book[filterKey] === filterValue);
        });

        if(query.$sort)
            qr = qr.sort((a, b) => a[query.$sort] - b[query.$sort]);
        if(query.$skip)
            qr = qr.slice(query.$skip);
        if(query.$limit)
            qr = qr.slice(0, query.$limit);
        cb(null, qr);
    }).catch((err) => {
        console.error(err);
        cb(err);
    });
}
    

odataServer.update(update)
    .remove(remove)
    .query(query)
    .insert(insert);

export function setup_odata(server){
    // server.use('/odata', (req, res) => {
    //     odataServer.handle(req.raw, res.raw);
    // });
    http.createServer(odataServer.handle.bind(odataServer)).listen(1337);
    console.log("OData server running at http://localhost:1337");
}
