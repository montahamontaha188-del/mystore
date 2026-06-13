using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; 
using MyStore;

namespace MyStore
{

    public class SqlDiscountService : IDiscountService
    {
        private readonly string _connectionString = "Server=DESKTOP-9LFOAF6\\SQLEXPRESS;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;";

      
        public void AddDiscount(Discount discount)
        {
            string query = @"INSERT INTO Discounts (Code, Percentage, IsActive) 
                             VALUES (@Code, @Percentage, @IsActive)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", discount.Code);
                    command.Parameters.AddWithValue("@Percentage", discount.Percentage);
                    command.Parameters.AddWithValue("@IsActive", discount.IsActive);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

       
        public IEnumerable<Discount> GetAllDiscounts()
        {
            List<Discount> discountsList = new List<Discount>();
            string query = "SELECT Id, Code, Percentage, IsActive FROM Discounts";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            discountsList.Add(new Discount
                            {
                                Id = reader.GetInt32(0),
                                Code = reader.GetString(1),
                                Percentage = reader.GetDouble(2),
                                IsActive = reader.GetBoolean(3)
                            });
                        }
                    }
                }
            }
            return discountsList;
        }

     
        public Discount GetDiscountByCode(string code)
        {
            string query = "SELECT Id, Code, Percentage, IsActive FROM Discounts WHERE Code = @Code";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Discount
                            {
                                Id = reader.GetInt32(0),
                                Code = reader.GetString(1),
                                Percentage = reader.GetDouble(2),
                                IsActive = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }
            return null;
        }


        public Discount GetActiveDiscount(string code)
        {
            string query = "SELECT Id, Code, Percentage, IsActive FROM Discounts WHERE Code = @Code AND IsActive = 1";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Discount
                            {
                                Id = reader.GetInt32(0),
                                Code = reader.GetString(1),
                                Percentage = reader.GetDouble(2),
                                IsActive = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool IsCodeExists(string code)
        {
            string query = "SELECT COUNT(1) FROM Discounts WHERE Code = @Code";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}