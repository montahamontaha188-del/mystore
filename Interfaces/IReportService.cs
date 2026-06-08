using System;
using System.Collections.Generic;

namespace MyStore
{
    public interface IReportService
    {
         IEnumerable<dynamic> GetBestSellingProducts(IEnumerable<Order> orders);
         IEnumerable<dynamic> GetTopCustomers(IEnumerable<Order> orders);
         (int TotalOrders, double TotalRevenue) GetDailySalesSummary(IEnumerable<Order> orders, DateTime date);
         IEnumerable<Product> GetLowStockProducts(IEnumerable<Product> products, int threshold);
    }
}