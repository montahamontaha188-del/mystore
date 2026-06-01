using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStoreSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            StoreManager store = new StoreManager();

            while (true)
            {
                Console.WriteLine("\n---  MY STORE SYSTEM ---");
                Console.WriteLine("1. Product");
                Console.WriteLine("2. Customers");
                Console.WriteLine("3. Orders");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                // تم إزالة النقطتين الرأسيتين من هنا
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("\n--- PRODUCT MENU ---");
                            Console.WriteLine("1. Add product");
                            Console.WriteLine("2. List products");
                            Console.WriteLine("3. Total value");
                            Console.WriteLine("0. Back to Main Menu");
                            Console.Write("Select an option: ");

                            int choice2 = Convert.ToInt32(Console.ReadLine());

                            if (choice2 == 1)
                            {
                                Console.Write("Enter product name: ");
                                string name = Console.ReadLine();

                                Console.Write("Enter product price: ");
                                double price = Convert.ToDouble(Console.ReadLine());

                                Console.Write("Enter product quantity: ");
                                int quantity = Convert.ToInt32(Console.ReadLine());

                                store.AddProduct(name, price, quantity);
                            }
                            else if (choice2 == 2)
                            {
                                store.ListProducts();
                            }
                            else if (choice2 == 3)
                            {
                                store.ShowTotalValue();
                            }
                            else if (choice2 == 0)
                            {
                                // هنا يعود تلقائياً للقائمة الرئيسية بفضل الـ break الخاصة بالـ switch
                                Console.WriteLine("Returning to main menu...");
                            }
                            else
                            {
                                Console.WriteLine("Invalid option. Please try again.");
                            }
                            break;
                        }
                    case 0:
                        {
                            Console.WriteLine("Finished. Goodbye!");
                            return; // تم استخدام return لإغلاق البرنامج بالكامل وإنهاء الحلقة اللانهائية
                        }
                    default:
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                        }
                }
            }
        }
    }

    public class StoreManager
    {
        private List<Product> productsList = new();
        private int idCounter = 1;

        public void AddProduct(string name, double price, int quantity)
        {
            Product newProduct = new Product
            {
                Id = idCounter++,
                Name = name,
                Price = price,
                Quantity = quantity
            };

            productsList.Add(newProduct);
            Console.WriteLine($"Product '{name}' added successfully with ID: {newProduct.Id}");
        }

        public void ListProducts()
        {
            if (productsList.Count == 0)
            {
                Console.WriteLine("No products found in inventory.");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|-----------|----------");

            foreach (var prod in productsList)
            {
                Console.WriteLine(
                    prod.Id.ToString().PadRight(5) + "| " +
                    prod.Name.PadRight(15) + "| " +
                    prod.Price.ToString("0.00").PadRight(10) + "| " +
                    prod.Quantity
                );
            }
        }

        public void ShowTotalValue()
        {
            if (productsList.Count == 0)
            {
                Console.WriteLine("Inventory is empty. Total Value: 0.00");
                return;
            }

            double totalValue = productsList.Sum(p => p.Price * p.Quantity);
            Console.WriteLine($"\nTotal Inventory Value: {totalValue:0.00}");
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}