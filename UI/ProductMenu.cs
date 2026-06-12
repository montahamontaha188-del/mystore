using System;
using System.Linq;

namespace MyStore
{
    public class ProductMenu
    {
        private readonly IProductService _productService;
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
                Console.WriteLine("4. ManageMenu");
                Console.WriteLine("0. Back to Main Menu");
       

                int choice = InputHelper.ReadInt("Select an option:", 0, 4);

                switch (choice)
                {
                    case 1: AddProductUI(); break;
                    case 2: ListProductsUI(); break;
                    case 3: ShowTotalValueUI(); break;
                    case 4: ManageMenu(); break;
                    case 0: return;
                }
            }
        }

        private void AddProductUI()
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
            /*var list1 = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            foreach (var category in list1) {
                Console.WriteLine(category);}
            int count =list1.Count*/

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
            Console.WriteLine($"Product '{name}' added successfully under category '{selectedCategory}'.");
            Console.ResetColor();
        }
         private void ListProductsUI()
        {
          
            var products = _productService.GetAllProducts().ToList();
            if (products.Count == 0)
            {
                throw new BusinessException("No products found.");
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(15) + "| " + "Category".PadRight(15) + "| " + "Price".PadRight(10) + "| " + "Quantity");
            Console.WriteLine("-----|----------------|----------------|----------|----------");

            foreach (var prod in products)
            {
                Console.WriteLine($"{prod.Id.ToString().PadRight(5)}| {prod.Name.PadRight(15)}| {prod.Category.ToString().PadRight(15)}| {prod.Price.ToString("0.00").PadRight(10)}| {prod.Quantity}");
            }
        }

        private void ShowTotalValueUI()
        {
            var breakdown = _productService.GetValueBreakdownByCategory();
            if (breakdown.Count == 0)
            {
                throw new BusinessException("Inventory is empty. Total Value: 0.00");
            }

            Console.WriteLine("\n--- Inventory Value Breakdown by Category ---");
            foreach (var item in breakdown)
            {
                Console.WriteLine($"{item.Key.ToString().PadRight(15)} : {item.Value:0.00}");
            }

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"Total Store Value: {_productService.GetGrandTotalValue():0.00}");
        }
        private void ManageMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- PRODUCT MANAGEMENT ---");
                Console.WriteLine("1. Search product by name");
                Console.WriteLine("2. Update price");
                Console.WriteLine("3. Update quantity");
                Console.WriteLine("4. Delete product");
                Console.WriteLine("0. Back");
                ListProductsUI();
                int choice = InputHelper.ReadInt("Select: ", 0, 4);
                
                switch (choice)
                {
                    case 1: SearchProductByName(); break;
                    case 2: UpdateProductPrice(); break;
                    case 3: UpdateProductQuantity(); break;
                    case 4: DeleteProductById(); break;
               
                    case 0: return;
                }
            }
        }
     
        private void SearchProductByName()
        {
            string searchName = InputHelper.ReadNonEmptyString("Enter product name: ");
            var results = _productService.SearchProductsByName(searchName).ToList();

            if (results.Count == 0)
                throw new BusinessException("No products found.");

            foreach (var prod in results)
                Console.WriteLine($"ID: {prod.Id} | Name: {prod.Name} | Price: {prod.Price}");
        }
       

        private void UpdateProductPrice()
        {
            int id = InputHelper.ReadInt("Enter ID to update price: ", 1, int.MaxValue);
            double newPrice = InputHelper.ReadDouble("Enter new price: ", 0.01);
            _productService.UpdateProductPrice(id, newPrice);
            Console.WriteLine("Updated successfully!");
        }

        private void UpdateProductQuantity()
        {
            int id = InputHelper.ReadInt("Enter ID to update quantity: ", 1, int.MaxValue);
            int newQty = InputHelper.ReadInt("Enter new quantity: ", 0);
            _productService.UpdateProductQuantity(id, newQty);
            Console.WriteLine("Updated successfully!");
        }

        private void DeleteProductById()
        {
            int id = InputHelper.ReadInt("Enter ID to delete: ", 1, int.MaxValue);
            _productService.DeleteProduct(id);
            Console.WriteLine("Deleted successfully!");
        }


        
    }
}