
using Microsoft.Extensions.DependencyInjection;
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

            string _connectionString = "Server=HP0005WIN11A-E;Database=StoreDb;Trusted_Connection=True;TrustServerCertificate=True;";


            var services = new ServiceCollection();

            // Services
            // DI Types: AddScoped AddTransient  AddSingleton
            services.AddScoped<IProductService, SqlProductService>(sp => new SqlProductService(_connectionString));
            services.AddScoped<ICustomerService, SqlCustomerService>(sp => new SqlCustomerService(_connectionString));
            services.AddScoped<IDiscountService, SqlDiscountService>(sp => new SqlDiscountService(_connectionString));
            services.AddScoped<IOrderService, SqlOrderService>(sp => new SqlOrderService(_connectionString));
            services.AddScoped<IReportService, SqlReportService>(sp => new SqlReportService(_connectionString));

            // UI Menus
            services.AddScoped<ProductMenu>();
            services.AddScoped<CustomerMenu>();
            services.AddScoped<DiscountMenu>();
            services.AddScoped<OrderMenu>();
            services.AddScoped<ReportMenu>();

            var provider = services.BuildServiceProvider();

            var productMenu = provider.GetRequiredService<ProductMenu>();
            var customerMenu = provider.GetRequiredService<CustomerMenu>();
            var discountMenu = provider.GetRequiredService<DiscountMenu>();
            var orderMenu = provider.GetRequiredService<OrderMenu>();
            var reportMenu = provider.GetRequiredService<ReportMenu>();






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