namespace PriceCalculator.Model
{
    public class Promotion
    {
        public string Code { get; set; }

        public decimal Price { get; set; }

        public int NumberCustomer { get; set; }

        public decimal Discount { get; set; }
    }
}
