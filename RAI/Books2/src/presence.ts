// add here caching

import path from "path";
import { Book } from "./library";
import fs from 'fs/promises';

const BASE_DIR = path.resolve(process.env.BASE_DIR || './data')

const availableBooksCache = new Map<string, boolean>();
const bookPromiseCache = new Map<string, Promise<Book>>();

export const getBook = async (id: string): Promise<Book> => {
    if(!availableBooksCache.has(id) || availableBooksCache.get(id) === false){
        const getBookPromise = fs.readFile(`${BASE_DIR}/${id}.json`, 'utf-8').then
        (data => {
            return JSON.parse(data);
        });
        bookPromiseCache.set(id, getBookPromise); 
    }

    return bookPromiseCache.get(id)!;
}

export const getBooks = async (): Promise<Book[]> => {
    const bookFilenames = (await fs.readdir(BASE_DIR)).map((filename) => filename.replace('.json', ''));
    return Promise.all(bookFilenames.map((book) => getBook(book)));
}

export const trySetBook = async (book: Book): Promise<boolean> => {
    if(availableBooksCache.has(book.id) || await fs.access(`${BASE_DIR}/${book.id}.json`).then(() => true).catch(() => false)){
        return false;
    }

    try{
        await setBook(book);
        return true;
    } catch(err){
        return false;
    }
}

export const setBook = async (book: Book): Promise<void> => {
    await fs.writeFile(`${BASE_DIR}/${book.id}.json`, JSON.stringify(book));
    availableBooksCache.set(book.id, false);
}

export const deleteBook = async (id: string): Promise<void> => {
    await fs.unlink(`${BASE_DIR}/${id}.json`);
    availableBooksCache.delete(id);
};