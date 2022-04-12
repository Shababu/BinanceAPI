using System.Collections.Generic;
using System.Text;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Symbol: {Symbol}\n");
            sb.Append($"Base Asset: {BaseAsset}\n");
            sb.Append($"Quote Asset: {QuoteAsset}\n");
            sb.Append($"Quote Precision: {QuotePrecision}\n");

            return sb.ToString();
        }
    }
}
