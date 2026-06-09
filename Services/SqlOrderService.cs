using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MyStore;

namespace MyStore
{
    public class SqlOrderService : IOrderService
    {
      
        private readonly string _connectionString = "Server=.;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;";

        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
               
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        DateTime orderDate = (order.OrderDate < new DateTime(1753, 1, 1)) ? DateTime.Now : order.OrderDate;

                      
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
                        throw new Exception("خطأ أثناء حفظ الطلبية: " + ex.Message);
                    }
                }
            }
        }

        public IEnumerable<Order> GetAllOrders() => new List<Order>();
        public Order GetOrderById(int id) => null;
    }
}