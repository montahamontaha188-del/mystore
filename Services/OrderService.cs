 
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class OrderService : IOrderService
    {
        private List<Order> ordersList;
        private int idCounter = 1;

        // الدالة البنائية تقرأ الطلبات من الـ JSON فور تشغيل الخدمة
        public OrderService()
        {
            ordersList = DataStore.LoadOrders() ?? new List<Order>();
            UpdateIdCounter();
        }

        public void CreateOrder(Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
            {
                throw new BusinessException("Order cancelled because no items were added.");
            }

            // إسناد الـ ID والوقت الحالي في طبقة الـ Service الحركية
            order.Id = idCounter++;
            order.OrderDate = DateTime.Now;

            // خصم الكميات من المخزن حركياً عند تأكيد الطلب
            foreach (var item in order.Items)
            {
                item.Product.Quantity -= item.Quantity;
            }

            ordersList.Add(order);

            // حفظ الطلبات الجديدة وتحديث قائمة المنتجات في الـ JSON بعد خصم الكميات
            DataStore.SaveOrders(ordersList);

            // نمرر القائمة الحالية للمنتجات لحفظ تعديل الكميات
            var products = order.Items.Select(i => i.Product).ToList();
            // ملاحظة: الـ DataStore سيتكفل بتحديث حالة المخزن في ملف الـ JSON
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return ordersList;
        }

        public Order GetOrderById(int id)
        {
            return ordersList.FirstOrDefault(o => o.Id == id);
        }

        private void UpdateIdCounter()
        {
            if (ordersList != null && ordersList.Count > 0)
            {
                idCounter = ordersList.Max(o => o.Id) + 1;
            }
            else
            {
                idCounter = 1;
            }
        }
    }
}