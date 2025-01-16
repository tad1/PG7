import { deleteBook, getBook, getBooks, setBook, trySetBook } from "./presence";

export {};

interface Book {
  id: string;
  title: string;
  author: string;
  publisher?: string;
  publish_year?: string;
  ISBN_10?: string;
  ISBN_13?: string;
  language?: string;
  category?: string;
  genre?: string;
  keywords?: string;
};

class Account {
  private readonly id: string;
  readonly name: string;

  constructor(id: string, name: string) {
    this.id = id;
    this.name = name;
  }

  getId(): string {
    return this.id;
  }
}

class LibraryBook {
  readonly id: string;
  readonly book_id: string;

  constructor(id: string, book_id: string) {
    this.id = id;
    this.book_id = book_id;
  }
}

class BorrowEntry<BorrowedObject, Borrower> {
  readonly borrower: Borrower;
  readonly object: BorrowedObject;
  readonly borrow_date: Date;
  return_date: Date | null;

  constructor(borrower: Borrower, object: BorrowedObject, borrow_date: Date, return_date: Date | null = null) {
    this.borrower = borrower;
    this.object = object;
    this.borrow_date = borrow_date;
    this.return_date = return_date;
    Object.freeze(this);
  }
}

class Library {
  private booksMap: Map<string, LibraryBook[]> = new Map();
  private ledgerMap: Map<LibraryBook, BorrowEntry<LibraryBook, Account>[]> = new Map();
  private libraryName: string;
  private libraryLocation: string;

  constructor(name: string, location: string) {
    this.libraryName = name;
    this.libraryLocation = location;
  }

  get books(): Map<string, LibraryBook[]> {
    return new Map(this.booksMap);
  }

  get ledger(): Map<LibraryBook, BorrowEntry<LibraryBook, Account>[]> {
    return new Map(this.ledgerMap);
  }

  get name(): string {
    return this.libraryName;
  }

  get location(): string {
    return this.libraryLocation;
  }

  rent(books: LibraryBook[], account: Account): LibraryBook[] {
    return books
      .filter((book) => this.booksMap.get(book.book_id)?.includes(book) ?? false)
      .map((book) => {
        let ledger = this.ledgerMap.get(book) ?? [];
        if (ledger.length === 0 || ledger.at(-1)?.return_date !== null) {
          ledger.push(new BorrowEntry(account, book, new Date(), null));
          this.ledgerMap.set(book, ledger);
          return book;
        }
        return null;
      })
      .filter((book): book is LibraryBook => book !== null);
  }

  return(books: LibraryBook[]): LibraryBook[] {
    return books.filter((book) => {
      const lastEntry = this.ledgerMap.get(book)?.at(-1);
      if (!lastEntry || lastEntry.return_date !== null) return true;
      lastEntry.return_date = new Date();
      return false;
    });
  }

  whoRented(book: LibraryBook): Account | null {
    const lastEntry = this.ledgerMap.get(book)?.at(-1);
    return lastEntry?.return_date === null ? lastEntry.borrower : null;
  }

  getBookInfo(book_id: string){
    const promise = getBook(book_id);
    return promise;
  }

  getAllBookInfos(){
    const promise = getBooks();
    return promise;
  }

  addBookInfo(book: Book) {
    const promise = trySetBook(book);
    return promise;
  }

  updateBookInfo(book: Book) {
    const promise = setBook(book);
    return promise;
  }

  removeBookInfo(book_id: string) {
    const promise = deleteBook(book_id);
    return promise;
  }

  registerBooks(books: LibraryBook[]): void {
    books.forEach((book) => {
      const list = this.booksMap.get(book.book_id) ?? [];
      list.push(book);
      this.booksMap.set(book.book_id, list);
    });
  }

  async searchExact(criteria: Partial<Book>): Promise<LibraryBook[]> {
    const promises = [...this.booksMap.entries()].flatMap(async ([book_id, libraryBooks]) => {
      const book = await getBook(book_id);
      return Object.entries(criteria).every(([key, value]) => (book as any)[key] === value) ? libraryBooks : []
    });
    return (await Promise.all(promises)).flat();
  }

  async searchRE(criteria: Record<string, string>): Promise<LibraryBook[]> {
    const promises = Array.from(this.booksMap.entries())
      .filter(async ([book_id]) =>{
        const book = await getBook(book_id);
        Object.entries(criteria).every(([key, regex]) => new RegExp(regex).test((book as any)[key]))
      }
      )
      .flatMap(([, libraryBooks]) => libraryBooks);
    return (await Promise.all(promises)).flat();
  }

  stats(): string {
    return `
${this.libraryName}, ${this.libraryLocation}
Total books: ${Array.from(this.booksMap.values()).reduce((sum, books) => sum + books.length, 0)}
Total unique books: ${this.booksMap.size}`;
  }
}

const mainLibrary = new Library("Main Library", "Main Street");

export { Account, Book, Library, LibraryBook, mainLibrary };

