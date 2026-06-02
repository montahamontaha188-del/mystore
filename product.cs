using MyStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
    public class Productclass
    {
        private List<Product> productsList = new();
        private int idCounter = 1;


        public void displayproductmenue()
        {
            bool inProductMenu = true;
            while (inProductMenu)
            {

                Console.WriteLine("\n--- PRODUCT MENU ---");
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. List products");
                Console.WriteLine("3. Total value");
                Console.WriteLine("4. Manage Existing Products (Search/Update/Delete) ");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");
                int choice2 = Convert.ToInt32(Console.ReadLine());


                switch (choice2)
                {
                    case 1:
                        {
                            AddProduct();
                            break;
                        }
                    case 2:

                        {
                            ListProducts();
                            break;
                        }

                    case 3:
                        {
                            ShowTotalValue();
                            break;
                        }
                    case 4:
                        ManageMenu();
                        break;
                    case 0:

                        {

                            Console.WriteLine("Returning to main menu...");
                            inProductMenu = false;
                            break;

                        }
                    default:
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                        }
                }
            }
        }
        private void AddProduct()
        {
            Console.Write("Enter product name: ");
            string name = Console.ReadLine()?.Trim();

            Console.Write("Enter product price: ");
            double price = Convert.ToDouble(Console.ReadLine()?.Trim());

            Console.Write("Enter product quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine()?.Trim());
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

        private void ListProducts()
        {
            if (productsList.Count == 0)
            {
                Console.WriteLine("No products found .");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|----------|----------");

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

        private void ShowTotalValue()
        {
            if (productsList.Count == 0)
            {
                Console.WriteLine("Inventory is empty. Total Value: 0.00");
                return;
            }

            double totalValue = productsList.Sum(p => p.Price * p.Quantity);
            Console.WriteLine($"\nTotal Inventory Value: {totalValue:0.00}");
        }
        private void ManageMenu()
        {
            {
                bool inManageMenu = true;
                while (inManageMenu)
                {
                    Console.WriteLine("\n--- MANAGE PRODUCTS SUB-MENU ---");
                    Console.WriteLine("1. Search product by name");
                    Console.WriteLine("2. Update product price");
                    Console.WriteLine("3. Update product quantity");
                    Console.WriteLine("4. Delete product by ID");
                    Console.WriteLine("0. Back to Product Menu");
                    Console.Write("Select an option: ");

                    int subChoice = Convert.ToInt32(Console.ReadLine());

                    switch (subChoice)
                    {
                        case 1:
                            SearchProductByName();
                            break;
                        case 2:
                            UpdateProductPrice();
                            break;
                        case 3:
                            UpdateProductQuantity();
                            break;
                        case 4:
                            DeleteProductById();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Product Menu...");
                            inManageMenu = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }
        private void SearchProductByName()
        {
            Console.Write("Enter product name to search: ");
            string searchName = Console.ReadLine();

            var results = productsList.Where(n => n.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("Error: No products found matching that name.");
                return;
            }

            Console.WriteLine("\n--- Search Results ---");
            foreach (var prod in results)
            {
                Console.WriteLine($"ID: {prod.Id} | Name: {prod.Name} | Price: {prod.Price:0.00} | Quantity: {prod.Quantity}");
            }
        }
        private void UpdateProductPrice()
        {
            Console.Write("Enter product ID to update price: ");
            int id = Convert.ToInt32(Console.ReadLine());

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            Console.Write($"Current Price for '{prod.Name}' is {prod.Price:0.00}. Enter new price: ");
            double newPrice = Convert.ToDouble(Console.ReadLine());

            if (newPrice <= 0)
            {
                Console.WriteLine("Error: Price must be a positive number.");
                return;
            }

            prod.Price = newPrice;
            Console.WriteLine("Price updated successfully!");
        }
        private void UpdateProductQuantity()
        {
            Console.Write("Enter product ID to update quantity: ");
            int id = Convert.ToInt32(Console.ReadLine());

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            Console.Write($"Current Quantity for '{prod.Name}' is {prod.Quantity}. Enter new quantity: ");
            int newQuantity = Convert.ToInt32(Console.ReadLine());

            if (newQuantity < 0)
            {
                Console.WriteLine("Error: Quantity cannot be negative.");
                return;
            }

            prod.Quantity = newQuantity;
            Console.WriteLine("Quantity updated successfully!");
        }

        private void DeleteProductById()
        {
            Console.Write("Enter product ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Product prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete {prod.Name}? (y/n): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm == "y" || confirm == "yes")
            {
                productsList.Remove(prod);
                Console.WriteLine("Product deleted successfully.");
            }
            else if (confirm ==  "n")
            {
                Console.WriteLine("Deletion cancelled.");
            }
            else
            {
                Console.WriteLine("print leter n to canccel and leter y to comfirm deleting .");
            }
        }
    }
}

