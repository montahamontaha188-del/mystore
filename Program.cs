

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
            Console.WriteLine("4. Discounts");
            Console.WriteLine("5. Reports & Statistics");
            Console.WriteLine("0. Exit");
            
        }

        static void Main(string[] args)
        {
            Productservices product1 = new Productservices();
            CustomerClass customer1 = new CustomerClass();
            Orderservices order1 = new Orderservices();
            DiscountServices discount1 = new DiscountServices();
            ReportServices report1 = new ReportServices();
        

           

            while (true)
            {
                try {
                    displaymenue();
                    int choice1 = InputHelper.ReadInt("Select an option:", 0, 5); ;
                    switch (choice1)
                    {
                        case 1: product1.displayproductmenue(); break;
                        case 2: customer1.DisplayCustomerMenu(); break;
                        case 3: order1.DisplayOrderMenu(product1, customer1, discount1); break;
                        case 4: discount1.DisplayDiscountMenu(); break;
                        case 5: report1.DisplayReportMenu(product1, order1); break;
                        case 0:
                            Console.WriteLine("return to the main menu..");
                           
                            return;
                        default: Console.WriteLine("Invalid option. Please try again."); break;
                    }

                }
                catch (BusinessException ex)
                {

                    InputHelper.WriteLineWithColor($"warning: {ex.Message}", ConsoleColor.Yellow);
                }
           
                catch (Exception ex)
                {

                    InputHelper. WriteLineWithColor($"error: {ex.Message}", ConsoleColor.Red);
                }
            }    
      
        }

    }
}