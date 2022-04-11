using System;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceFilledTrade : IFilledTrade
    {
        public long OrderId { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal QuoteQty { get; set; }
        public decimal Commission { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsMaker { get; set; }
        public Sides Side { get; set; }
        public override string ToString()
        {
            return string.Format($"Order Id: {OrderId}\nSymbol: {Symbol}\nPrice: {Price}\nQty: {Qty}\nQuoteQty: {QuoteQty}\n" +
                $"Commission: {Commission}\nTimeStamp: {TimeStamp}\nIsBuyer: {IsBuyer}\nIsMaker: {IsMaker}\nSide: {Side}");
        }
    }
}