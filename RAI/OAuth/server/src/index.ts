// src/index.ts
import express, { Express, Request, Response } from "express";
import dotenv from "dotenv";
import bodyparser from "body-parser";
import OAuthServer from "@node-oauth/express-oauth-server";
import { AuthorizationCode, AuthorizationCodeModel, Client, ClientCredentialsModel, ExtensionModel, Falsey, PasswordModel, RefreshToken, RefreshTokenModel, Token, User } from "@node-oauth/oauth2-server";
import cors from "cors";


dotenv.config();

const app = express();
const port = process.env.PORT || 3000;


app.use(cors());

const user : User = {
    test: "test"
}

const clients : Client[] = [{
    id: process.env.CLIENT_ID!,
    grants: ['client_credentials'],
    secret: process.env.CLIENT_SECRET!,
}];

const AuthorizationCodes : Record<string, AuthorizationCode> = {};
const Tokens : Record<string,Token> = {};

const model : AuthorizationCodeModel | ClientCredentialsModel | RefreshTokenModel | ExtensionModel = {
    
    getAuthorizationCode: function (authorizationCode: string): Promise<AuthorizationCode | Falsey> {
        return Promise.resolve(AuthorizationCodes[authorizationCode]);
    },
    getUserFromClient: function (client: Client): Promise<User | Falsey> {
        return Promise.resolve(client && user);
    },
    saveAuthorizationCode: function (code: Pick<AuthorizationCode, "authorizationCode" | "expiresAt" | "redirectUri" | "scope" | "codeChallenge" | "codeChallengeMethod">, client: Client, user: User): Promise<AuthorizationCode | Falsey> {
        AuthorizationCodes[code.authorizationCode] = {
            authorizationCode: code.authorizationCode,
            expiresAt: code.expiresAt,
            redirectUri: code.redirectUri,
            scope: code.scope,
            codeChallenge: code.codeChallenge,
            codeChallengeMethod: code.codeChallengeMethod,
            client: client,
            user: user
        };
        return Promise.resolve(AuthorizationCodes[code.authorizationCode]);
    },
    revokeAuthorizationCode: function (code: AuthorizationCode): Promise<boolean> {
        delete AuthorizationCodes[code.authorizationCode];
        return Promise.resolve(true);
    },
    getClient: function (clientId: string, clientSecret: string): Promise<Client | Falsey> {
        const client = clients.find(client => client.id === clientId && client.secret === clientSecret);
        return Promise.resolve(client);
    },
    saveToken: function (token: Token, client: Client, user: User): Promise<Token | Falsey> {
        token.client = client;
        token.user = user;
        Tokens[token.accessToken] = token;
        return Promise.resolve(token);
    },
    getAccessToken: function (accessToken: string): Promise<Token | Falsey> {
        return Promise.resolve(Tokens[accessToken]);
    },
    getRefreshToken: async function (refreshToken: string): Promise<RefreshToken | Falsey> {
        const token = Object.values(Tokens).find(token => token.refreshToken === refreshToken);
        if(!token) {
            return false;
        }
        return {
            refreshToken: token.refreshToken!,
            accessToken: token.accessToken,
            user: token.user,
            client: token.client,
            scope: token.scope,
        }
    },

    revokeToken: function (token: RefreshToken): Promise<boolean> {
        delete Tokens[token.accessToken];
        return Promise.resolve(true);
    },

    verifyScope: function (token: Token, scope: string | string[]): Promise<boolean> {
        return Promise.resolve(true);
    }
}

const oauth = new OAuthServer({
    model: model,
});

app.use(bodyparser.json());
app.use(bodyparser.urlencoded({ extended: false }));

//@ts-ignore
app.use('/token', oauth.token());

//@ts-ignore
app.use('/public', function (req, res) {
    res.send('public area');
});

//@ts-ignore
app.get("/private", oauth.authenticate(), (req: Request, res: Response) => {
  res.send("Secret area");
});

const internal = {
    resource: null
};

//@ts-ignore
app.get('/read-resource', oauth.authenticate(), function (req, res) {
    res.send({ resource: internal.resource });
  });
  
  //@ts-ignore
  app.post('/write-resource',  oauth.authenticate(), function (req, res) {
    internal.resource = req.body.value;
    console.log('resource updated:', internal.resource);
    res.send({ message: 'resource created' });
  });

app.listen(port, () => {
    console.debug('[Provider]: listens to http://localhost:'+port);
});

// const privateKey = fs.readFileSync('./cert/localhost+1-key.pem');
// const certificate = fs.readFileSync('./cert/localhost+1.pem');

// https.createServer({
//     key: privateKey,
//     cert: certificate
// }, app).listen(4430, () => {
//     console.log(`[server]: Server is running at https://localhost:4430`);
// });