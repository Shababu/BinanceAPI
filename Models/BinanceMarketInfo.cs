using Newtonsoft.Json;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceMarketInfo : IMarketInfo
    {
        public IAssetStatus Get24HourStatOnAsset(string symbol)
        {
            string url = $"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            return BinanceAssetStatus.ConvertToAssetStatus(BinanceAssetStatsDeserialization.DeserializeAssetStats(response));
        }
        public decimal GetPrice(string pairSymbol)
        {
            string url = $"https://api.binance.com/api/v3/ticker/price?symbol={pairSymbol}";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            var currentPrice = JsonConvert.DeserializeObject<Cryptocurrency>(response);

            if (currentPrice != null)
            {
                return currentPrice.Price;
            }
            else return 0;
        }
        public List<Cryptocurrency> GetPrice()
        {
            string url = $"https://api.binance.com/api/v3/ticker/price";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<List<Cryptocurrency>>(response);
        }
        public List<IAssetStatus> Get24HourStatOnAllAssets()
        {
            string url = $"https://api.binance.com/api/v3/ticker/24hr";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<IAssetStatus> assets = new List<IAssetStatus>();
            string[] assetsJson = response.Split(new string[] { "}," }, StringSplitOptions.RemoveEmptyEntries);

            assetsJson[0] = assetsJson[0].Trim('[');
            assetsJson[assetsJson.Length - 1] = assetsJson[assetsJson.Length - 1].Trim(']');
            for (int i = 0; i < assetsJson.Length - 1; i++)
            {
                assetsJson[i] = assetsJson[i] + '}';
            }

            foreach(var asset in assetsJson)
            {
                assets.Add(BinanceAssetStatus.ConvertToAssetStatus(BinanceAssetStatsDeserialization.DeserializeAssetStats(asset)));
            }

            return assets;
        }
        public List<ICandle> GetCandles(string symbol, string interval, int limit)
        {
            string url = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval}";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<ICandle> normalCandles = new List<ICandle>();
            List<BinanceCandlestickDeserialization> candles = BinanceCandlestickDeserialization.DeserializeCandlestick(response);

            foreach(var candle in candles)
            {
                normalCandles.Add(BinanceCandle.ConvertToCandle(candle));
            }

            return normalCandles;
        }
        public List<IDepth> GetDepth(string pairSymbol, int limit = 100)
        {
            string url = $"https://www.binance.com/api/v3/depth?symbol={pairSymbol}&limit={limit}";
            string response;

            using (HttpClient client = new())
            {
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<string> info = response.Split(':').ToList();
            info.RemoveRange(0, 2);

            List<string> bids = info[0].Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> asks = info[1].Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            bids[0] = bids[0].Trim('[').Trim('[');
            asks[0] = asks[0].Trim('[').Trim('[');
            bids.RemoveAt(bids.Count - 1);
            asks.RemoveAt(asks.Count - 1);
            
            List<IDepth> depth = new List<IDepth>();

            foreach (var bid in bids)
            {
                string[] info2 = bid.Split(',');
                decimal bidPrice = Convert.ToDecimal(info2[0].Trim('\"').Replace('.', ','));
                decimal bidQnty = Convert.ToDecimal(info2[1].Trim('\"').Replace('.', ','));
                depth.Add(new BinanceDepth("bid", bidQnty, bidPrice));
            }

            foreach (var ask in asks)
            {
                string[] info2 = ask.Split(',');
                decimal askPrice = Convert.ToDecimal(info2[0].Trim('\"').Replace('.', ','));
                decimal askQnty = Convert.ToDecimal(info2[1].Trim('\"').Replace('.', ','));
                depth.Add(new BinanceDepth("ask", askQnty, askPrice));
            }

            return depth;
        }
        internal string GetTimestamp()
        {
            return Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
        }
    }
}
