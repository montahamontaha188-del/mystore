using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyStore
{
    public class ReportServices
    {
     
        public void DisplayReportMenu(Productservices productManager, Orderservices orderManager)
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
                    case 1:
                        ShowBestSellingProducts(orderManager.GetOrdersList());
                        break;
                    case 2:
                        ShowTopCustomers(orderManager.GetOrdersList());
                        break;
                    case 3:
                        ShowDailySalesSummary(orderManager.GetOrdersList());
                        break;
                    case 4:
                        ShowLowStockAlert(productManager.GetProductsList());
                        break;
                    case 0:
                        return; 
                }
            }
        }

     
        private void ShowBestSellingProducts(List<Order> orders)
        {
            Console.WriteLine("\n--- TOP 5 BEST-SELLING PRODUCTS ---");

            var topProducts = orders
                .SelectMany(o => o.Items)
                .GroupBy(item => item.Product.Id)
                .Select(group => new
                {
                    ProductName = group.First().Product.Name,
                    TotalQuantity = group.Sum(item => item.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(5)
                .ToList();

            if (!topProducts.Any())
            {
                throw new BusinessException("No sales data available yet.");
                
            }

            int rank = 1;
            foreach (var prod in topProducts)
            {
                Console.WriteLine($"{rank}. {prod.ProductName.PadRight(20)} | Total Units Sold: {prod.TotalQuantity}");
                rank++;
            }
        }

     
        private void ShowTopCustomers(List<Order> orders)
        {
            Console.WriteLine("\n--- TOP 5 CUSTOMERS BY SPEND ---");

            var topCustomers = orders
                .GroupBy(o => o.Customer.Id)
                .Select(group => new
                {
                    CustomerName = group.First().Customer.Name,
                    TotalSpent = group.Sum(o => o.Total)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();

            if (!topCustomers.Any())
            {
                throw new BusinessException("No orders data available yet.");
               
            }

            int rank = 1;
            foreach (var cust in topCustomers)
            {
                Console.WriteLine($"{rank}. {cust.CustomerName.PadRight(20)} | Total Spent: {cust.TotalSpent:0.00}");
                rank++;
            }
        }

         
        private void ShowDailySalesSummary(List<Order> orders)
        {
            Console.WriteLine("\n--- DAILY SALES SUMMARY ---");

           
            DateTime selectedDate = InputHelper.ReadDate("Enter date (dd-MM-yyyy): ");

          
            var dailyOrders = orders.Where(o => o.OrderDate.Date == selectedDate.Date).ToList();

            
    
            int totalOrders = dailyOrders.Count;
            double totalRevenue = dailyOrders.Sum(o => o.Total);

            Console.WriteLine($"\nSummary for {selectedDate.ToString("dd-MM-yyyy")}:");
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"Total Orders  : {totalOrders}");
            Console.WriteLine($"Total Revenue : {totalRevenue:0.00}");
            Console.WriteLine("------------------------------------------");
        }

       
        private void ShowLowStockAlert(List<Product> products)
        {
            Console.WriteLine("\n--- LOW STOCK ALERT ---");
            int threshold = InputHelper.ReadInt("Enter low stock threshold quantity: ", 0);

           
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