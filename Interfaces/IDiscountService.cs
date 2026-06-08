using System.Collections.Generic;

namespace MyStore
{
    public interface IDiscountService
    {
        void AddDiscount(Discount discount);
        IEnumerable<Discount> GetAllDiscounts();
        Discount GetDiscountByCode(string code);
        Discount GetActiveDiscount(string code);
        bool IsCodeExists(string code);
    }
}