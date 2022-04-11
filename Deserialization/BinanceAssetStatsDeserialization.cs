using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceAssetStatsDeserialization
    {
        public string Symbol { get; set; }
        public string PriceChange { get; set; }
        public string PriceChangePercent { get; set; }
        public string WeightedAvgPrice { get; set; }
        public string PrevClosePrice { get; set; }
        public string LastPrice { get; set; }
        public string LastQty { get; set; }
        public string OpenPrice { get; set; }
        public string HighPrice { get; set; }
        public string LowPrice { get; set; }
        public string Volume { get; set; }
        public string QuoteVolume { get; set; }

        public static BinanceAssetStatsDeserialization DeserializeAssetStats(string json)
        {
            return JsonConvert.DeserializeObject<BinanceAssetStatsDeserialization>(json);
        }
    }
}
