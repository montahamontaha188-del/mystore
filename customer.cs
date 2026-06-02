using MyStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace mystore
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class CustomerClass
    {
        private List<Customer> customersList = new();
        private int idCounter = 1;

        public void DisplayCustomerMenu()
        {
            bool inCustomerMenu = true;
            while (inCustomerMenu)
            {
                Console.WriteLine("\n--- CUSTOMER MENU ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. List All Customers");
                Console.WriteLine("3. Find Customer by Name");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                int choice2 = Convert.ToInt32(Console.ReadLine());

                switch (choice2)
                {
                    case 1:
                        AddCustomer();
                        break;
                    case 2:
                        ListCustomers();
                        break;
                    case 3:
                        FindCustomerByName();
                        break;
                    case 0:
                        Console.WriteLine("Returning to main menu...");
                        inCustomerMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private void AddCustomer()
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Error: Name cannot be empty.");
                return;
            }

            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(phone) || !phone.All(char.IsDigit))
            {
                Console.WriteLine("Error: Phone number must contain only digits.");
                return;
            }

       
            bool isDuplicate = customersList.Any(c => c.PhoneNumber == phone);
            if (isDuplicate)
            {
                Console.WriteLine("Error: A customer with this phone number already exists.");
                return;
            }

    
            Customer newCustomer = new Customer
            {
                Id = idCounter++,
                Name = name,
                PhoneNumber = phone
            };

            customersList.Add(newCustomer);
            Console.WriteLine($"Customer '{name}' added successfully with ID: {newCustomer.Id}");
        }
        private void ListCustomers()
        {
  
            if (customersList.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(20) + "| " + "Phone Number");
            Console.WriteLine("-----|---------------------|----------------");

            foreach (var cust in customersList)
            {
                Console.WriteLine(
                    cust.Id.ToString().PadRight(5) + "| " +
                    cust.Name.PadRight(20) + "| " +
                    cust.PhoneNumber
                );
            }
        }
        private void FindCustomerByName()
        {
            Console.Write("Enter name to search: ");
            string searchName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                Console.WriteLine("Error: Search query cannot be empty.");
                return;
            }


            var results = customersList.Where(c => c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No customers found matching that name.");
                return;
            }

            Console.WriteLine("\n--- Search Results ---");
            foreach (var cust in results)
            {
                Console.WriteLine($"ID: {cust.Id} | Name: {cust.Name} | Phone: {cust.PhoneNumber}");
            }
        }
        public Customer GetCustomerById(int id)
        {
            return customersList.FirstOrDefault(c => c.Id == id);
        }


    }
}
