using System;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceAccountBalanceDeserialization
    {
        public decimal Total => Convert.ToDecimal(Free.Replace(".", ",")) + Convert.ToDecimal(Locked.Replace(".", ","));
        public string Asset { get; set; }
        public string Free { get; set; }
        public string Locked { get; set; }
        public decimal RubValue {get;set; }
    }
}
