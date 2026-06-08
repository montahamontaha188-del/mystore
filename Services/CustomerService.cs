using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class CustomerService : ICustomerService
    {
        private List<Customer> customersList = new();
        private int idCounter = 1;

        public void AddCustomer(Customer customer)
        {
            customer.Id = idCounter++;
            customersList.Add(customer);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return customersList;
        }

        public IEnumerable<Customer> FindCustomersByName(string name)
        {
            return customersList.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Customer GetCustomerById(int id)
        {
            return customersList.FirstOrDefault(c => c.Id == id);
        }

        public bool IsPhoneNumberExists(string phone)
        {
            return customersList.Any(c => c.PhoneNumber == phone);
        }
    }
}