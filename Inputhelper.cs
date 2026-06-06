using System;
using System.Globalization;


namespace MyStore
{
    public static class InputHelper
    {
        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine($"Error: Please enter a valid number between {min} and {max}.");
            }
        }

        public static double ReadDouble(string prompt, double min=0 )
        {
            
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
                string input = Console.ReadLine();

                if (double.TryParse(input, out double result) && result >= min)
                {
                    return result;
                }
                Console.WriteLine($"Error: Please enter a valid number greater than or equal to {min}.");
            }
        }

        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                Console.WriteLine("Error: Input cannot be empty. Please try again.");
            }
        }

        public static bool Confirm(string prompt)
        {
            while (true)
            {
                Console.Write(prompt + " (y/n): ");
                string input = Console.ReadLine().ToLower();

                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;

                Console.WriteLine("Error: Please enter 'y' for Yes or 'n' for No.");
            }
        }
        public static string ReadPhoneNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

              
                if (!string.IsNullOrEmpty(input) && input.All(char.IsDigit))
                {
                    return input; 
                }

                Console.WriteLine("Error: Phone number cannot be empty and must contain only digits.");
            }
        }
        public static string ReadOptionalCode(string prompt)
        {
            if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
            string input = Console.ReadLine();

         
            if (string.IsNullOrWhiteSpace(input))
            {
                return ""; 
            }

           
            return input.Trim().ToUpper();
        }
         

        public static DateTime ReadDate(string prompt)
        {
        while (true)
        {
            Console.Write(prompt);
            string dateInput = Console.ReadLine();

            if (DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate))
            {
                return validDate; 
            }

            Console.WriteLine("Error: Invalid date format. Please use dd-MM-yyyy (e.g., 06-06-2026).");
        }
        }
}
}