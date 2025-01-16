import { SwaggerOptions } from "@fastify/swagger";
import { FastifyRegisterOptions } from "fastify";

export const docs : FastifyRegisterOptions<SwaggerOptions> = {
    openapi: {
        info: {
          title: 'Books API',
          description: 'API for managing books',
          version: '1.0.0'
        },
        servers: [
          {
            url: 'http://localhost:8080',
            description: 'Local server'
          }
        ],
        components: {
          schemas: {
            Book: {
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
            }
          }
        }
      }
}

export const getBookByIdSchema = {
  schema: {
    params: {
      type: 'object',
      properties: {
        id: { type: 'string' }
      }
    },
    response: {
      200: {
        $ref: 'Book#'
      }
    }
  }
};
export const createBookSchema = {
  schema: {
    body: {
      $ref: 'Book#'
    },
    response: {
      201: {
        description: 'Book created',
      },
      409: {
        description: 'Book already exists',
      }
    }
  }
};
export const updateBookSchema = {
  schema: {
    body: {
      $ref: 'Book#'
    },
    response: {
      204: {
        description: 'Book updated',
      }
    }
  },
};
export const deleteBookSchema = {
  schema: {
    params: {
      type: 'object',
      properties: {
        id: { type: 'string' }
      }
    },
    response: {
      204: {
        description: 'Book deleted',
      }
    }
  }
};
export const getBooksResponseSchema = {
  schema: {
    response: {
      200: {
        type: 'array',
        items: {
          $ref: 'Book#'
        }
      }
    }
  }
};