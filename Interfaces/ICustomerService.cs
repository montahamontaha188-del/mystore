using System.Collections.Generic;

namespace MyStore
{
    public interface ICustomerService
    {
         void AddCustomer(Customer customer);
         IEnumerable<Customer> GetAllCustomers();
         IEnumerable<Customer> FindCustomersByName(string name);
         Customer GetCustomerById(int id);
         bool IsPhoneNumberExists(string phone);
    }
}