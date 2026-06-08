
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class OrderService : IOrderService
    {
        private List<Order> ordersList = new();
        private int idCounter = 1;

        public void AddOrder(Order order)
        {
            order.Id = idCounter++;
            order.OrderDate = DateTime.Now;
            ordersList.Add(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return ordersList;
        }

        public Order GetOrderById(int id)
        {
            return ordersList.FirstOrDefault(o => o.Id == id);
        }
    }
}