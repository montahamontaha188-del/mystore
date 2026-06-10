using System;
using System.Collections.Generic;
using System.Linq;

namespace MyStore
{
    public class DiscountMenu
    {
        private readonly IDiscountService _discountService;

        // حقن الخدمة عبر الـ Constructor
        public DiscountMenu(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public void DisplayDiscountMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- DISCOUNTS MENU ---");
                Console.WriteLine("1. Add Discount Code");
                Console.WriteLine("2. List All Discount Codes");
                Console.WriteLine("3. Toggle Code Active/Inactive");
                Console.WriteLine("0. Back to Main Menu");

                int choice = InputHelper.ReadInt("Select an option: ", 0, 3);

                switch (choice)
                {
                    case 1: AddDiscount(); break;
                    case 2: ListDiscounts(); break;
                    case 3: ToggleDiscountStatus(); break;
                    case 0: Console.WriteLine("Returning to main menu..."); return;
                }
            }
        }

        private void AddDiscount()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code (e.g., SUMMER10): ").ToUpper();
            double percentage = InputHelper.ReadDouble("Enter discount percentage (1-100): ", 0.01);

            if (percentage > 100)
            {
                throw new BusinessException("Error: Percentage cannot exceed 100%.");
            }

            Discount newDiscount = new Discount
            {
                Code = code,
                Percentage = percentage
            };

            // استدعاء الخدمة للتأكد من التكرار والحفظ في الجيسون
            _discountService.AddDiscount(newDiscount);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Discount code '{code}' ({percentage}%) added successfully.");
            Console.ResetColor();
        }

        private void ListDiscounts()
        {
            var discounts = _discountService.GetAllDiscounts();
            if (!discounts.Any())
            {
                throw new BusinessException("No discount codes found.");
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Code".PadRight(15) + "| " + "Percentage".PadRight(12) + "| " + "Status");
            Console.WriteLine("-----|----------------|------------|----------");

            foreach (var d in discounts)
            {
                string status = d.IsActive ? "Active" : "Inactive";
                Console.WriteLine($"{d.Id.ToString().PadRight(5)}| {d.Code.PadRight(15)}| {d.Percentage.ToString() + "%".PadRight(11)}| {status}");
            }
        }

        private void ToggleDiscountStatus()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code to activate/deactivate: ").ToUpper();

            // استدعاء الخدمة لقلب الحالة حركياً في الملف
            _discountService.ToggleDiscountStatus(code);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Discount code '{code}' status toggled successfully.");
            Console.ResetColor();
        }
    }
}