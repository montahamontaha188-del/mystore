
using System.Collections.Generic;

namespace MyStore
{
    public interface IProductService
    {
    
        void AddProduct(Product product);
         IEnumerable<Product> GetAllProducts();
         Product GetProductById(int id);
         IEnumerable<Product> SearchProductsByName(string name);
         void UpdateProductPrice(int id, double newPrice);
         void UpdateProductQuantity(int id, int newQuantity);
         void DeleteProduct(int id);
         double GetGrandTotalValue();
          Dictionary<Category, double> GetValueBreakdownByCategory();
    }
}
