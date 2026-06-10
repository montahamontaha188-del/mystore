using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class CustomerMenu
    {
        private readonly ICustomerService _customerService;

        // حقن الخدمة عبر الـ Constructor
        public CustomerMenu(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void DisplayCustomerMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- CUSTOMER MENU ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. List All Customers");
                Console.WriteLine("3. Find Customer by Name");
                Console.WriteLine("0. Back to Main Menu");

                int choice = InputHelper.ReadInt("Select an option: ", 0, 3);

                switch (choice)
                {
                    case 1: AddCustomer(); break;
                    case 2: ListCustomers(); break;
                    case 3: FindCustomerByName(); break;
                    case 0: Console.WriteLine("Returning to main menu..."); return;
                    default: Console.WriteLine("Invalid choice. Please try again."); break;
                }
            }
        }

        private void AddCustomer()
        {
            string name = InputHelper.ReadNonEmptyString("Enter customer name: ");
            string phone = InputHelper.ReadPhoneNumber("Enter phone number: ");

            Customer newCustomer = new Customer
            {
                Name = name,
                PhoneNumber = phone
            };

            // استدعاء الخدمة الصامتة لتتأكد من الهاتف وتضيف للـ JSON
            _customerService.AddCustomer(newCustomer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Customer '{name}' added successfully!");
            Console.ResetColor();
        }

        private void ListCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            if (!customers.Any())
            {
                throw new BusinessException("No customers found.");
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(20) + "| " + "Phone Number");
            Console.WriteLine("-----|---------------------|----------------");

            foreach (var cust in customers)
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
            string searchName = InputHelper.ReadNonEmptyString("Enter name to search: ");
            var results = _customerService.SearchCustomersByName(searchName);

            if (!results.Any())
            {
                throw new BusinessException("No customers found matching that name.");
            }

            Console.WriteLine("\n--- Search Results ---");
            foreach (var cust in results)
            {
                Console.WriteLine($"ID: {cust.Id} | Name: {cust.Name} | Phone: {cust.PhoneNumber}");
            }
        }
    }
}