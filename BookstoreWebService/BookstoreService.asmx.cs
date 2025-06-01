using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Services;
using BookstoreLibrary.Models;
using BookstoreLibrary.BusinessLogic;
using System.Data.SqlClient;

namespace BookstoreWebService
{
    /// <summary>
    /// Summary description for BookstoreService
    /// </summary>
    [WebService(Namespace = "http://bookstore.webservice/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class BookstoreService : System.Web.Services.WebService
    {
        private readonly BookBLL _bookBLL;

        public BookstoreService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BookstoreConnection"].ConnectionString;
            _bookBLL = new BookBLL(connectionString);
        }

        [WebMethod]
        public int InsertBook(string title, string author, string isbn, decimal price,
            DateTime? publishedDate, string category, string description, int stockQuantity)
        {
            try
            {
                var book = new Book
                {
                    Title = title,
                    Author = author,
                    ISBN = isbn,
                    Price = price,
                    PublishedDate = publishedDate,
                    Category = category,
                    Description = description,
                    StockQuantity = stockQuantity
                };

                return _bookBLL.AddBook(book);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting book: {ex.Message}");
            }
        }

        [WebMethod]
        public bool UpdateBook(int bookId, string title, string author, string isbn, decimal price,
            DateTime? publishedDate, string category, string description, int stockQuantity)
        {
            try
            {
                var book = new Book
                {
                    BookId = bookId,
                    Title = title,
                    Author = author,
                    ISBN = isbn,
                    Price = price,
                    PublishedDate = publishedDate,
                    Category = category,
                    Description = description,
                    StockQuantity = stockQuantity
                };

                return _bookBLL.UpdateBook(book);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating book: {ex.Message}");
            }
        }

        [WebMethod]
        public Book GetBookById(int bookId)
        {
            try
            {
                return _bookBLL.GetBookById(bookId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving book: {ex.Message}");
            }
        }

        [WebMethod]
        public List<Book> GetAllBooks()
        {
            try
            {
                return _bookBLL.GetAllBooks();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving books: {ex.Message}");
            }
        }

        [WebMethod]
        public List<Book> GetBooksByCategory(string category)
        {
            try
            {
                return _bookBLL.GetBooksByCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving books by category: {ex.Message}");
            }
        }

        [WebMethod]
        public List<Book> SearchBooks(string searchTerm)
        {
            try
            {
                return _bookBLL.SearchBooks(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching books: {ex.Message}");
            }
        }

        [WebMethod]
        public string GetDatabaseName()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookstoreConnection"].ConnectionString))
                {
                    connection.Open();
                    return connection.Database;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving database name: {ex.Message}");
            }
        }

        [WebMethod]
        public string GetServerInfo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookstoreConnection"].ConnectionString))
                {
                    connection.Open();
                    return $"Database: {connection.Database}, Server: {connection.DataSource}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving server info: {ex.Message}");
            }
        }
    }
}