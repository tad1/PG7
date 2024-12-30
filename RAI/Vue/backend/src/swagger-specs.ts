const swaggerJsdoc = require("swagger-jsdoc");

const options = {
    definition: {
      openapi: "3.1.0",
      info: {
        title: "Tristar App API",
        version: "0.1.0",
      },
      components: {
        securitySchemes: {
          bearerAuth: {
            type: 'http',
            scheme: 'bearer',
            bearerFormat: 'JWT',
          },
        },
      },
      security: [
        {
          bearerAuth: [],
        },
      ],
      servers: [
        {
          url: "http://localhost:3000",
        },
      ],
    },
    apis: ["./src/route/*.ts", "./src/index.ts"],
  };


const specs = swaggerJsdoc(options);
export default specs;