namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceFiltersInfoDeserialization
    {
        public string FilterType { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string PriceScale { get; set; }
        public string MinQty { get; set; }
        public string MinVal { get; set; }
    }
}
