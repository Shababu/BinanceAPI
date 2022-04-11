using TradingCommonTypes;

namespace BinanceApiLibrary 
{ 
    public class BinanceSimpleSymbol : ISimpleSymbol
    {
        public string Symbol { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
    }
}
