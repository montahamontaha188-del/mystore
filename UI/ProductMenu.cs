using System;
using System.Linq;

namespace MyStore
{
    public class ProductMenu
    {
        private readonly IProductService _productService;

        // حقن الخدمة عبر الدالة البنائية لكي نتمكن من استخدام دالاتها
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
                Console.WriteLine("0. Back to Main Menu");

                int choice = InputHelper.ReadInt("Select an option:", 0, 3);

                switch (choice)
                {
                    case 1: AddProductUI(); break;
                    case 2: ListProductsUI(); break;
                    case 3: ShowTotalValueUI(); break;
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

            int categoryChoice = InputHelper.ReadInt("Select an option: ", 1, categories.Length);
            Category selectedCategory = categories[categoryChoice - 1];

            // نجهز كائن المنتج السادة
            Product newProduct = new Product
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = selectedCategory
            };

            // نرسل المنتج الجاهز للخدمة لكي تحفظه في الذاكرة وتمنحه المعرف التلقائي
            _productService.AddProduct(newProduct);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Product '{name}' added successfully under category '{selectedCategory}'.");
            Console.ResetColor();
        }

        private void ListProductsUI()
        {
            // نطلب القائمة من الخدمة ونحولها مجدداً لعرضها
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
    }
}