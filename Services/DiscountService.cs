 
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class DiscountService : IDiscountService
    {
        private List<Discount> discountsList;
        private int idCounter = 1;

        // الدالة البنائية تقرأ الكود المخزن في الـ JSON تلقائياً
        public DiscountService()
        {
            discountsList = DataStore.LoadDiscounts() ?? new List<Discount>();
            UpdateIdCounter();
        }

        public void AddDiscount(Discount discount)
        {
            // التحقق من عدم تكرار الكود
            if (discountsList.Any(d => d.Code == discount.Code))
            {
                throw new BusinessException("Error: This discount code already exists.");
            }

            discount.Id = idCounter++;
            discount.IsActive = true; // أي كود جديد ينزل نشط تلقائياً

            discountsList.Add(discount);
            DataStore.SaveDiscounts(discountsList); // حفظ التغيير حركياً في ملف الـ JSON
        }

        public IEnumerable<Discount> GetAllDiscounts()
        {
            return discountsList;
        }

        public Discount GetActiveDiscount(string code)
        {
            return discountsList.FirstOrDefault(d => d.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && d.IsActive);
        }

        public void ToggleDiscountStatus(string code)
        {
            var discount = discountsList.FirstOrDefault(d => d.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            if (discount == null)
            {
                throw new BusinessException("Error: Discount code not found.");
            }

            discount.IsActive = !discount.IsActive; // قلب الحالة (نشط/غير نشط)
            DataStore.SaveDiscounts(discountsList); // حفظ الحالة الجديدة في الـ JSON
        }

        private void UpdateIdCounter()
        {
            if (discountsList != null && discountsList.Count > 0)
            {
                idCounter = discountsList.Max(d => d.Id) + 1;
            }
            else
            {
                idCounter = 1;
            }
        }
    }
}