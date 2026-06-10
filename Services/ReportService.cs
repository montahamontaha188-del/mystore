 
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ReportService : IReportService
    {
        public IEnumerable<object> GetBestSellingProducts(IEnumerable<Order> orders)
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

        public IEnumerable<object> GetTopCustomers(IEnumerable<Order> orders)
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
    }
}