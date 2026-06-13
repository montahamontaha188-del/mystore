using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MyStore;

namespace MyStore
{
    public class SqlReportService : IReportService
    {
        private readonly string _connectionString = "Server=DESKTOP-9LFOAF6\\SQLEXPRESS;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;";

         public IEnumerable<dynamic> GetBestSellingProducts(IEnumerable<Order> orders)
        {
            List<dynamic> results = new List<dynamic>();
            string query = @"SELECT TOP 5 p.Name, SUM(oi.Quantity) as TotalQty
                             FROM OrderItems oi
                             JOIN Products p ON oi.ProductId = p.Id
                             GROUP BY p.Id, p.Name
                             ORDER BY TotalQty DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new { ProductName = reader.GetString(0), TotalQuantity = reader.GetInt32(1) });
                        }
                    }
                }
            }
            return results;
        }

         public IEnumerable<dynamic> GetTopCustomers(IEnumerable<Order> orders)
        {
            List<dynamic> results = new List<dynamic>();
             string query = @"SELECT TOP 5 c.Name, SUM(oi.Quantity * oi.Price) as TotalSpent
                             FROM Orders o
                             JOIN Customers c ON o.CustomerId = c.Id
                             JOIN OrderItems oi ON o.Id = oi.OrderId
                             GROUP BY c.Id, c.Name
                             ORDER BY TotalSpent DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new
                            {
                                CustomerName = reader.GetString(0),
                                TotalSpent = Convert.ToDouble(reader.GetDouble(1))
                            });
                        }
                    }
                }
            }
            return results;
        }

         public (int TotalOrders, double TotalRevenue) GetDailySalesSummary(IEnumerable<Order> orders, DateTime date)
        {
 
            string query = @"SELECT COUNT(DISTINCT o.Id), ISNULL(SUM(oi.Quantity * oi.Price), 0) 
                             FROM Orders o
                             JOIN OrderItems oi ON o.Id = oi.OrderId
                             WHERE CAST(o.OrderDate AS DATE) = CAST(@Date AS DATE)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (reader.GetInt32(0), reader.GetDouble(1));
                        }
                    }
                }
            }
            return (0, 0);
        }
 
        public IEnumerable<Product> GetLowStockProducts(IEnumerable<Product> products, int threshold)
        {
            List<Product> lowStockProducts = new List<Product>();
            string query = @"SELECT Id, Name, Price, Quantity, Category 
                             FROM Products WHERE Quantity <= @Threshold";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Threshold", threshold);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lowStockProducts.Add(new Product
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
            return lowStockProducts;
        }
    }
}