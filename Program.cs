
using MyStore.MyStore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    class Program
    {
        static void displaymenue()
        {
            
 
            Console.WriteLine("\n---  MY STORE SYSTEM ---");
            Console.WriteLine("1. Product");
            Console.WriteLine("2. Customers");
            Console.WriteLine("3. Orders");
            Console.WriteLine("4. Discounts");
            Console.WriteLine("5. Reports & Statistics");
            Console.WriteLine("0. Exit");
            
        }

        static void Main(string[] args)
        {
            Productservices product1 = new Productservices();
            CustomerClass customer1 = new CustomerClass();
            Orderservices order1 = new Orderservices();
            DiscountServices discount1 = new DiscountServices();
            ReportServices report1 = new ReportServices();
            DataStore store = new DataStore();

            try
            {
                List<Product> savedProducts = store.LoadProducts();
                product1.SetProductsList(savedProducts);
                product1.UpdateIdCounter(); 

                List<Customer> savedCustomers = store.LoadCustomers();
                customer1.SetCustomersList(savedCustomers);
                customer1.UpdateIdCounter();
                List<Order> savedOrders = store.LoadOrders();

                order1.SetOrdersList(savedOrders);
                order1.UpdateIdCounter();


                List<Discount> savedDiscounts = store.LoadDiscounts();
                discount1.SetDiscountsList(savedDiscounts);
                discount1.UpdateIdCounter();

                if (savedProducts.Count > 0 || savedCustomers.Count > 0 || savedOrders.Count > 0 || savedDiscounts.Count > 0)
                {
                    InputHelper.WriteLineWithColor("Inventory data loaded successfully from JSON storage.", ConsoleColor.Cyan);
                }
            }
            catch (Exception ex)
            {
                InputHelper.WriteLineWithColor("Initialization Warning: " + ex.Message, ConsoleColor.Yellow);
            }

            while (true)
            {
                try {
                    displaymenue();
                    int choice1 = InputHelper.ReadInt("Select an option:", 0, 5); ;
                    switch (choice1)
                    {
                        case 1: product1.displayproductmenue(); break;
                        case 2: customer1.DisplayCustomerMenu(); break;
                        case 3: order1.DisplayOrderMenu(product1, customer1, discount1); break;
                        case 4: discount1.DisplayDiscountMenu(); break;
                        case 5: report1.DisplayReportMenu(product1, order1); break;
                        case 0:
                            Console.WriteLine("Saving store data to JSON...");
                            store.SaveProducts(product1.GetProductsList());
                            store.SaveCustomers(customer1.GetCustomersList());
                            store.SaveOrders(order1.GetOrdersList());
                            store.SaveDiscounts(discount1.GetDiscountsList());
                            Console.WriteLine("Exiting program... Goodbye!");
                            return;
                        default: Console.WriteLine("Invalid option. Please try again."); break;
                    }

                }
                catch (BusinessException ex)
                {

                    InputHelper.WriteLineWithColor($"warning: {ex.Message}", ConsoleColor.Yellow);
                }
           
                catch (Exception ex)
                {

                    InputHelper. WriteLineWithColor($"error: {ex.Message}", ConsoleColor.Red);
                }
            }    
      
        }

    }
}