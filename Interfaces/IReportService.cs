using System;
using System.Collections.Generic;

namespace MyStore
{
    public interface IReportService
    {
        // دالة ترجع قائمة بأكثر 5 منتجات مبيعاً
        IEnumerable<dynamic> GetBestSellingProducts(IEnumerable<Order> orders);

        // دالة ترجع قائمة بأكثر 5 زبائن شراءً
        IEnumerable<dynamic> GetTopCustomers(IEnumerable<Order> orders);

        // دالة لحساب إجمالي الفواتير والإيرادات ليوم معين
        (int TotalOrders, double TotalRevenue) GetDailySalesSummary(IEnumerable<Order> orders, DateTime date);

        // دالة لجلب المنتجات اللّي كميتها أقل من الحد المطلوب
        IEnumerable<Product> GetLowStockProducts(IEnumerable<Product> products, int threshold);
    }
}