
using System;
using System.Collections.Generic;
using System.Text;

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
    public class Orderservices
    {
        private List<Order> ordersList = new();
        private int idCounter = 1;

        public void DisplayOrderMenu(Productservices productManager, CustomerClass customerManager, DiscountServices discountManager)
        {

            while (true)
            {
                Console.WriteLine("\n--- ORDER MENU ---");
                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. List All Orders");
                Console.WriteLine("3. View Order Details");
                Console.WriteLine("0. Back to Main Menu");


                int choice = InputHelper.ReadInt("Select an option: ", 0, 3);

                switch (choice)
                {
                    case 1:
                        CreateOrder(productManager, customerManager, discountManager);
                        break;
                    case 2:
                        ListAllOrders();
                        break;
                    case 3:
                        ViewOrderDetails();
                        break;
                    case 0:
                        Console.WriteLine("Returning to main menu...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private void CreateOrder(Productservices productManager1, CustomerClass customerManager1, DiscountServices discountManager)
        {


            int customerId = InputHelper.ReadInt("Enter Customer ID: ");

            var customer = customerManager1.GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine("Error: Customer not found!");
                return;
            }

            Order newOrder = new Order
            {
                Id = idCounter++,
                Customer = customer,
                OrderDate = DateTime.Now
            };


            while (true)
            {

                int productId = InputHelper.ReadInt("Enter Product ID to add (or 0 to finish adding): ");

                if (productId == 0) break;

                var product = productManager1.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine("Error: Product not found!");
                    continue;
                }


                int quantity1 = InputHelper.ReadInt($"Enter quantity for '{product.Name}' (Available: {product.Quantity}): ", 1, product.Quantity);



                OrderItem item = new OrderItem
                {
                    Product = product,
                    Quantity = quantity1
                };
                newOrder.Items.Add(item);
                Console.WriteLine($"Added {quantity1} x '{product.Name}' to order.");
            }


            if (newOrder.Items.Count == 0)
            {
                Console.WriteLine("Order cancelled because no items were added.");
                return;
            }
            Console.Write("Do you have a discount code? (leave blank to skip): ");
            string codeInput = Console.ReadLine().Trim().ToUpper();
            if (!string.IsNullOrEmpty(codeInput))
            {

                var discount = discountManager.GetActiveDiscount(codeInput);

                if (discount != null)
                {
                    newOrder.AppliedDiscountCode = discount.Code;
                    newOrder.DiscountPercentage = discount.Percentage;
                    Console.WriteLine($"Success: Discount code '{discount.Code}' (-{discount.Percentage}%) applied!");
                }
                else
                {
                    Console.WriteLine("Warning: Invalid or inactive discount code. Proceeding without discount.");
                }
            }

            if (InputHelper.Confirm($"\nTotal amount after discount is: {newOrder.Total:0.00}. Confirm order?"))


            {

                foreach (var i in newOrder.Items)
                {
                    i.Product.Quantity -= i.Quantity;
                }

                ordersList.Add(newOrder);

                Console.WriteLine($"Order ID: {newOrder.Id} created successfully with today's date.");
                return;
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

            int id = InputHelper.ReadInt("Enter Order ID to view details: ");

            var order = ordersList.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                Console.WriteLine("Error: Order ID not found.");
                return;
            }


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
            Console.WriteLine($"Subtotal:     {order.Subtotal1:0.00}");
            if (!string.IsNullOrEmpty(order.AppliedDiscountCode))
            {

                double discountAmount = order.Subtotal1 * (order.DiscountPercentage / 100);


                Console.WriteLine($"Discount:     {order.AppliedDiscountCode} (-{order.DiscountPercentage}%) -> -{discountAmount:0.00}");
                Console.WriteLine("------------------------------------------");
            }

            Console.WriteLine($"GRAND TOTAL:  {order.Total:0.00}");

            Console.WriteLine("==========================================");
        }
    }



}
    


