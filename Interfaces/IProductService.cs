
using System.Collections.Generic;

namespace MyStore
{
    public interface IProductService
    {
        // دالة لإضافة منتج جديد
        void AddProduct(Product product);

        // دالة لجلب كل المنتجات (استخدمنا IEnumerable لمرونة القراءة)
        IEnumerable<Product> GetAllProducts();

        // دالة للبحث عن منتج بواسطة الرقم المعرف
        Product GetProductById(int id);

        // دالة للبحث عن المنتجات بواسطة الاسم
        IEnumerable<Product> SearchProductsByName(string name);

        // دالة لتعديل سعر المنتج
        void UpdateProductPrice(int id, double newPrice);

        // دالة لتعديل كمية المنتج
        void UpdateProductQuantity(int id, int newQuantity);

        // دالة لحذف منتج تماماً
        void DeleteProduct(int id);

        // دالة لحساب القيمة الإجمالية للمخزن كاملاً
        double GetGrandTotalValue();

        // دالة لتجميع القيمة المالية للمنتجات مقسمة حسب الفئة
        Dictionary<Category, double> GetValueBreakdownByCategory();
    }
}
