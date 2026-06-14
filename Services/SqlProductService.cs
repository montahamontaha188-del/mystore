using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; 
using MyStore;

namespace MyStore
{
    
    public class SqlProductService : IProductService
    {

        private readonly string _connectionString;

        public SqlProductService(string connectionString)
        {
            _connectionString = connectionString;
        }

       
        public void AddProduct(Product product)
        {
            string query = @"INSERT INTO Products (Name, Price, Quantity, Category) 
                             VALUES (@Name, @Price, @Quantity, @Category)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@Category", (int)product.Category);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

      
        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT Id, Name, Price, Quantity, Category FROM Products";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDouble(2),
                                Quantity = reader.GetInt32(3),
                                Category = (Category)reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return products;
        }

        
        public Product GetProductById(int id)
        {
            string query = "SELECT Id, Name, Price, Quantity, Category FROM Products WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDouble(2),
                                Quantity = reader.GetInt32(3),
                                Category = (Category)reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        
        public IEnumerable<Product> SearchProductsByName(string name)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT Id, Name, Price, Quantity, Category FROM Products WHERE Name LIKE @Name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", "%" + name + "%");
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDouble(2),
                                Quantity = reader.GetInt32(3),
                                Category = (Category)reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return products;
        }

        
        public void UpdateProductPrice(int id, double newPrice)
        {
            string query = "UPDATE Products SET Price = @Price WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Price", newPrice);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        
        public void UpdateProductQuantity(int id, int newQuantity)
        {
            string query = "UPDATE Products SET Quantity = @Quantity WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", newQuantity);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

 
        public void DeleteProduct(int id)
        {
            string query = "DELETE FROM Products WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

 
        public double GetGrandTotalValue()
        {
            string query = "SELECT SUM(Price * Quantity) FROM Products";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
                }
            }
        }
 
        public Dictionary<Category, double> GetValueBreakdownByCategory()
        {
            Dictionary<Category, double> breakdown = new Dictionary<Category, double>();
            string query = "SELECT Category, SUM(Price * Quantity) FROM Products GROUP BY Category";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = (Category)reader.GetInt32(0);
                            double totalValue = reader.GetDouble(1);
                            breakdown.Add(category, totalValue);
                        }
                    }
                }
            }
            return breakdown;
        }

    }
}