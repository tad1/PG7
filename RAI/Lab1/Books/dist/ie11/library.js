"use strict";

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
function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.LibraryBook = exports.Library = exports.Book = exports.Account = void 0;
function _toConsumableArray(r) { return _arrayWithoutHoles(r) || _iterableToArray(r) || _unsupportedIterableToArray(r) || _nonIterableSpread(); }
function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }
function _iterableToArray(r) { if ("undefined" != typeof Symbol && null != r[Symbol.iterator] || null != r["@@iterator"]) return Array.from(r); }
function _arrayWithoutHoles(r) { if (Array.isArray(r)) return _arrayLikeToArray(r); }
function _slicedToArray(r, e) { return _arrayWithHoles(r) || _iterableToArrayLimit(r, e) || _unsupportedIterableToArray(r, e) || _nonIterableRest(); }
function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }
function _unsupportedIterableToArray(r, a) { if (r) { if ("string" == typeof r) return _arrayLikeToArray(r, a); var t = {}.toString.call(r).slice(8, -1); return "Object" === t && r.constructor && (t = r.constructor.name), "Map" === t || "Set" === t ? Array.from(r) : "Arguments" === t || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(t) ? _arrayLikeToArray(r, a) : void 0; } }
function _arrayLikeToArray(r, a) { (null == a || a > r.length) && (a = r.length); for (var e = 0, n = Array(a); e < a; e++) n[e] = r[e]; return n; }
function _iterableToArrayLimit(r, l) { var t = null == r ? null : "undefined" != typeof Symbol && r[Symbol.iterator] || r["@@iterator"]; if (null != t) { var e, n, i, u, a = [], f = !0, o = !1; try { if (i = (t = t.call(r)).next, 0 === l) { if (Object(t) !== t) return; f = !1; } else for (; !(f = (e = i.call(t)).done) && (a.push(e.value), a.length !== l); f = !0); } catch (r) { o = !0, n = r; } finally { try { if (!f && null != t.return && (u = t.return(), Object(u) !== u)) return; } finally { if (o) throw n; } } return a; } }
function _arrayWithHoles(r) { if (Array.isArray(r)) return r; }
function _classPrivateFieldInitSpec(e, t, a) { _checkPrivateRedeclaration(e, t), t.set(e, a); }
function _checkPrivateRedeclaration(e, t) { if (t.has(e)) throw new TypeError("Cannot initialize the same private elements twice on an object"); }
function _classPrivateFieldSet(s, a, r) { return s.set(_assertClassBrand(s, a), r), r; }
function _classPrivateFieldGet(s, a) { return s.get(_assertClassBrand(s, a)); }
function _assertClassBrand(e, t, n) { if ("function" == typeof e ? e === t : e.has(t)) return arguments.length < 3 ? t : n; throw new TypeError("Private element is not present on this object"); }
function _defineProperties(e, r) { for (var t = 0; t < r.length; t++) { var o = r[t]; o.enumerable = o.enumerable || !1, o.configurable = !0, "value" in o && (o.writable = !0), Object.defineProperty(e, _toPropertyKey(o.key), o); } }
function _createClass(e, r, t) { return r && _defineProperties(e.prototype, r), t && _defineProperties(e, t), Object.defineProperty(e, "prototype", { writable: !1 }), e; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
function _classCallCheck(a, n) { if (!(a instanceof n)) throw new TypeError("Cannot call a class as a function"); }
var Book = exports.Book = /*#__PURE__*/_createClass(
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
function Book(info) {
  _classCallCheck(this, Book);
  if (!info.author || !info.title) throw TypeError("Missing required properties: {".concat(info.author ? "" : "author,").concat(info.title ? "" : "title", "}"));
  //#NF12: object assign
  Object.assign(this, info);
  //#NF4: immutable objects 
  Object.freeze(this);
});
var Account = exports.Account = /*#__PURE__*/_createClass(function Account(id, name) {
  _classCallCheck(this, Account);
  Object.defineProperty(this, 'id', {
    get: function get() {
      return id;
    }
  }); //defines immutable prop
  this.name = name;
});
/**
 * @prop {readonly string} id
 * @prop {readonly Book} book
 */
var LibraryBook = exports.LibraryBook = /*#__PURE__*/_createClass(
/**
 * @param {string} id 
 * @param {Book} book 
 */
function LibraryBook(id, book) {
  _classCallCheck(this, LibraryBook);
  this.id = id;
  this.book = book;
  Object.freeze(this);
});
;

/**
 * @template BorrowedObject
 * @template Borrower
 */
var BorrowEntry = /*#__PURE__*/_createClass(
/**
 * @param {Borrower} borrower 
 * @param {BorrowedObject} object 
 * @param {Date} borrow_date 
 * @param {Date?} return_date 
 */
function BorrowEntry(borrower, object, borrow_date, return_date) {
  _classCallCheck(this, BorrowEntry);
  this.borrower = borrower;
  this.object = object;
  this.borrow_date = borrow_date;
  this.return_date = return_date;
  Object.freeze(this);
});
;
var _books = /*#__PURE__*/new WeakMap();
var _ledger = /*#__PURE__*/new WeakMap();
var _name = /*#__PURE__*/new WeakMap();
var _location = /*#__PURE__*/new WeakMap();
var Library = exports.Library = /*#__PURE__*/function () {
  /**
   * @param {string} name 
   * @param {string} location 
   */
  function Library(name, location) {
    _classCallCheck(this, Library);
    /**
     * #NF1: private class fields 
     * #NF2: map class 
     * @type {Map<Book, Array<LibraryBook>>}
     */
    _classPrivateFieldInitSpec(this, _books, new Map());
    /**
     * @type {Map<LibraryBook, Array<BorrowEntry<Account, LibraryBook>>}
     */
    _classPrivateFieldInitSpec(this, _ledger, new Map());
    /**
     * @type {string}
     */
    _classPrivateFieldInitSpec(this, _name, void 0);
    /**
     * @type {string}
     */
    _classPrivateFieldInitSpec(this, _location, void 0);
    _classPrivateFieldSet(_name, this, name);
    _classPrivateFieldSet(_location, this, location);
  }

  /**
   * returns sucessfully rented books
   * @param {Array<LibraryBook>} books 
   * @param {Account} account 
   * @returns {Array<LibraryBook>}
   */
  return _createClass(Library, [{
    key: "books",
    get: function get() {
      return new Map(_classPrivateFieldGet(_books, this));
    }
  }, {
    key: "ledger",
    get:
    //#NF13: public accessor
    function get() {
      return new Map(_classPrivateFieldGet(_ledger, this));
    }
  }, {
    key: "name",
    get: function get() {
      return _classPrivateFieldGet(_name, this);
    }
  }, {
    key: "location",
    get: function get() {
      return _classPrivateFieldGet(_location, this);
    }
  }, {
    key: "rent",
    value: function rent(books, account) {
      var _this = this;
      return books.filter(function (book) {
        var _classPrivateFieldGet2;
        //#NF2: optional chaining ?.find()
        return ((_classPrivateFieldGet2 = _classPrivateFieldGet(_books, _this).get(book.book)) === null || _classPrivateFieldGet2 === void 0 ? void 0 : _classPrivateFieldGet2.find(function (v) {
          return v === book;
        })) !== undefined;
      }).map(function (book) {
        var ledger = _classPrivateFieldGet(_ledger, _this).get(book);
        if (ledger === undefined) {
          ledger = [];
          _classPrivateFieldGet(_ledger, _this).set(book, ledger);
        }
        ;
        /**
         * @type {[LibraryBook, BorrowEntry<Account, LibraryBook>[]]}
         */
        var res = [book, ledger];
        return res;
      }).filter(function (_ref) {
        var _ref2 = _slicedToArray(_ref, 2),
          _ = _ref2[0],
          ledger = _ref2[1];
        return ledger.length === 0 || ledger.at(-1).return_date !== null;
      }).map(function (_ref3) {
        var _ref4 = _slicedToArray(_ref3, 2),
          book = _ref4[0],
          ledger = _ref4[1];
        ledger.push({
          object: book,
          borrower: account,
          borrow_date: new Date(Date.now()),
          return_date: null
        });
        return book;
      });
    }

    /**
     * Returns not-returned books
     * @param {Array<LibraryBook>} books 
     * @returns {Array<LibraryBook>?}
     */
  }, {
    key: "return",
    value: function _return(books) {
      var _this2 = this;
      return books.filter(function (book) {
        var _classPrivateFieldGet3;
        var entry = (_classPrivateFieldGet3 = _classPrivateFieldGet(_ledger, _this2).get(book)) === null || _classPrivateFieldGet3 === void 0 ? void 0 : _classPrivateFieldGet3.at(-1);
        if (!entry || entry.return_date != null) {
          return true;
        }
        ;
        entry.return_date = new Date(Date.now());
        return false;
      });
    }

    /**
     * @param {Array<LibraryBook>} books 
     */
  }, {
    key: "registerBooks",
    value: function registerBooks(books) {
      var _this3 = this;
      books.forEach(function (book) {
        var list = _classPrivateFieldGet(_books, _this3).get(book.book);
        if (list === undefined) {
          list = [];
          _classPrivateFieldGet(_books, _this3).set(book.book, list);
        }
        list.push(book);
      });
    }

    /**
     * @returns {Array<Book>}
     * @param  {object} criteria 
     */
  }, {
    key: "searchExact",
    value: function searchExact(criterias) {
      //#NF:14 spread operator
      //#NF:5 array methods (flatmap)
      //#NF:15 short if ?:
      return _toConsumableArray(_classPrivateFieldGet(_books, this).entries()).flatMap(function (_ref5) {
        var _ref6 = _slicedToArray(_ref5, 2),
          book = _ref6[0],
          books = _ref6[1];
        return Object.entries(criterias).every(function (_ref7) {
          var _ref8 = _slicedToArray(_ref7, 2),
            key = _ref8[0],
            value = _ref8[1];
          return book[key] === value;
        }) ? books : [];
      });
    }
  }, {
    key: "searchRE",
    value: function searchRE(criterias) {
      //#NF5: array methods (filter, flat, map)
      //#NF6: arrow function
      //#NF7: array deconstruction
      //#NF8: Object.entries()
      //#NF9: every()
      //#NF10: Array.from()
      //#NF11: regular expressions
      return Array.from(_classPrivateFieldGet(_books, this).entries()).filter(function (_ref9) {
        var _ref10 = _slicedToArray(_ref9, 2),
          book = _ref10[0],
          _ = _ref10[1];
        return Object.entries(criterias).every(function (_ref11) {
          var _ref12 = _slicedToArray(_ref11, 2),
            key = _ref12[0],
            value = _ref12[1];
          return new RegExp(value).test(book[key]);
        });
      }).map(function (_ref13) {
        var _ref14 = _slicedToArray(_ref13, 2),
          _ = _ref14[0],
          books = _ref14[1];
        return books;
      }).flat();
    }
  }, {
    key: "stats",
    value: function stats() {
      var _this4 = this;
      //#NF3: Template Literals `${}`
      return "\n".concat(_classPrivateFieldGet(_name, this), ", ").concat(_classPrivateFieldGet(_location, this), "\ntotal books: ").concat(Array.from(_classPrivateFieldGet(_books, this).keys()).reduce(function (acc, key) {
        return acc + _classPrivateFieldGet(_books, _this4).get(key).length;
      }, 0), "\ntotal unique books: ").concat(Array.from(_classPrivateFieldGet(_books, this).keys()).length);
    }
  }]);
}();