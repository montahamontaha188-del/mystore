using System;
using System.Linq;

namespace MyStore
{
    public class DiscountMenu
    {
        private readonly IDiscountService _discountService;

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
                    case 1: AddDiscountUI(); break;
                    case 2: ListDiscountsUI(); break;
                    case 3: ToggleDiscountStatusUI(); break;
                    case 0: return;
                }
            }
        }

        private void AddDiscountUI()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code (e.g., SUMMER10): ").ToUpper();

            if (_discountService.IsCodeExists(code))
            {
                throw new BusinessException("Error: This discount code already exists.");
            }

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

            _discountService.AddDiscount(newDiscount);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Discount code '{code}' ({percentage}%) added successfully.");
            Console.ResetColor();
        }

        private void ListDiscountsUI()
        {
            var discounts = _discountService.GetAllDiscounts().ToList();
            if (discounts.Count == 0)
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

        private void ToggleDiscountStatusUI()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code to activate/deactivate: ").ToUpper();
            var discount = _discountService.GetDiscountByCode(code);

            if (discount == null)
            {
                throw new BusinessException("Error: Discount code not found.");
            }

            discount.IsActive = !discount.IsActive;
            string status = discount.IsActive ? "Activated" : "Deactivated";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Discount code '{code}' has been {status} successfully.");
            Console.ResetColor();
        }
    }
}