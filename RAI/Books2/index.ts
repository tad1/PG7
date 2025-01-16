import fastify from 'fastify'
import fs from 'fs/promises'
import path from 'path'
import { deleteBook, getBook, getBooks, setBook } from './src/presence'
import { Book } from './src/library'
import fastifySwagger from '@fastify/swagger'
import fastifySwaggerUi from '@fastify/swagger-ui'

import { docs } from './src/api-docs'
import { setupEndpoints } from './src/api'
import { setup_odata } from './src/odata'
import { fastifyExpress } from '@fastify/express'
import { populate } from './src/populate'
const express = require('express')

const server = fastify()
const BASE_DIR = path.resolve(process.env.BASE_DIR || './data')

async function ensureDatabase(){
    try{
        await fs.mkdir(BASE_DIR,{ recursive: true })
    } catch(err){
        console.error(err)
    }
}

async function setup(){
  await ensureDatabase()
  await server.register(fastifyExpress);
  await populate();
  setup_odata(server);

  server.register(fastifySwagger);

  server.register(fastifySwaggerUi, {
    routePrefix: '/docs',
    uiConfig: {
      docExpansion: 'full',
      deepLinking: false
    },
    uiHooks: {
      onRequest: function (request, reply, next) { next() },
      preHandler: function (request, reply, next) { next() }
    },
    staticCSP: true,
    transformStaticCSP: (header) => header,
    transformSpecification: (swaggerObject, request, reply) => { return swaggerObject },
    transformSpecificationClone: true
  });

  server.addSchema({
    $id: 'Book',
    type: 'object',
    properties: {
      id: { type: 'string' },
      title: { type: 'string' },
      author: { type: 'string' },
      publisher: { type: 'string' },
      publish_year: { type: 'string' },
      ISBN_10: { type: 'string' },
      ISBN_13: { type: 'string' },
      language: { type: 'string' },
      category: { type: 'string' },
      genre: { type: 'string' },
      keywords: { type: 'string' }
    },
    required: ['id', 'title', 'author']
  });

  server.register(async function (server, options) {
    setupEndpoints(server)
  }) 



  server.listen({ port: 8080 }, (err, address) => {
    if (err) {
      console.error(err)
      process.exit(1)
    }
    console.log(`Server listening at ${address}`)
  })

  await server.ready()
  server.swagger()
}


setup()




