using System.Collections.Generic;

namespace MyStore
{
    public interface IDiscountService
    {
        void AddDiscount(Discount discount);
        IEnumerable<Discount> GetAllDiscounts();
        Discount GetActiveDiscount(string code);
        void ToggleDiscountStatus(string code);
    }
}