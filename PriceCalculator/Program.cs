using PriceCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    class Program
    {
        public static string PartFile = @"D:\Promotion.txt";

        static void Main(string[] args)
        {
            //----------------------------input value---------------------------------//
            Console.WriteLine("Number of customers = ");
            int numberOfCustomers = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Price per person = ");
            decimal pricePerPerson = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("coupon code = ");
            var conponCode = Console.ReadLine();
            //------------------------------------------------------------------------//

            var totalPrice = numberOfCustomers * pricePerPerson;
            var prices = CalculatePrice(numberOfCustomers, totalPrice, conponCode);

            if (prices.Count > 0)
            {
                // find price that customer pay least.
                var pay = FindPricesLeast(prices);
                Console.WriteLine(pay.ToString());
            }
            else
            {
                Console.WriteLine(totalPrice.ToString());
            }

            Console.ReadLine();
        }

        private static decimal FindPricesLeast(List<decimal> prices)
        {
            return prices.Min();
        }

        private static Promotion ConvertData(string[] values)
        {
            var promotion = new Promotion();
            if (decimal.TryParse(values[0], out var price))
            {
                promotion.Price = price;
            }

            if (!string.IsNullOrEmpty(values[1]))
            {
                promotion.Code = values[1].ToString();
            }

            if (int.TryParse(values[2], out var custumers))
            {
                promotion.NumberCustomer = custumers;
            }

            if (decimal.TryParse(values[3], out var discount))
            {
                promotion.Discount = discount;
            }

            return promotion;
        }

        private static List<decimal> CalculatePrice(int customerInputNumber, decimal totalPrice, string coupon)
        {
            var prices = new List<decimal>();

            // Currenly Rule
            //  1.Discount 10 % when customer present coupon code "DIS10" or price is more / equal than 2000 baht
            //  2.Discount 30 % when customer present coupon code "STARCARD" for 2 customers
            //  3.Come 4 pay 3 when customer present coupon code "STARCARD" ***discount 25%
            //  4.Discount 25 % when price more / equal that 2500 baht.

            var promotions = new List<Promotion>();

            // read file
            string[] lines = System.IO.File.ReadAllLines(PartFile);
            foreach (string line in lines)
            {
                var values = line.Split(',');
                promotions.Add(ConvertData(values));
            }

            // calculate
            foreach (var promotion in promotions)
            {
                if (!string.IsNullOrEmpty(coupon) && (promotion.Code != null))
                {
                    if (promotion.Code.Equals(coupon))
                    {
                        if (promotion.NumberCustomer > 0 && promotion.NumberCustomer == customerInputNumber)
                        {
                            prices.Add(CalculateDiscount(totalPrice, promotion.Discount));
                        }

                        if (promotion.NumberCustomer == 0)
                        {
                            prices.Add(CalculateDiscount(totalPrice, promotion.Discount));
                        }
                    }
                }

                if (totalPrice >= promotion.Price && promotion.Price > 0)
                {
                    prices.Add(CalculateDiscount(totalPrice, promotion.Discount));
                }
            }

            return prices;
        }

        private static decimal CalculateDiscount(decimal totalPrice, decimal discount)
        {
            return totalPrice - (totalPrice * (discount/100));
        }
    }
}
