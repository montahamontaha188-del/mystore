
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
        public Category Category { get; set; }
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
                Console.WriteLine("4. Filter products by Category");
                Console.WriteLine("5. Manage Existing Products (Search/Update/Delete) ");
                Console.WriteLine("0. Back to Main Menu");
                int choice2 = InputHelper.ReadInt("Select an option:", 0, 5);


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
                        FilterByCategory();
                        break;
                    case 5:
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
            int quantity = InputHelper.ReadInt("Enter product quantity: ", 0);
            


            Console.WriteLine("\nSelect a Category:");
            var categories = (Category[])Enum.GetValues(typeof(Category)); 

            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}"); 
            }

           
            int categoryChoice = InputHelper.ReadInt("Select an option: ", 1, categories.Length);
            Category selectedCategory = categories[categoryChoice - 1];
           

            Product newProduct = new Product
            {
                Id = idCounter ++,
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = selectedCategory 
            };

            productsList.Add(newProduct);
            //addproduct(newproduct)
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Product '{name}' added successfully under category '{selectedCategory}' with ID: {newProduct.Id}");
            Console.ResetColor();
        }

        private void ListProducts()
        {
            if (productsList.Count == 0)
            {
                throw new BusinessException("No products found.");
               
            }

          
            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Category".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|----------------|----------|----------");

            foreach (var prod in productsList)
            {
                Console.WriteLine(
                    prod.Id.ToString().PadRight(5) + "| " +
                    prod.Name.PadRight(15) + "| " +
                    prod.Category.ToString().PadRight(15) + "| " + 
                    prod.Price.ToString("0.00").PadRight(10) + "| " +
                    prod.Quantity
                );
            }
        }

        private void ShowTotalValue()
        {
            if (productsList.Count == 0)
            {
                throw new BusinessException("Inventory is empty. Total Value: 0.00");
              
            }

            Console.WriteLine("\n--- Inventory Value Breakdown by Category ---");
            var categoryGroups = productsList.GroupBy(p => p.Category);
            foreach (var group in categoryGroups)
            {
                double categoryTotal = group.Sum(p => p.Price * p.Quantity);
                Console.WriteLine($"{group.Key.ToString().PadRight(15)} : {categoryTotal:0.00}");
            }

            Console.WriteLine("---------------------------------------------");
            double grandTotal = productsList.Sum(p => p.Price * p.Quantity);
            Console.WriteLine($"Total Store Value: {grandTotal:0.00}");
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
            string searchName = InputHelper.ReadNonEmptyString("Enter product name to search: ");

            var results = productsList.Where(n => n.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (results.Count == 0)
            {
                throw new BusinessException("Error: No products found matching that name.");
              
            }

            Console.WriteLine("\n--- Search Results ---");
            foreach (var prod in results)
            {
                Console.WriteLine($"ID: {prod.Id} | Name: {prod.Name} | Price: {prod.Price:0.00} | Quantity: {prod.Quantity}");
            }
        }
        private void UpdateProductPrice()
        {
           
            int id = InputHelper.ReadInt("Enter product ID to update price: ",1, idCounter-1) ;

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
              
            }

            double newPrice = InputHelper.ReadDouble($"Current Price for '{prod.Name}' is {prod.Price:0.00}. Enter new price: ",0.01);

            prod.Price = newPrice;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Price updated successfully!");
            Console.ResetColor();
        }
        private void UpdateProductQuantity()
        {
          
            int id = InputHelper.ReadInt("Enter product ID to update quantity: " , 1, idCounter - 1);

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
             
            }

            int newQuantity = InputHelper.ReadInt($"Current Quantity for '{prod.Name}' is {prod.Quantity}. Enter new quantity: ",0);

            prod.Quantity = newQuantity;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Quantity updated successfully!");
            Console.ResetColor();
        }

        private void DeleteProductById()
        {
            int id = InputHelper.ReadInt("Enter product ID to delete: ", 1, idCounter-1 );

            var prod = productsList.FirstOrDefault(p => p.Id == id);

            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }
            if (InputHelper.Confirm($"Are you sure you want to delete {prod.Name}?"))
            {
                productsList.Remove(prod);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product deleted successfully.");
                Console.ResetColor();
              
                for (int i = 0; i < productsList.Count; i++)
                {
                    productsList[i].Id = i + 1;
                }

               
              

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

        private void FilterByCategory()
        {
            Console.WriteLine("\nSelect a Category to filter by:");
            var categories = (Category[])Enum.GetValues(typeof(Category));
            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            int choice = InputHelper.ReadInt("Select an option: ", 1, categories.Length);
            Category selectedCategory = categories[choice - 1];


            var filteredList = productsList.Where(p => p.Category == selectedCategory).ToList();

            if (filteredList.Count == 0)
            {
                throw new BusinessException($"\nNo products found under category: {selectedCategory}");

            }


            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Category".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|----------------|----------|----------");

            foreach (var prod in filteredList)
            {
                Console.WriteLine(
                    prod.Id.ToString().PadRight(5) + "| " +
                    prod.Name.PadRight(15) + "| " +
                    prod.Category.ToString().PadRight(15) + "| " +
                    prod.Price.ToString("0.00").PadRight(10) + "| " +
                    prod.Quantity
                );
            }
        }
            public List<Product> GetProductsList()
        {
            return productsList;
        }
    }
        
    }



