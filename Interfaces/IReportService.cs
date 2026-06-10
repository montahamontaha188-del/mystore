
using System.Collections.Generic;

namespace MyStore
{
    public interface IReportService
    {
        IEnumerable<object> GetBestSellingProducts(IEnumerable<Order> orders);
        IEnumerable<object> GetTopCustomers(IEnumerable<Order> orders);
    }
}