using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;  
using MyStore;

namespace MyStore
{

    public class SqlCustomerService : ICustomerService
    {
        private readonly string _connectionString;

        public SqlCustomerService(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void AddCustomer(Customer customer)
        {
            string query = @"INSERT INTO Customers (Name, PhoneNumber) 
                             VALUES (@Name, @PhoneNumber)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

     
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT Id, Name, PhoneNumber FROM Customers";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PhoneNumber = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return customers;
        }


        public Customer GetCustomerById(int id)
        {
            string query = "SELECT Id, Name, PhoneNumber FROM Customers WHERE Id = @Id";

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
                            return new Customer
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PhoneNumber = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

    
        public IEnumerable<Customer> FindCustomersByName(string name)
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT Id, Name, PhoneNumber FROM Customers WHERE Name LIKE @Name";

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
                            customers.Add(new Customer
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PhoneNumber = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return customers;
        }


        public bool IsPhoneNumberExists(string phoneNumber)
        {
            string query = "SELECT COUNT(1) FROM Customers WHERE PhoneNumber = @PhoneNumber";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}