using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceDepth : IDepth
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public BinanceDepth(string name, decimal quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
        public BinanceDepth()
        {
            Name = "";
            Quantity = 0;
            Price = 0;
        }

        public override string ToString()
        {
            return string.Format($"{Name} Price: {Price}, Quantity: {Quantity}");
        }
    }
}
