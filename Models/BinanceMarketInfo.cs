using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;
using System.Linq;

namespace BinanceApiLibrary
{
    public class BinanceMarketInfo : IMarketInfo
    {
        public IAssetStatus Get24HourStatOnAsset(string symbol)
        {
            string url = $"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}";

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return BinanceAssetStatus.ConvertToAssetStatus(BinanceAssetStatsDeserialization.DeserializeAssetStats(response));
        }
        public decimal GetPrice(string pairSymbol)
        {
            string url = $"https://api.binance.com/api/v3/ticker/price?symbol={pairSymbol}";

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            Cryptocurrency cryptoInfo = JsonConvert.DeserializeObject<Cryptocurrency>(response);
            return cryptoInfo.Price;
        }
        public List<Cryptocurrency> GetPrice()
        {
            string url = $"https://api.binance.com/api/v3/ticker/price";

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<Cryptocurrency>>(response);
        }
        public string GetTimestamp()
        {
            return Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
        }        
        public List<IAssetStatus> Get24HourStatOnAllAssets()
        {
            string url = $"https://api.binance.com/api/v3/ticker/24hr";
  
            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
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
            if(limit > 0)
            {
                url += $"&limit={limit}";
            }
            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse;
            try
            {
                HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();
            }
            catch (Exception)
            {
                return new List<ICandle>();
            }
            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
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

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
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
    }
}
