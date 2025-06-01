using System;
using System.Collections.Generic;
using BookstoreLibrary.Models;
using BookstoreLibrary.DataAccess;

namespace BookstoreLibrary.BusinessLogic
{
    public class BookBLL
    {
        private readonly BookDAL _bookDAL;

        public BookBLL()
        {
            _bookDAL = new BookDAL();
        }

        public BookBLL(string connectionString)
        {
            _bookDAL = new BookDAL(connectionString);
        }

        public int AddBook(Book book)
        {
            // Business validation
            ValidateBook(book);

            // Additional business rules
            if (string.IsNullOrEmpty(book.ISBN) == false && IsISBNExists(book.ISBN, book.BookId))
            {
                throw new ArgumentException("ISBN already exists in the system.");
            }

            return _bookDAL.InsertBook(book);
        }

        public bool UpdateBook(Book book)
        {
            // Business validation
            ValidateBook(book);

            if (book.BookId <= 0)
            {
                throw new ArgumentException("Invalid Book ID.");
            }

            // Check if book exists
            var existingBook = _bookDAL.GetBookById(book.BookId);
            if (existingBook == null)
            {
                throw new ArgumentException("Book not found.");
            }

            // Additional business rules
            if (string.IsNullOrEmpty(book.ISBN) == false && IsISBNExists(book.ISBN, book.BookId))
            {
                throw new ArgumentException("ISBN already exists in the system.");
            }

            return _bookDAL.UpdateBook(book);
        }

        public Book GetBookById(int bookId)
        {
            if (bookId <= 0)
            {
                throw new ArgumentException("Invalid Book ID.");
            }

            return _bookDAL.GetBookById(bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _bookDAL.GetAllBooks();
        }

        public List<Book> GetBooksByCategory(string category)
        {
            var allBooks = _bookDAL.GetAllBooks();
            return allBooks.FindAll(b => string.Equals(b.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Book>();
            }

            var allBooks = _bookDAL.GetAllBooks();
            return allBooks.FindAll(b =>
                b.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.Author.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (b.ISBN != null && b.ISBN.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        private void ValidateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new ArgumentException("Book title is required.");
            }

            if (string.IsNullOrWhiteSpace(book.Author))
            {
                throw new ArgumentException("Book author is required.");
            }

            if (book.Price <= 0)
            {
                throw new ArgumentException("Book price must be greater than zero.");
            }

            if (book.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.");
            }

            // ISBN validation (basic)
            if (!string.IsNullOrEmpty(book.ISBN) && book.ISBN.Length < 10)
            {
                throw new ArgumentException("ISBN must be at least 10 characters long.");
            }
        }

        private bool IsISBNExists(string isbn, int excludeBookId = 0)
        {
            var allBooks = _bookDAL.GetAllBooks();
            return allBooks.Exists(b => b.ISBN == isbn && b.BookId != excludeBookId);
        }
    }
}