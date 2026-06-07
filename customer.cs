
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MyStore
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
            
            while (true)
            {
                Console.WriteLine("\n--- CUSTOMER MENU ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. List All Customers");
                Console.WriteLine("3. Find Customer by Name");
                Console.WriteLine("0. Back to Main Menu");
               
                int choice2 = InputHelper.ReadInt("Select an option: ",0,3);

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
                        return;
                        
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private void AddCustomer()
        {
             
            string name = InputHelper.ReadNonEmptyString("Enter customer name: ");
            string phone = InputHelper.ReadPhoneNumber("Enter phone number: ");

       
            bool isDuplicate = customersList.Any(c => c.PhoneNumber == phone);
            if (isDuplicate)
            {
                    throw new BusinessException("Error: A customer with this phone number already exists.");
               
            }

    
            Customer newCustomer = new Customer
            {
                Id = idCounter++,
                Name = name,
                PhoneNumber = phone
            };

            customersList.Add(newCustomer);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Customer '{name}' added successfully with ID: {newCustomer.Id}");
            Console.ResetColor();

        }
        private void ListCustomers()
        {
  
            if (customersList.Count == 0)
            {
                throw new BusinessException("No customers found.");
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

            string searchName = InputHelper.ReadNonEmptyString("Enter name to search: ");
            var results = customersList.Where(c => c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

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
        public Customer GetCustomerById(int id)
        {
            return customersList.FirstOrDefault(c => c.Id == id);
        }
    }
}
