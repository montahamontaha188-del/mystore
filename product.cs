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
    public class Productservices
    {
        private List<Product> productsList = new();
        private int idCounter = 1;


        public void displayproductmenue()
        {
           
            while (true)
            {

                Console.WriteLine("\n--- PRODUCT MENU ---");
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. List products");
                Console.WriteLine("3. Total value");
                Console.WriteLine("4. Manage Existing Products (Search/Update/Delete) ");
                Console.WriteLine("0. Back to Main Menu");
                int choice2 = InputHelper.ReadInt("Select an option:", 0, 4);


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
                            return;
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
            
            string name = InputHelper.ReadNonEmptyString("Enter product name: ");

            double price = InputHelper.ReadDouble("Enter product price: ");

            int quantity = InputHelper.ReadInt("Enter product quantity: ",1);

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
                
                while (true)
                {
                    Console.WriteLine("\n--- MANAGE PRODUCTS SUB-MENU ---");
                    Console.WriteLine("1. Search product by name");
                    Console.WriteLine("2. Update product price");
                    Console.WriteLine("3. Update product quantity");
                    Console.WriteLine("4. Delete product by ID");
                    Console.WriteLine("0. Back to Product Menu");


                    int subChoice = InputHelper.ReadInt("Select an option: ", 0, 4);

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
                            return;
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
           
            int id = InputHelper.ReadInt("Enter product ID to update price: ") ;

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            double newPrice = InputHelper.ReadDouble($"Current Price for '{prod.Name}' is {prod.Price:0.00}. Enter new price: ",0.01);

            prod.Price = newPrice;
            Console.WriteLine("Price updated successfully!");
        }
        private void UpdateProductQuantity()
        {
            Console.Write("Enter product ID to update quantity: ");
            int id = InputHelper.ReadInt("Enter product ID to update quantity: " , 1);

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            int newQuantity = InputHelper.ReadInt($"Current Quantity for '{prod.Name}' is {prod.Quantity}. Enter new quantity: ",1);

            prod.Quantity = newQuantity;
            Console.WriteLine("Quantity updated successfully!");
        }

        private void DeleteProductById()
        {
            int id = InputHelper.ReadInt("Enter product ID to delete: ", 1);

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                Console.WriteLine("Error: Product ID not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete {prod.Name}? (y/n): ");
            string confirm = Console.ReadLine().ToLower();

            if (InputHelper.Confirm($"Are you sure you want to delete {prod.Name}?"))
            {
                productsList.Remove(prod);
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
          
        }
        public Product GetProductById(int id)
        {
            return productsList.FirstOrDefault(p => p.Id == id);
        }
    }
}

