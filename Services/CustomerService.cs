
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class CustomerService : ICustomerService
    {
        private List<Customer> customersList;
        private int idCounter = 1;

        // الدالة البنائية تقرأ الزبائن المخزنين في الـ JSON فوراً
        public CustomerService()
        {
            customersList = DataStore.LoadCustomers() ?? new List<Customer>();
            UpdateIdCounter();
        }

        public void AddCustomer(Customer customer)
        {
            // التحقق من عدم تكرار رقم الهاتف
            bool isDuplicate = customersList.Any(c => c.PhoneNumber == customer.PhoneNumber);
            if (isDuplicate)
            {
                throw new BusinessException("Error: A customer with this phone number already exists.");
            }

            customer.Id = idCounter++;
            customersList.Add(customer);
            DataStore.SaveCustomers(customersList); // حفظ التغيير في ملف JSON حركياً
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return customersList;
        }

        public Customer GetCustomerById(int id)
        {
            return customersList.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> SearchCustomersByName(string name)
        {
            return customersList.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        private void UpdateIdCounter()
        {
            if (customersList != null && customersList.Count > 0)
            {
                idCounter = customersList.Max(c => c.Id) + 1;
            }
            else
            {
                idCounter = 1;
            }
        }
    }
}