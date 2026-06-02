using mystore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
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

        static void Main(string[] args)
        {
            Productclass product1 = new Productclass();
            CustomerClass customer1 = new CustomerClass();

            while (true)
            {
                displaymenue();
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        {
                            product1.displayproductmenue();
                            break;
                        }
                    case 2:
                        {
                            customer1.DisplayCustomerMenu();
                            break;
                        }
                    case 0:
                        Console.WriteLine("Exiting program... Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
              
            }
        }

    }
}