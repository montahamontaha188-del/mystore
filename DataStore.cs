using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    namespace MyStore
    {
        public class DataStore
        {
        
            private readonly string productsPath = "products.json";
            private readonly string customersPath = "customers.json"; 
            private readonly string ordersPath = "orders.json";
            private readonly string discountsPath = "discounts.json";

         
            public void SaveProducts(List<Product> products)
            {
                try
                {
              
                    var options = new JsonSerializerOptions { WriteIndented = true };  
                    string jsonString = JsonSerializer.Serialize(products, options);

                
                    File.WriteAllText(productsPath, jsonString);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to save products to JSON file: " + ex.Message);
                }
            }

      
            public List<Product> LoadProducts()
            {
                try
                {
         
                    if (!File.Exists(productsPath))
                    {
                        return new List<Product>();
                    }

                
                    string jsonString = File.ReadAllText(productsPath);

                
                    return JsonSerializer.Deserialize<List<Product>>(jsonString) ?? new List<Product>();
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to load products from JSON file: " + ex.Message);
                }
            }
      
        public void SaveCustomers(List<Customer> customers)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(customers, options);
                    File.WriteAllText(customersPath, jsonString);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to save customers to JSON file: " + ex.Message);
                }
            }

         
            public List<Customer> LoadCustomers()
            {
                try
                {
                
                    if (!File.Exists(customersPath))
                    {
                        return new List<Customer>();
                    }

                    string jsonString = File.ReadAllText(customersPath);
                    return JsonSerializer.Deserialize<List<Customer>>(jsonString) ?? new List<Customer>();
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to load customers from JSON file: " + ex.Message);
                }
            }
            public void SaveOrders(List<Order> orders)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(orders, options);
                    File.WriteAllText(ordersPath, jsonString);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to save orders to JSON file: " + ex.Message);
                }
            }

            public List<Order> LoadOrders()
            {
                try
                {
                    if (!File.Exists(ordersPath)) return new List<Order>();
                    string jsonString = File.ReadAllText(ordersPath);
                    return JsonSerializer.Deserialize<List<Order>>(jsonString) ?? new List<Order>();
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to load orders from JSON file: " + ex.Message);
                }
            }
            public void SaveDiscounts(List<Discount> discounts)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(discounts, options);
                    File.WriteAllText(discountsPath, jsonString);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to save discounts to JSON file: " + ex.Message);
                }
            }

         
            public List<Discount> LoadDiscounts()
            {
                try
                {
                    if (!File.Exists(discountsPath))
                    {
                        return new List<Discount>();
                    }

                    string jsonString = File.ReadAllText(discountsPath);
                    return JsonSerializer.Deserialize<List<Discount>>(jsonString) ?? new List<Discount>();
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to load discounts from JSON file: " + ex.Message);
                }
            }
        }
    }
}
