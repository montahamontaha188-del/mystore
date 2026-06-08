using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Subtotal => Product.Price * Quantity;
    }

    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public string? AppliedDiscountCode { get; set; }
        public double DiscountPercentage { get; set; }
        public double Subtotal1 => Items.Sum(i => i.Subtotal);
        public double Total => Subtotal1 - (Subtotal1 * (DiscountPercentage / 100));
    }
}