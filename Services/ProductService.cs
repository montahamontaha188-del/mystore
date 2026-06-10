 
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ProductService : IProductService
    {
        private List<Product> productsList;
        private int idCounter = 1;

        // الدالة البنائية تقرأ البيانات من ملف الـ JSON فور تشغيل الخدمة
        public ProductService()
        {
            // ملاحظة: لو اسم كلاس الجيسون عندك مختلف (مثلا JsonContext)، غيري الاسماء هنا فقط
            productsList = DataStore.LoadProducts() ?? new List<Product>();
            UpdateIdCounter();
        }

        public void AddProduct(Product product)
        {
            product.Id = idCounter++;
            productsList.Add(product);
            DataStore.SaveProducts(productsList); // حفظ التغيير في ملف JSON
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return productsList;
        }

        public Product GetProductById(int id)
        {
            return productsList.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> SearchProductsByName(string name)
        {
            return productsList.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateProductPrice(int id, double newPrice)
        {
            var prod = GetProductById(id);
            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }
            prod.Price = newPrice;
            DataStore.SaveProducts(productsList); // حفظ التغيير في ملف JSON
        }

        public void UpdateProductQuantity(int id, int newQuantity)
        {
            var prod = GetProductById(id);
            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }
            prod.Quantity = newQuantity;
            DataStore.SaveProducts(productsList); // حفظ التغيير في ملف JSON
        }

        public void DeleteProduct(int id)
        {
            var prod = GetProductById(id);
            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }

            productsList.Remove(prod);

            // إعادة ترقيم الـ IDs لكي تظل متسلسلة ونظيفة
            for (int i = 0; i < productsList.Count; i++)
            {
                productsList[i].Id = i + 1;
            }

            idCounter = productsList.Count + 1;
            DataStore.SaveProducts(productsList); // حفظ التغيير في ملف JSON
        }

        public double GetGrandTotalValue()
        {
            return productsList.Sum(p => p.Price * p.Quantity);
        }

        public Dictionary<Category, double> GetValueBreakdownByCategory()
        {
            return productsList.GroupBy(p => p.Category)
                               .ToDictionary(g => g.Key, g => g.Sum(p => p.Price * p.Quantity));
        }

        private void UpdateIdCounter()
        {
            if (productsList != null && productsList.Count > 0)
            {
                idCounter = productsList.Max(p => p.Id) + 1;
            }
            else
            {
                idCounter = 1;
            }
        }
    }
}