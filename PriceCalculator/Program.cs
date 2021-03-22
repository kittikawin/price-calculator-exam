using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    class Program
    {
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
            var ruleWithTotalPrice = CalculatePrice(numberOfCustomers, totalPrice, conponCode);

            if (ruleWithTotalPrice.Count > 0)
            {
                // find price that customer pay least.
                var pay = FindPricesLeast(ruleWithTotalPrice);
                Console.WriteLine(pay.ToString());
            }
            else
            {
                Console.WriteLine(totalPrice.ToString());
            }
        }

        private static decimal FindPricesLeast(Dictionary<int, decimal> ruleWitnTotalPrice)
        {
            return ruleWitnTotalPrice.Values.Min();
        }

        private static Dictionary<int, decimal> CalculatePrice(int customers, decimal prices, string coupon)
        {
            // Rule
            //  1.Discount 10 % when customer present coupon code "DIS10" or price is more / equal than 2000 baht
            //  2.Discount 30 % when customer present coupon code "STARCARD" for 2 customers
            //  3.Come 4 pay 3 when customer present coupon code "STARCARD"
            //  4.Discount 25 % when price more / equal that 2500 baht.

            var rule_totalPrices = new Dictionary<int, decimal>(); // key is rule and and value is money to be paid!!

            if (!string.IsNullOrEmpty(coupon)) // calculate from coupon first
            {
                if (coupon.Equals("DIS10"))
                {
                    var (rule, totalPrice) = CalculateRuleOne(prices);
                    rule_totalPrices.Add(rule, totalPrice);
                }

                if (coupon.Equals("STARCARD"))
                {
                    if (customers == 2)
                    {
                        var (rule, totalPrice) = CalculateRuleTwo(prices);
                        rule_totalPrices.Add(rule, totalPrice);
                    }
                    else if (customers == 4)
                    {
                        var (rule, totalPrice) = CalculateRuleThree(prices);
                        rule_totalPrices.Add(rule, totalPrice);
                    }
                }
            }

            if (prices >= 2500)
            {
                var (rule, totalPrice) = CalculateRuleFour(prices);
                rule_totalPrices.Add(rule, totalPrice);
            }

            if (prices >= 2000 && prices < 2500)
            {
                var (rule, totalPrice) = CalculateRuleOne(prices);
                rule_totalPrices.Add(rule, totalPrice);
            }

            return rule_totalPrices;
        }

        private static (int rule, decimal totalPrice) CalculateRuleOne(decimal price)
        {
            // discount 10%
            return (1, price - (price * 0.1m));
        }

        private static (int rule, decimal totalPrice) CalculateRuleTwo(decimal price)
        {
            // discount 30%
            return (2, price - (price * 0.3m));
        }

        private static (int rule, decimal totalPrice) CalculateRuleThree(decimal price)
        {
            // come 4 pay 3
            return (3, price - (price / 4));
        }

        private static (int rule, decimal totalPrice) CalculateRuleFour(decimal price)
        {
            // discount 25%
            return (4, price - (price * 0.25m));
        }
    }
}
