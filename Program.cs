using System;

namespace MyStore
{
    class Program
    {
         static void DisplayMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==========================================");
            Console.WriteLine("          MY STORE SYSTEM (JSON)          ");
            Console.WriteLine("==========================================");
            Console.ResetColor();
            Console.WriteLine("1. Product Management");
            Console.WriteLine("2. Customers Management");
            Console.WriteLine("3. Orders Management");
            Console.WriteLine("4. Discounts Management");
            Console.WriteLine("5. Reports & Statistics");
            Console.WriteLine("0. Exit");
            Console.WriteLine("==========================================");
        }

        static void Main(string[] args)
        {
             IProductService productService = new ProductService();
            ICustomerService customerService = new CustomerService();
            IDiscountService discountService = new DiscountService();
            IOrderService orderService = new OrderService();
            IReportService reportService = new ReportService();

             ProductMenu productMenu = new ProductMenu(productService);
            CustomerMenu customerMenu = new CustomerMenu(customerService);
            DiscountMenu discountMenu = new DiscountMenu(discountService);
            OrderMenu orderMenu = new OrderMenu(orderService, productService, customerService, discountService);
            ReportMenu reportMenu = new ReportMenu(reportService, productService, orderService);

             InputHelper.WriteLineWithColor("Inventory data loaded successfully from JSON storage.", ConsoleColor.Green);

             while (true)
            {
                try
                {
                    DisplayMainMenu();
                    int choice = InputHelper.ReadInt("Select an option: ", 0, 5);

                    switch (choice)
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
                             Console.WriteLine("All data is securely saved in JSON files. Exiting program... Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (BusinessException ex)
                {
                     InputHelper.WriteLineWithColor($"\nWarning: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                     InputHelper.WriteLineWithColor($"\nSystem Error: {ex.Message}", ConsoleColor.Red);
                }
            }
        }
    }
}