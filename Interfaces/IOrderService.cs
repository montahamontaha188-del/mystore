
using System.Collections.Generic;

namespace MyStore
{
    public interface IOrderService
    {
        void AddOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
    }
}