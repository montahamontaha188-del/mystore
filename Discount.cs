using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; }      
        public double Percentage { get; set; } 
        public bool IsActive { get; set; }    
    }
    public class DiscountServices
    {
        private List<Discount> discountsList = new();
        private int idCounter = 1;

      
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
                    case 0: return;
                }
            }
        }

        private void AddDiscount()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code (e.g., SUMMER10): ").ToUpper();

          
            if (discountsList.Any(d => d.Code == code))
            {
                Console.WriteLine("Error: This discount code already exists.");
                return;
            }

            double percentage = InputHelper.ReadDouble("Enter discount percentage (1-100): ", 0.01);
            if (percentage > 100)
            {
                Console.WriteLine("Error: Percentage cannot exceed 100%.");
                return;
            }

            Discount newDiscount = new Discount
            {
                Id = idCounter++,
                Code = code,
                Percentage = percentage,
                IsActive = true 
            };

            discountsList.Add(newDiscount);
            Console.WriteLine($"Discount code '{code}' ({percentage}%) added successfully.");
        }

        private void ListDiscounts()
        {
            if (discountsList.Count == 0)
            {
                Console.WriteLine("No discount codes found.");
                return;
            }

            Console.WriteLine("\n" + "ID".PadRight(5) + "| " + "Code".PadRight(15) + "| " + "Percentage".PadRight(12) + "| " + "Status");
            Console.WriteLine("-----|----------------|------------|----------");
            foreach (var d in discountsList)
            {
                string status = d.IsActive ? "Active" : "Inactive";
                Console.WriteLine($"{d.Id.ToString().PadRight(5)}| {d.Code.PadRight(15)}| {d.Percentage.ToString() + "%".PadRight(11)}| {status}");
            }
        }

        private void ToggleDiscountStatus()
        {
            string code = InputHelper.ReadNonEmptyString("Enter discount code to activate/deactivate: ").ToUpper();
            var discount = discountsList.FirstOrDefault(d => d.Code == code);

            if (discount == null)
            {
                Console.WriteLine("Error: Discount code not found.");
                return;
            }

            discount.IsActive = !discount.IsActive; 
            string status = discount.IsActive ? "Activated" : "Deactivated";
            Console.WriteLine($"Discount code '{code}' has been {status} successfully.");
        }

        
        public Discount GetActiveDiscount(string code)
        {
            return discountsList.FirstOrDefault(d => d.Code == code && d.IsActive);
        }
    }
}
