using System;
using System.Linq;

namespace MyStore
{
    public class CustomerMenu
    {
        private readonly ICustomerService _customerService;

 
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
                    case 1: AddCustomerUI(); break;
                    case 2: ListCustomersUI(); break;
                    case 3: FindCustomerByNameUI(); break;
                    case 0:
                        Console.WriteLine("Returning to main menu...");
                        return;
                }
            }
        }

        private void AddCustomerUI()
        {
            string name = InputHelper.ReadNonEmptyString("Enter customer name: ");
            string phone = InputHelper.ReadPhoneNumber("Enter phone number: ");

  
            if (_customerService.IsPhoneNumberExists(phone))
            {
                throw new BusinessException("Error: A customer with this phone number already exists.");
            }

            Customer newCustomer = new Customer
            {
                Name = name,
                PhoneNumber = phone
            };

            _customerService.AddCustomer(newCustomer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Customer '{name}' added successfully.");
            Console.ResetColor();
        }

        private void ListCustomersUI()
        {
            var customers = _customerService.GetAllCustomers().ToList();
            if (customers.Count == 0)
            {
                throw new BusinessException("No customers found.");
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Name".PadRight(20) + "| " + "Phone Number");
            Console.WriteLine("-----|---------------------|----------------");

            foreach (var cust in customers)
            {
                Console.WriteLine($"{cust.Id.ToString().PadRight(5)}| {cust.Name.PadRight(20)}| {cust.PhoneNumber}");
            }
        }

        private void FindCustomerByNameUI()
        {
            string searchName = InputHelper.ReadNonEmptyString("Enter name to search: ");
            var results = _customerService.FindCustomersByName(searchName).ToList();

            if (results.Count == 0)
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