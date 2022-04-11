using System.Collections.Generic;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceSymbolInfo : ISymbolInfo
    {
        public string Symbol { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public decimal QuotePrecision { get; set; }

        public List<ISymbolFilter> Filters { get; set; }
    }
}
