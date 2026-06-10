using System.Collections.Generic;

namespace MyStore
{
    public interface ICustomerService
    {
        void AddCustomer(Customer customer);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        IEnumerable<Customer> SearchCustomersByName(string name);
    }
}