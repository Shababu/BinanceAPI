using System;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceAssetStatus : IAssetStatus
    {
        public string Symbol { get; set; }
        public decimal PriceChange { get; set; }
        public float PriceChangePercent { get; set; }
        public decimal WeightedAvgPrice { get; set; }
        public decimal PrevClosePrice { get; set; }
        public decimal LastPrice { get; set; }
        public decimal LastQty { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal Volume { get; set; }
        public decimal QuoteVolume { get; set; }

        internal static BinanceAssetStatus ConvertToAssetStatus(BinanceAssetStatsDeserialization asset)
        {
            BinanceAssetStatus assetStatus = new BinanceAssetStatus()
            {
                Symbol = asset.Symbol,
                PriceChange = Convert.ToDecimal(asset.PriceChange.Replace(".", ",")),
                PriceChangePercent = Convert.ToSingle(asset.PriceChangePercent.Replace(".", ",")),
                WeightedAvgPrice = Convert.ToDecimal(asset.WeightedAvgPrice.Replace(".", ",")),
                PrevClosePrice = Convert.ToDecimal(asset.PrevClosePrice.Replace(".", ",")),
                LastPrice = Convert.ToDecimal(asset.LastPrice.Replace(".", ",")),
                LastQty = Convert.ToDecimal(asset.LastQty.Replace(".", ",")),
                OpenPrice = Convert.ToDecimal(asset.OpenPrice.Replace(".", ",")),
                HighPrice = Convert.ToDecimal(asset.HighPrice.Replace(".", ",")),
                LowPrice = Convert.ToDecimal(asset.LowPrice.Replace(".", ",")),
                Volume = Convert.ToDecimal(asset.Volume.Replace(".", ",")),
                QuoteVolume = Convert.ToDecimal(asset.QuoteVolume.Replace(".", ",")),
            };

            return assetStatus;
        }

        public override string ToString()
        {
            return string.Format($"Symbol: {Symbol}\nPriceChange: {PriceChange}\nPriceChangePercent: {PriceChangePercent}\n" +
                $"WeightedAvgPrice: {WeightedAvgPrice}\nPrevClosePrice: {PrevClosePrice}\nLastPrice: {LastPrice}\nLastQty: {LastQty}\n" +
                $"OpenPrice: {OpenPrice}\nHighPrice: {HighPrice}\nLowPrice: {LowPrice}\nVolume: {Volume}\nQuoteVolume: {QuoteVolume}\n");
        }
    }
}
