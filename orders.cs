using MyStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace mystore
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
        public double Total => Items.Sum(i => i.Subtotal);
    }
    public class OrderClass
    {
        private List<Order> ordersList = new List<Order>();
        private int idCounter = 1;

        public void DisplayOrderMenu(Productclass productManager, CustomerClass customerManager)
        {
            bool inOrderMenu = true;
            while (inOrderMenu)
            {
                Console.WriteLine("\n--- ORDER MENU ---");
                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. List All Orders");
                Console.WriteLine("3. View Order Details");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateOrder(productManager, customerManager);
                        break;
                    case 2:
                        ListAllOrders();
                        break;
                    case 3:
                        ViewOrderDetails();
                        break;
                    case 0:
                        Console.WriteLine("Returning to main menu...");
                        inOrderMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private void CreateOrder(Productclass productManager, CustomerClass customerManager)
        {

            Console.Write("Enter Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            Customer customer = customerManager.GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine("Error: Customer not found!");
                return;
            }

            Order newOrder = new Order
            {
                Id = idCounter,
                Customer = customer,
                OrderDate = DateTime.Now
            };


            while (true)
            {
                Console.Write("Enter Product ID to add (or 0 to finish adding): ");
                int productId = Convert.ToInt32(Console.ReadLine());

                if (productId == 0) break;

                Product product = productManager.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine("Error: Product not found!");
                    continue;
                }

                Console.Write($"Enter quantity for '{product.Name}' (Available: {product.Quantity}): ");
                int quantity = Convert.ToInt32(Console.ReadLine());

                if (quantity > product.Quantity)
                {
                    Console.WriteLine($"Error: Not enough stock! Available only: {product.Quantity}. Please try again.");
                    continue;
                }
                if (quantity <= 0)
                {
                    Console.WriteLine("Error: Quantity must be greater than 0.");
                    continue;
                }

 
                OrderItem item = new OrderItem
                {
                    Product = product,
                    Quantity = quantity
                };
                newOrder.Items.Add(item);
                Console.WriteLine($"Added {quantity} x '{product.Name}' to order.");
            }

 
            if (newOrder.Items.Count == 0)
            {
                Console.WriteLine("Order cancelled because no items were added.");
                return;
            }

            Console.Write($"\nTotal amount is: {newOrder.Total:0.00}. Confirm order? (y/n): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm == "y" || confirm == "yes")
            {
 
                foreach (var item in newOrder.Items)
                {
                    item.Product.Quantity -= item.Quantity;
                }

                ordersList.Add(newOrder);
                idCounter++;
                Console.WriteLine($"Order ID: {newOrder.Id} created successfully with today's date.");
            }
            else
            {
                Console.WriteLine("Order cancelled.");
            }
        }

        private void ListAllOrders()
        {
            if (ordersList.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Customer".PadRight(20) + "| " + "Date".PadRight(12) + "| " + "Total");
            Console.WriteLine("-----|---------------------|------------|----------");

            foreach (var order in ordersList)
            {
                Console.WriteLine(
                    order.Id.ToString().PadRight(5) + "| " +
                    order.Customer.Name.PadRight(20) + "| " +
                    order.OrderDate.ToString("yyyy-MM-dd").PadRight(12) + "| " +
                    order.Total.ToString("0.00")
                );
            }
        }

        private void ViewOrderDetails()
        {
            Console.Write("Enter Order ID to view details: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Order order = ordersList.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                Console.WriteLine("Error: Order ID not found.");
                return;
            }

            // طباعة الفاتورة الكاملة (Full Receipt)
            Console.WriteLine("\n==========================================");
            Console.WriteLine($"                RECEIPT                  ");
            Console.WriteLine("==========================================");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Date:     {order.OrderDate}");
            Console.WriteLine($"Customer: {order.Customer.Name} (Phone: {order.Customer.PhoneNumber})");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Items:");

            foreach (var item in order.Items)
            {
                Console.WriteLine($"- {item.Product.Name} x {item.Quantity}");
                Console.WriteLine($"  Price: {item.Product.Price:0.00} | Subtotal: {item.Subtotal:0.00}");
            }

            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"GRAND TOTAL: {order.Total:0.00}");
            Console.WriteLine("==========================================");
        }
    }
}
    


