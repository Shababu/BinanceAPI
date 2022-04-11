namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceSymbolsInfoDeserialization
    {
        public string Symbol { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string QuotePrecision { get; set; }

        public BinanceFiltersInfoDeserialization[] Filters { get; set; }
    }
}
