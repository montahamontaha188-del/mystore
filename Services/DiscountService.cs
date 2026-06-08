using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class DiscountService : IDiscountService
    {
        private List<Discount> discountsList = new();
        private int idCounter = 1;

        public void AddDiscount(Discount discount)
        {
            discount.Id = idCounter++;
            discount.IsActive = true;
            discountsList.Add(discount);
        }

        public IEnumerable<Discount> GetAllDiscounts()
        {
            return discountsList;
        }

        public Discount GetDiscountByCode(string code)
        {
            return discountsList.FirstOrDefault(d => d.Code == code);
        }

        public Discount GetActiveDiscount(string code)
        {
            return discountsList.FirstOrDefault(d => d.Code == code && d.IsActive);
        }

        public bool IsCodeExists(string code)
        {
            return discountsList.Any(d => d.Code == code);
        }
    }
}