using System.Collections.Generic;

namespace MyStore
{
    public interface ICustomerService
    {
        // دالة لإضافة زبون جديد
        void AddCustomer(Customer customer);

        // دالة لجلب كل الزبائن في النظام
        IEnumerable<Customer> GetAllCustomers();

        // دالة للبحث عن الزبائن بواسطة الاسم
        IEnumerable<Customer> FindCustomersByName(string name);

        // دالة للبحث عن زبون محدد بواسطة الرقم المعرف
        Customer GetCustomerById(int id);

        // دالة للتحقق من وجود رقم الهاتف مسبقاً في النظام
        bool IsPhoneNumberExists(string phone);
    }
}