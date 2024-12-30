import { Router } from "express";
import { authenticateToken } from "../jwt";
import {IUser, User} from '../db_shema';

const jwt = require('jsonwebtoken');
const router = Router();


function generateAccessToken(username: string) {
    return jwt.sign({username: username}, process.env.TOKEN_SECRET, { expiresIn: '1800s' });
}  


/**
 * @swagger
 * components:
 *   schemas:
 *     LoginRequest:
 *       type: object
 *       required:
 *         - username
 *         - password
 *       properties:
 *         username:
 *           type: string
 *           description: The username of the user
 *         password:
 *           type: string
 *           description: The password of the user
 *       example:
 *         username: exampleUser
 *         password: examplePassword
 *     TokenResponse:
 *       type: object
 *       properties:
 *         token:
 *           type: string
 *           description: JWT token for the authenticated user
 *       example:
 *         token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
 *     RegisterRequest:
 *       type: object
 *       required:
 *         - username
 *         - password
 *       properties:
 *         username:
 *           type: string
 *           description: The username to register
 *         password:
 *           type: string
 *           description: The password to register
 *       example:
 *         username: exampleUser
 *         password: examplePassword
 */

/**
 * @swagger
 * /auth/login:
 *   post:
 *     summary: Authenticate a user and return a token
 *     tags: [Auth]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             $ref: '#/components/schemas/LoginRequest'
 *     responses:
 *       200:
 *         description: Successfully authenticated
 *         content:
 *           application/json:
 *             schema:
 *               $ref: '#/components/schemas/TokenResponse'
 *       403:
 *         description: Forbidden - invalid credentials
 *       404:
 *         description: User not found
 */
router.post("/login", async (req, res) => {
    // return token
    const username : string = req.body.username;
    const password : string = req.body.password;

    User.findOne({username: username}).then(async user => {
        console.log(user);
        if(!user) {
            res.sendStatus(404);
            return;
        }
        console.log("comparing password");
        const isMatch = await user.comparePassword(password);
        if(isMatch) {
            console.log("passwords match");
            const token = generateAccessToken(username);
            res.json({token: token});
            return;
        } else {
            console.log("passwords don't match");
            res.sendStatus(403);
            return;
        }
    }).catch(err => {
        console.log(err.message);
        res.sendStatus(500);
        return
    });
});

/**
 * @swagger
 * /auth/register:
 *   post:
 *     summary: Register a new user
 *     tags: [Auth]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             $ref: '#/components/schemas/RegisterRequest'
 *     responses:
 *       200:
 *         description: Successfully registered
 *       400:
 *         description: Bad request - validation error
 */
router.post("/register", async (req, res) => {
    const username : string = req.body.username;
    const password : string = req.body.password;

    let newUser = new User({username: username, password: password});
    newUser.save().then(() => {
        res.sendStatus(200);
    }).catch((err: any) => {
        console.log(err);
        res.sendStatus(400);
    });
});


export { router as authRouter };