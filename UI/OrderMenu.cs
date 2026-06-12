
using System;
using System.Linq;

namespace MyStore
{
    public class OrderMenu
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IDiscountService _discountService;

 
        public OrderMenu(
            IOrderService orderService,
            IProductService productService,
            ICustomerService customerService,
            IDiscountService discountService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
            _discountService = discountService;
        }
    
        public void DisplayOrderMenu()
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
                    case 1: CreateOrderUI(); break;
                    case 2: ListAllOrdersUI(); break;
                    case 3: ViewOrderDetailsUI(); break;
                    case 0:
                        Console.WriteLine("Returning to main menu...");
                        return;
                }
            }
        }

        private void CreateOrderUI()
        {
            int customerId = InputHelper.ReadInt("Enter Customer ID: ");
            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
            {
                throw new BusinessException("Error: Customer not found!");
            }

            Order newOrder = new Order { Customer = customer };

            while (true)
            {
                try
                {
                    int productId = InputHelper.ReadInt("Enter Product ID to add (or 0 to finish adding): ");
                    if (productId == 0) break;

                    var product = _productService.GetProductById(productId);
                    if (product == null)
                    {
                        throw new BusinessException("Error: Product not found!");
                    }

                    int quantity = InputHelper.ReadInt($"Enter quantity for '{product.Name}' (Available: {product.Quantity}): ", 1, product.Quantity);

                    OrderItem item = new OrderItem
                    {
                        Product = product,
                        Quantity = quantity
                    };
                    newOrder.Items.Add(item);
                    Console.WriteLine($"Added {quantity} x '{product.Name}' to order.");
                }
                catch (BusinessException ex)
                {
                    InputHelper.WriteLineWithColor($"warning: {ex.Message}", ConsoleColor.Yellow);
                }
            }

            if (newOrder.Items.Count == 0)
            {
                throw new BusinessException("Order cancelled because no items were added.");
            }

            Console.Write("Do you have a discount code? (leave blank to skip): ");
            string codeInput = Console.ReadLine().Trim().ToUpper();
            if (!string.IsNullOrEmpty(codeInput))
            {
                var discount = _discountService.GetActiveDiscount(codeInput);
                if (discount != null)
                {
                    newOrder.AppliedDiscountCode = discount.Code;
                    newOrder.DiscountPercentage = discount.Percentage;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Success: Discount code '{discount.Code}' (-{discount.Percentage}%) applied!");
                    Console.ResetColor();
                }
                else
                {
                    throw new BusinessException("Warning: Invalid or inactive discount code. Proceeding without discount.");
                }
            }

            if (InputHelper.Confirm($"\nTotal amount  is: {newOrder.Total:0.00}. Confirm order?"))
            {
    
                foreach (var item in newOrder.Items)
                {
                    _productService.UpdateProductQuantity(item.Product.Id, item.Product.Quantity - item.Quantity);
                }

                _orderService.AddOrder(newOrder);
                Console.WriteLine($"Order ID: {newOrder.Id} created successfully with today's date.");
            }
        }

        private void ListAllOrdersUI()
        {
            var orders = _orderService.GetAllOrders().ToList();
            if (orders.Count == 0)
            {
                throw new BusinessException("No orders found.");
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Customer".PadRight(20) + "| " + "Date".PadRight(12) + "| " + "Total");
            Console.WriteLine("-----|---------------------|------------|----------");

            foreach (var order in orders)
            {
                Console.WriteLine($"{order.Id.ToString().PadRight(5)}| {order.Customer.Name.PadRight(20)}| {order.OrderDate.ToString("yyyy-MM-dd").PadRight(12)}| {order.Total.ToString("0.00")}");
            }
        }

        private void ViewOrderDetailsUI()
        {
            int id = InputHelper.ReadInt("Enter Order ID to view details: ");
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                throw new BusinessException("Error: Order ID not found.");
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