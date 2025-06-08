using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using NewBookStoreApplication.Models;
using Newtonsoft.Json;
namespace NewBookStoreApplication
{
    [WebService(Namespace = "http://bookstore.webservice/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class BookService : System.Web.Services.WebService
    {
        private readonly string _connectionString;

        // Constructor - Dependency Injection for connection string
        public BookService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BookServiceConnection"].ConnectionString;
        }

        // Constructor overload for testing or custom connection string
        public BookService(string connectionString)
        {
            _connectionString = connectionString;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World - Bookstore Service is running!";
        }

        [WebMethod]
        public string GetDatabaseName()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.Database;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        [WebMethod]
        public string GetServerInfo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return $"Database: {connection.Database}, Server: {connection.DataSource}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        [WebMethod]
        public List<string> GetAllBookTitles()
        {
            try
            {
                // Sample method - using connection string from constructor
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    // Later you can add actual database query here
                    return new List<string> { "Sample Book 1", "Sample Book 2", "Sample Book 3" };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving books: {ex.Message}");
            }
        }

        [WebMethod]
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public string GetConnectionStatus()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return $"✅ Connected successfully to {connection.Database} on {connection.DataSource}";
                }
            }
            catch (Exception ex)
            {
                return $"❌ Connection failed: {ex.Message}";
            }
        }


        [WebMethod]
        public string InsertBook(
             string title,
             string author,
             string isbn,
             decimal price,
             DateTime publishedDate,
             string category,
             string description,
             int stockQuantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertBook", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@ISBN", isbn);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@PublishedDate", publishedDate);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

                        object result = cmd.ExecuteScalar(); // returns BookId
                        return $"✅ Book inserted successfully with ID: {result}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"❌ Error inserting book: {ex.Message}";
            }
        }

        [WebMethod]
        public string GetAllBooks()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    // 1. Create the command for the stored procedure
                    using (SqlCommand cmd = new SqlCommand("sp_GetAllBooks", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // 2. Use SqlDataAdapter to fill the DataSet
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        // 3. Fill the DataSet
                        adapter.Fill(ds, "Books");

                        // 4. Convert the first DataTable to JSON using Newtonsoft
                        if (ds.Tables.Count > 0)
                        {
                            return JsonConvert.SerializeObject(ds.Tables["Books"]);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { message = "No records found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }


        [WebMethod]
        public string GetBooksByCategoryId(string categoryId)
        {
            try
            {
                int catId = Convert.ToInt32(categoryId); // Convert string to int

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetBooksByCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass the category ID as a parameter
                        cmd.Parameters.AddWithValue("@categoryID", catId);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        adapter.Fill(ds, "Books");

                        if (ds.Tables.Count > 0 && ds.Tables["Books"].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(ds.Tables["Books"]);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { message = "No records found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
      }


        [WebMethod]
        public string GetBooksByTitle(string title)
        {
            try
            {
                //int catId = Convert.ToInt32(categoryId); // Convert string to int

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SearchBooksByTitle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass the category ID as a parameter
                        cmd.Parameters.AddWithValue("@bookTitle", title);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        adapter.Fill(ds, "Books");

                        if (ds.Tables.Count > 0 && ds.Tables["Books"].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(ds.Tables["Books"]);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { message = "No records found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        [WebMethod]
        public string GetBooksByPriceRange(decimal upperPrice, decimal lowerPrice = 0)


        {
            try
            {
                //int catId = Convert.ToInt32(categoryId); // Convert string to int

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetBooksByPriceRange", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass the category ID as a parameter
                        cmd.Parameters.AddWithValue("@lowerPrice", lowerPrice);
                        cmd.Parameters.AddWithValue("@upperPrice", upperPrice);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        adapter.Fill(ds, "Books");

                        if (ds.Tables.Count > 0 && ds.Tables["Books"].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(ds.Tables["Books"]);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { message = "No records found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }


        [WebMethod]
        public string GetTopRatedBooks()


        {
            try
            {
                //int catId = Convert.ToInt32(categoryId); // Convert string to int

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetTopRatedBooks", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //// Pass the category ID as a parameter
                        //cmd.Parameters.AddWithValue("@lowerPrice", lowerPrice);
                        //cmd.Parameters.AddWithValue("@lowerPrice", upperPrice);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        adapter.Fill(ds, "Books");

                        if (ds.Tables.Count > 0 && ds.Tables["Books"].Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(ds.Tables["Books"]);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { message = "No records found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }


    }
}