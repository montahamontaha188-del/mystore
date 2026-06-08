
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
          
            IProductService productService = new ProductService();
            ProductMenu productMenu = new ProductMenu(productService);
 
            ICustomerService customerService = new CustomerService();
            CustomerMenu customerMenu = new CustomerMenu(customerService);

     
            IDiscountService discountService = new DiscountService();
            DiscountMenu discountMenu = new DiscountMenu(discountService);
 
            IOrderService orderService = new OrderService();
            OrderMenu orderMenu = new OrderMenu(orderService, productService, customerService, discountService);

           
            IReportService reportService = new ReportService();
            ReportMenu reportMenu = new ReportMenu(reportService, productService, orderService);

            while (true)
            {
                try
                {
                    displaymenue();
                    int choice1 = InputHelper.ReadInt("Select an option:", 0, 5);

                    switch (choice1)
                    {
                        case 1:
                            productMenu.DisplayProductMenu();
                            break;

                        case 2:
                            customerMenu.DisplayCustomerMenu();
                            break;

                        case 3:
                            orderMenu.DisplayOrderMenu();
                            break;

                        case 4:
                            discountMenu.DisplayDiscountMenu();
                            break;

                        case 5:
                           
                            reportMenu.DisplayReportMenu();
                            break;

                        case 0:
                            Console.WriteLine("Exiting the system.. Goodbye!");
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (BusinessException ex)
                {
                    InputHelper.WriteLineWithColor($"warning: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    InputHelper.WriteLineWithColor($"error: {ex.Message}", ConsoleColor.Red);
                }
            }
        }
    }
}