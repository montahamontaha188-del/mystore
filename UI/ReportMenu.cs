using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ReportMenu
    {
        private readonly IReportService _reportService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        // حقن الخدمات عبر الـ Constructor Injection
        public ReportMenu(IReportService reportService, IProductService productService, IOrderService orderService)
        {
            _reportService = reportService;
            _productService = productService;
            _orderService = orderService;
        }

        public void DisplayReportMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("        REPORTS & STATISTICS MENU         ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Best-Selling Products (Top 5)");
                Console.WriteLine("2. Top Customers by Spend (Top 5)");
                Console.WriteLine("3. Daily Sales Summary");
                Console.WriteLine("4. Low Stock Alert");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("==========================================");

                int choice = InputHelper.ReadInt("Select an option: ", 0, 4);

                switch (choice)
                {
                    case 1: ShowBestSellingProducts(); break;
                    case 2: ShowTopCustomers(); break;
                    case 3: ShowDailySalesSummary(); break;
                    case 4: ShowLowStockAlert(); break;
                    case 0: Console.WriteLine("Returning to main menu..."); return;
                }
            }
        }

        private void ShowBestSellingProducts()
        {
            Console.WriteLine("\n--- TOP 5 BEST-SELLING PRODUCTS ---");
            var orders = _orderService.GetAllOrders();

            var topProducts = _reportService.GetBestSellingProducts(orders);

            if (!topProducts.Any())
            {
                throw new BusinessException("No sales data available yet.");
            }

            int rank = 1;
            foreach (dynamic prod in topProducts)
            {
                Console.WriteLine($"{rank}. {((string)prod.ProductName).PadRight(20)} | Total Units Sold: {prod.TotalQuantity}");
                rank++;
            }
        }

        private void ShowTopCustomers()
        {
            Console.WriteLine("\n--- TOP 5 CUSTOMERS BY SPEND ---");
            var orders = _orderService.GetAllOrders();

            var topCustomers = _reportService.GetTopCustomers(orders);

            if (!topCustomers.Any())
            {
                throw new BusinessException("No orders data available yet.");
            }

            int rank = 1;
            foreach (dynamic cust in topCustomers)
            {
                Console.WriteLine($"{rank}. {((string)cust.CustomerName).PadRight(20)} | Total Spent: {cust.TotalSpent:0.00}");
                rank++;
            }
        }

        private void ShowDailySalesSummary()
        {
            Console.WriteLine("\n--- DAILY SALES SUMMARY ---");
            DateTime selectedDate = InputHelper.ReadDate("Enter date (dd-MM-yyyy): ");

            var orders = _orderService.GetAllOrders();
            var dailyOrders = orders.Where(o => o.OrderDate.Date == selectedDate.Date).ToList();

            int totalOrders = dailyOrders.Count;
            double totalRevenue = dailyOrders.Sum(o => o.Total);

            Console.WriteLine($"\nSummary for {selectedDate.ToString("dd-MM-yyyy")}:");
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"Total Orders  : {totalOrders}");
            Console.WriteLine($"Total Revenue : {totalRevenue:0.00}");
            Console.WriteLine("------------------------------------------");
        }

        private void ShowLowStockAlert()
        {
            Console.WriteLine("\n--- LOW STOCK ALERT ---");
            int threshold = InputHelper.ReadInt("Enter low stock threshold quantity: ", 0);

            var products = _productService.GetAllProducts();
            var lowStockProducts = products
                .Where(p => p.Quantity <= threshold)
                .OrderBy(p => p.Quantity)
                .ToList();

            if (!lowStockProducts.Any())
            {
                Console.WriteLine($"Great! No products are at or below the threshold of {threshold}.");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Product Name".PadRight(20) + "| " + "Current Stock");
            Console.WriteLine("-----|---------------------|--------------");
            foreach (var p in lowStockProducts)
            {
                Console.WriteLine($"{p.Id.ToString().PadRight(5)}| {p.Name.PadRight(20)}| {p.Quantity}");
            }
        }
    }
}