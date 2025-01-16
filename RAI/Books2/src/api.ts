import { FastifyInstance } from "fastify";
import { deleteBook, getBook, getBooks, setBook, trySetBook } from "./presence";
import { Book, mainLibrary } from "./library";
import { getBookByIdSchema, getBooksResponseSchema, deleteBookSchema, updateBookSchema, createBookSchema } from "./api-docs";


export function setupEndpoints(server : FastifyInstance){      
      server.get('/books',getBooksResponseSchema, async (request, reply) => {
        const books = await getBooks();
        return reply.send(books);
      })
      
      server.get('/books/:id', getBookByIdSchema, async (request, reply) => {
        const { id } = request.params as { id: string }
        const book = await mainLibrary.getBookInfo(id);
        return reply.send(book);
      });
      
      server.post('/books',createBookSchema, async (request, reply) => {
        const book: Book = request.body as Book;
        return mainLibrary.addBookInfo(book).then(async (res) => {
            if(res === false){
                reply.status(409).send();
            } else {
                reply.status(201).send();
            }
        }).catch((err) => {
          reply.status(500).send(err);
        });
      });

      server.put('/books',updateBookSchema, async (request, reply) => {
        const updatedBook: Book = request.body as Book;
       
        return mainLibrary.updateBookInfo(updatedBook).then(() => {
          reply.status(204).send();
        }).catch((err) => {
          reply.status(500).send(err);
        });
      });
      
      server.delete('/books/:id',deleteBookSchema, async (request, reply) => {
        const { id } = request.params as { id: string };
        return mainLibrary.removeBookInfo(id).then(() => {
          reply.status(204).send();
        }).catch((err) => {
          reply.status(500).send(err);
        });
      });
}