using System.Collections.Generic;

namespace MyStore
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
    }
}