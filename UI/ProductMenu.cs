using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ProductMenu
    {
        private readonly IProductService _productService;

        // حقن الخدمة عبر الدالة البنائية
        public ProductMenu(IProductService productService)
        {
            _productService = productService;
        }

        public void DisplayProductMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- PRODUCT MENU ---");
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. List products");
                Console.WriteLine("3. Total value");
                Console.WriteLine("4. Filter products by Category");
                Console.WriteLine("5. Manage Existing Products (Search/Update/Delete)");
                Console.WriteLine("0. Back to Main Menu");

                int choice = InputHelper.ReadInt("Select an option:", 0, 5);

                switch (choice)
                {
                    case 1: AddProduct(); break;
                    case 2: ListProducts(); break;
                    case 3: ShowTotalValue(); break;
                    case 4: FilterByCategory(); break;
                    case 5: ManageMenu(); break;
                    case 0: Console.WriteLine("Returning to main menu..."); return;
                    default: Console.WriteLine("Invalid choice. Please try again."); break;
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
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = selectedCategory
            };

            _productService.AddProduct(newProduct);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Product '{name}' added successfully under category '{selectedCategory}'!");
            Console.ResetColor();
        }

        private void ListProducts()
        {
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                throw new BusinessException("No products found.");
            }

            PrintProductsTable(products);
        }

        private void ShowTotalValue()
        {
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                throw new BusinessException("Inventory is empty. Total Value: 0.00");
            }

            Console.WriteLine("\n--- Inventory Value Breakdown by Category ---");
            var breakdown = _productService.GetValueBreakdownByCategory();
            foreach (var item in breakdown)
            {
                Console.WriteLine($"{item.Key.ToString().PadRight(15)} : {item.Value:0.00}");
            }

            Console.WriteLine("---------------------------------------------");
            double grandTotal = _productService.GetGrandTotalValue();
            Console.WriteLine($"Total Store Value: {grandTotal:0.00}");
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

            var filtered = _productService.GetAllProducts().Where(p => p.Category == selectedCategory);

            if (!filtered.Any())
            {
                throw new BusinessException($"\nNo products found under category: {selectedCategory}");
            }

            PrintProductsTable(filtered);
        }

        private void ManageMenu()
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
                    case 1: SearchProductByName(); break;
                    case 2: UpdateProductPrice(); break;
                    case 3: UpdateProductQuantity(); break;
                    case 4: DeleteProductById(); break;
                    case 0: Console.WriteLine("Returning to Product Menu..."); return;
                }
            }
        }

        private void SearchProductByName()
        {
            string searchName = InputHelper.ReadNonEmptyString("Enter product name to search: ");
            var results = _productService.SearchProductsByName(searchName);

            if (!results.Any())
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
            int id = InputHelper.ReadInt("Enter product ID to update price: ", 1);
            var prod = _productService.GetProductById(id);

            if (prod == null) { throw new BusinessException("Error: Product ID not found."); }

            double newPrice = InputHelper.ReadDouble($"Current Price for '{prod.Name}' is {prod.Price:0.00}. Enter new price: ", 0.01);
            _productService.UpdateProductPrice(id, newPrice);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Price updated successfully!");
            Console.ResetColor();
        }

        private void UpdateProductQuantity()
        {
            int id = InputHelper.ReadInt("Enter product ID to update quantity: ", 1);
            var prod = _productService.GetProductById(id);

            if (prod == null) { throw new BusinessException("Error: Product ID not found."); }

            int newQuantity = InputHelper.ReadInt($"Current Quantity for '{prod.Name}' is {prod.Quantity}. Enter new quantity: ", 0);
            _productService.UpdateProductQuantity(id, newQuantity);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Quantity updated successfully!");
            Console.ResetColor();
        }

        private void DeleteProductById()
        {
            int id = InputHelper.ReadInt("Enter product ID to delete: ", 1);
            var prod = _productService.GetProductById(id);

            if (prod == null) { throw new BusinessException("Error: Product ID not found."); }

            if (InputHelper.Confirm($"Are you sure you want to delete {prod.Name}?"))
            {
                _productService.DeleteProduct(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product deleted successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        private void PrintProductsTable(IEnumerable<Product> products)
        {
            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Category".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|----------------|----------|----------");

            foreach (var prod in products)
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
    }
}