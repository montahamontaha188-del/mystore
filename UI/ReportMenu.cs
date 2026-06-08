
using System;
using System.Linq;

namespace MyStore
{
    public class ReportMenu
    {
        private readonly IReportService _reportService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

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
                    case 1: ShowBestSellingProductsUI(); break;
                    case 2: ShowTopCustomersUI(); break;
                    case 3: ShowDailySalesSummaryUI(); break;
                    case 4: ShowLowStockAlertUI(); break;
                    case 0: return;
                }
            }
        }

        private void ShowBestSellingProductsUI()
        {
            Console.WriteLine("\n--- TOP 5 BEST-SELLING PRODUCTS ---");
            var orders = _orderService.GetAllOrders();
            var topProducts = _reportService.GetBestSellingProducts(orders).ToList();

            if (!topProducts.Any())
            {
                throw new BusinessException("No sales data available yet.");
            }

            int rank = 1;
            foreach (var prod in topProducts)
            {
                Console.WriteLine($"{rank}. {((string)prod.ProductName).PadRight(20)} | Total Units Sold: {prod.TotalQuantity}");
                rank++;
            }
        }

        private void ShowTopCustomersUI()
        {
            Console.WriteLine("\n--- TOP 5 CUSTOMERS BY SPEND ---");
            var orders = _orderService.GetAllOrders();
            var topCustomers = _reportService.GetTopCustomers(orders).ToList();

            if (!topCustomers.Any())
            {
                throw new BusinessException("No orders data available yet.");
            }

            int rank = 1;
            foreach (var cust in topCustomers)
            {
                Console.WriteLine($"{rank}. {((string)cust.CustomerName).PadRight(20)} | Total Spent: {cust.TotalSpent:0.00} LYD");
                rank++;
            }
        }

        private void ShowDailySalesSummaryUI()
        {
            Console.WriteLine("\n--- DAILY SALES SUMMARY ---");
            DateTime selectedDate = InputHelper.ReadDate("Enter date (dd-MM-yyyy): ");

            var orders = _orderService.GetAllOrders();
            var summary = _reportService.GetDailySalesSummary(orders, selectedDate);

            Console.WriteLine($"\nSummary for {selectedDate.ToString("dd-MM-yyyy")}:");
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"Total Orders  : {summary.TotalOrders}");
            Console.WriteLine($"Total Revenue : {summary.TotalRevenue:0.00} LYD");
            Console.WriteLine("------------------------------------------");
        }

        private void ShowLowStockAlertUI()
        {
            Console.WriteLine("\n--- LOW STOCK ALERT ---");
            int threshold = InputHelper.ReadInt("Enter low stock threshold quantity: ", 0);

            var products = _productService.GetAllProducts();
            var lowStockProducts = _reportService.GetLowStockProducts(products, threshold).ToList();

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