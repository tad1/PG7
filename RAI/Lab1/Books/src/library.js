"use strict"

/**
 * @prop {readonly string} title
 * @prop {readonly string} author
 * @prop {readonly string?} publisher
 * @prop {readonly string?} publish_year
 * @prop {readonly string?} ISBN_10
 * @prop {readonly string?} ISBN_13
 * @prop {readonly string?} language
 * @prop {readonly string?} category
 * @prop {readonly string?} genre
 * @prop {readonly string?} keywords
 */
class Book {
    /**
     * @param {{
     *      title: string,
     *      author: string,
     *      publisher: string?,
     *      publish_year: string?,
     *      ISBN_10: string?,
     *      ISBN_13: string?,
     *      language: string?,
     *      category: string?,
     *      genre: string?,
     *      keywords: string?
     * }} info 
     */
    constructor(info){
        if(!info.author || !info.title) throw TypeError(`Missing required properties: {${info.author ? "" : "author,"}${info.title ? "" : "title"}}`);
        //#NF12: object assign
        Object.assign(this, info);
        //#NF4: immutable objects 
        Object.freeze(this);
    };
}

class Account {
    constructor(id, name){
        Object.defineProperty(this, 'id', {get: function(){return id}}); //defines immutable prop
        this.name = name;
    }
}

/**
 * @prop {readonly string} id
 * @prop {readonly Book} book
 */
class LibraryBook {        
    /**
     * @param {string} id 
     * @param {Book} book 
     */
    constructor(id, book){
        this.id = id;
        this.book = book;
        Object.freeze(this);
    }
};

/**
 * @template BorrowedObject
 * @template Borrower
 */
class BorrowEntry {
    /**
     * @param {Borrower} borrower 
     * @param {BorrowedObject} object 
     * @param {Date} borrow_date 
     * @param {Date?} return_date 
     */
    constructor(borrower, object, borrow_date, return_date){
        this.borrower = borrower;
        this.object = object;
        this.borrow_date = borrow_date;
        this.return_date = return_date;
        Object.freeze(this)
    }    
};

class Library {

    /**
     * #NF1: private class fields 
     * #NF2: map class 
     * @type {Map<Book, Array<LibraryBook>>}
     */
    #books = new Map();
    get books(){
        return new Map(this.#books);
    }

    /**
     * @type {Map<LibraryBook, Array<BorrowEntry<Account, LibraryBook>>}
     */
    #ledger = new Map();
    //#NF13: public accessor
    get ledger() {
        return new Map(this.#ledger);
    }


    /**
     * @type {string}
     */
    #name;
    get name(){
        return this.#name;
    }
    /**
     * @type {string}
     */
    #location;
    get location(){
        return this.#location;
    }

    /**
     * @param {string} name 
     * @param {string} location 
     */
    constructor(name, location){
        this.#name = name;
        this.#location = location;
    }

    /**
     * returns sucessfully rented books
     * @param {Array<LibraryBook>} books 
     * @param {Account} account 
     * @returns {Array<LibraryBook>}
     */
    rent(books, account){
        return books
        .filter(book => {
            //#NF2: optional chaining ?.find()
            return this.#books.get(book.book)?.find(v => v === book) !== undefined;
        })
        .map(book => 
        {
            let ledger = this.#ledger.get(book);
            if(ledger === undefined) {
                ledger = []; 
                this.#ledger.set(book, ledger)
            };
            /**
             * @type {[LibraryBook, BorrowEntry<Account, LibraryBook>[]]}
             */
            let res = [book,ledger]
            return res;
        })
        .filter(([_, ledger]) => 
            ledger.length === 0 || ledger.at(-1).return_date !== null
        ).map(([book, ledger]) => {
            ledger.push({object: book,borrower: account, borrow_date: new Date(Date.now()), return_date: null})
            return book;
        });
    }


    /**
     * Returns not-returned books
     * @param {Array<LibraryBook>} books 
     * @returns {Array<LibraryBook>?}
     */
    return(books){
        return books.filter(book => {
            let entry = this.#ledger.get(book)?.at(-1);
            if(!entry || entry.return_date != null){return true};
            entry.return_date = new Date(Date.now());
            return false;
        })
    }

    /**
     * @param {Array<LibraryBook>} books 
     */
    registerBooks (books){
        books.forEach(book => {
            let list = this.#books.get(book.book);
            if(list === undefined){
                list = [];
                this.#books.set(book.book, list);
            }
            list.push(book);
        })
    }

    /**
     * @returns {Array<Book>}
     * @param  {object} criteria 
     */
    searchExact (criterias){
        return Array.from(this.#books.entries()).filter(([book,_]) => 
            Object.entries(criterias).every(([key, value]) => book[key] === value)
         ).map(([_, books])=>books).flat();
    }

    searchRE (criterias){
        //#NF5: array methods (filter, flat, map)
        //#NF6: arrow function
        //#NF7: array deconstruction
        //#NF8: Object.entries()
        //#NF9: every()
        //#NF10: Array.from()
        //#NF11: regular expressions
        return Array.from(this.#books.entries()).filter(([book,_]) => 
            Object.entries(criterias).every(([key, value]) => 
                (new RegExp(value)).test(book[key])
        )
        ).map(([_, books])=> books).flat();
    }

    stats(){
        //#NF3: Template Literals `${}`
        return `
${this.#name}, ${this.#location}
total books: ${Array.from(this.#books.keys()).reduce((acc, key)=> acc + this.#books.get(key).length, 0)}
total unique books: ${Array.from(this.#books.keys()).length}`;
    }
}

export {
    Account,
    Book,
    Library,
    LibraryBook,
}