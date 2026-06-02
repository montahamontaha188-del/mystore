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

        static void Main(string[] args)
        {
            Productclass product1 = new Productclass();

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
                    


                }
              
            }
        }

    }
}