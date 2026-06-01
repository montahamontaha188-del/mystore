using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStoreSystem
{
    class Program
    {
        static void displaymenue()
        {

            Console.WriteLine("\n---  MY STORE SYSTEM ---");
            Console.WriteLine("1. Product");
            Console.WriteLine("2. Customers");
            Console.WriteLine("3. Orders");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");


        }
        static void displayproductmenue()
        {
            Console.WriteLine("\n--- PRODUCT MENU ---");
            Console.WriteLine("1. Add product");
            Console.WriteLine("2. List products");
            Console.WriteLine("3. Total value");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Select an option: ");

        }

        static void Main(string[] args)
        {
            StoreManager store = new StoreManager();

            while (true)
            {
                displaymenue();
                int choice = Convert.ToInt32(Console.ReadLine());
                bool inProductMenu = true;
                while (inProductMenu)
                {
                    switch (choice)
                    {
                        case 1:
                            {
                                displayproductmenue();
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

                                    Console.WriteLine("Returning to main menu...");
                                    inProductMenu = false;

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
        }

    }
}