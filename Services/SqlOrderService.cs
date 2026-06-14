using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MyStore;

namespace MyStore
{
    public class SqlOrderService : IOrderService
    {

        private readonly string _connectionString;

        public SqlOrderService(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
               
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        DateTime orderDate = DateTime.Now ;

                      
                        string orderQuery = @"INSERT INTO Orders (CustomerId, OrderDate, AppliedDiscountCode, DiscountPercentage) 
                                             VALUES (@CustomerId, @OrderDate, @AppliedDiscountCode, @DiscountPercentage);
                                             SELECT SCOPE_IDENTITY();";

                        int newOrderId;
                        using (SqlCommand cmd = new SqlCommand(orderQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerId", order.Customer.Id);
                            cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                            cmd.Parameters.AddWithValue("@AppliedDiscountCode", (object)order.AppliedDiscountCode ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@DiscountPercentage", order.DiscountPercentage);
                            newOrderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        order.Id = newOrderId;

                        string itemQuery = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) 
                                             VALUES (@OrderId, @ProductId, @Quantity, @Price)";

                        foreach (var item in order.Items)
                        {
                            using (SqlCommand cmd = new SqlCommand(itemQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", newOrderId);
                                cmd.Parameters.AddWithValue("@ProductId", item.Product.Id);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("@Price", item.Product.Price);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); 
                        throw new Exception("error " + ex.Message);
                    }
                }
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
     
            var ordersMap = new Dictionary<int, Order>();

            string query = @" SELECT o.Id AS OrderId, o.OrderDate, o.AppliedDiscountCode, o.DiscountPercentage,
                       c.Id AS CustomerId, c.Name AS CustomerName, c.PhoneNumber,
                       oi.Quantity, oi.Price AS ItemPrice,
                       p.Id AS ProductId, p.Name AS ProductName
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerId = c.Id
                LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
                LEFT JOIN Products p ON oi.ProductId = p.Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = Convert.ToInt32(reader["OrderId"]);

                           
                            if (!ordersMap.TryGetValue(orderId, out var order))
                            {
                                order = new Order
                                {
                                    Id = orderId,
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    AppliedDiscountCode = reader["AppliedDiscountCode"] == DBNull.Value ? null : reader["AppliedDiscountCode"].ToString(),
                                    DiscountPercentage = Convert.ToDouble(reader["DiscountPercentage"]),
                                    Customer = new Customer
                                    {
                                        Id = Convert.ToInt32(reader["CustomerId"]),
                                        Name = reader["CustomerName"].ToString(),
                                        PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? "" : reader["PhoneNumber"].ToString()
                                    }
                                };
                                ordersMap.Add(orderId, order);
                            }

                           
                            if (reader["ProductId"] != DBNull.Value)
                            {
                                var item = new OrderItem
                                {
                                    Product = new Product
                                    {
                                        Id = Convert.ToInt32(reader["ProductId"]),
                                        Name = reader["ProductName"].ToString(),
                                        Price = Convert.ToDouble(reader["ItemPrice"])
                                    },
                                    Quantity = Convert.ToInt32(reader["Quantity"])
                                };
                                order.Items.Add(item);
                            }
                        }
                    }
                }
            }
            return ordersMap.Values;
        }

        
        public Order GetOrderById(int id)
        {
          
            return GetAllOrders().FirstOrDefault(o => o.Id == id);
        }
    }
}