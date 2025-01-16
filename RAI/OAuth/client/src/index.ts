import {UserManager} from 'oidc-client-ts'
import dotenv from 'dotenv';

dotenv.config();

const rootUrl : string = process.env.SERVER_URL!; 
const clientId: string = process.env.CLIENT_ID!;
const clientSecret: string = process.env.CLIENT_SECRET!;

const request = async ({ url, method = 'get', body, headers} : {url: string|URL, method?:string, body?: BodyInit, headers:HeadersInit}) => {
    console.log(method, url);
    const fullUrl = `${rootUrl}${url}`;
    console.log('=> fullUrl:', fullUrl, '\n');
    const options : RequestInit = { method, body, headers };
    const response = await fetch(fullUrl, options);

    const responseBody = await response.text();
    console.log('=> response:', response.status, response.statusText, responseBody, '\n');
    return responseBody;
  };



const run = async () => {
    const tokenBodyParams = new URLSearchParams();
    tokenBodyParams.append('grant_type', 'client_credentials');
    tokenBodyParams.append('scope', 'full');

    const body = await request({
        url: '/token',
        method: 'post',
        body: tokenBodyParams,
        headers: {
            Authorization: `Basic ${btoa(`${clientId}:${clientSecret}`)}`,
            'Content-Type': 'application/x-www-form-urlencoded',
        }});

    const token = JSON.parse(body);
    const access_token = token.access_token;
    const token_type = token.token_type;

    console.log('=> access_token:', access_token, '\n');
    if (access_token && token_type) {
        console.log('authorization token successfully retrieved!', '\n');
    }

    await request({
        url: '/write-resource',
        method: 'post',
        body: JSON.stringify({ value: 'foo-bar-moo' }),
        headers: {
          'content-type': 'application/json',
        //   'authorization': `${token_type} ${access_token}`
        }
      });

    await request({
        url: '/write-resource',
        method: 'post',
        body: JSON.stringify({ value: 'All your base are belong to us' }),
        headers: {
            'content-type': 'application/json',
            'authorization': `${token_type} ${access_token}`
        }
    });


    await request({
    url: '/read-resource',
    headers: {
      'authorization': `${token_type} ${access_token}`
    }
  });
}
console.log('Hello, world!');
run();
