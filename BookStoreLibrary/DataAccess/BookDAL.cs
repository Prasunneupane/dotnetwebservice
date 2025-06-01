using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BookstoreLibrary.Models;

namespace BookstoreLibrary.DataAccess
{
    public class BookDAL
    {
        private readonly string _connectionString;

        public BookDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BookstoreConnection"].ConnectionString;
        }

        public BookDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int InsertBook(Book book)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_InsertBook", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Title", book.Title);
                        command.Parameters.AddWithValue("@Author", book.Author);
                        command.Parameters.AddWithValue("@ISBN", book.ISBN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", book.Price);
                        command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Category", book.Category ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StockQuantity", book.StockQuantity);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting book: {ex.Message}", ex);
            }
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_UpdateBook", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@BookId", book.BookId);
                        command.Parameters.AddWithValue("@Title", book.Title);
                        command.Parameters.AddWithValue("@Author", book.Author);
                        command.Parameters.AddWithValue("@ISBN", book.ISBN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", book.Price);
                        command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Category", book.Category ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", book.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StockQuantity", book.StockQuantity);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating book: {ex.Message}", ex);
            }
        }

        public Book GetBookById(int bookId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetBookById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BookId", bookId);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToBook(reader);
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving book by ID: {ex.Message}", ex);
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllBooks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                books.Add(MapReaderToBook(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all books: {ex.Message}", ex);
            }

            return books;
        }

        private Book MapReaderToBook(SqlDataReader reader)
        {
            return new Book
            {
                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Author = reader.GetString(reader.GetOrdinal("Author")),
                ISBN = reader.IsDBNull(reader.GetOrdinal("ISBN")) ? null : reader.GetString(reader.GetOrdinal("ISBN")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                PublishedDate = reader.IsDBNull(reader.GetOrdinal("PublishedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PublishedDate")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate"))
            };
        }
    }
}