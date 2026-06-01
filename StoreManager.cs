using MyStoreSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyStoreSystem
{

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
}
