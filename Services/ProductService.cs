
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class ProductService : IProductService
    {
        private List<Product> productsList = new();
        private int idCounter = 1;

        public void AddProduct(Product product)
        {
            product.Id = idCounter++;
            productsList.Add(product);
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
        }

        public void UpdateProductQuantity(int id, int newQuantity)
        {
            var prod = GetProductById(id);
            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }
            prod.Quantity = newQuantity;
        }

        public void DeleteProduct(int id)
        {
            var prod = GetProductById(id);
            if (prod == null)
            {
                throw new BusinessException("Error: Product ID not found.");
            }

            productsList.Remove(prod);

            // إعادة ترتيب المعرفات تلقائياً بعد الحذف لضمان التسلسل
            for (int i = 0; i < productsList.Count; i++)
            {
                productsList[i].Id = i + 1;
            }
            idCounter = productsList.Count + 1;
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
    }
}