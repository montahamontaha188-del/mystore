using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ReportService : IReportService
    {
        public IEnumerable<dynamic> GetBestSellingProducts(IEnumerable<Order> orders)
        {
            return orders
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
        }

        public IEnumerable<dynamic> GetTopCustomers(IEnumerable<Order> orders)
        {
            return orders
                .GroupBy(o => o.Customer.Id)
                .Select(group => new
                {
                    CustomerName = group.First().Customer.Name,
                    TotalSpent = group.Sum(o => o.Total)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();
        }

        public (int TotalOrders, double TotalRevenue) GetDailySalesSummary(IEnumerable<Order> orders, DateTime date)
        {
            var dailyOrders = orders.Where(o => o.OrderDate.Date == date.Date).ToList();
            int totalOrders = dailyOrders.Count;
            double totalRevenue = dailyOrders.Sum(o => o.Total);

            return (totalOrders, totalRevenue);
        }

        public IEnumerable<Product> GetLowStockProducts(IEnumerable<Product> products, int threshold)
        {
            return products
                .Where(p => p.Quantity <= threshold)
                .OrderBy(p => p.Quantity)
                .ToList();
        }
    }
}