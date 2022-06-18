using Newtonsoft.Json;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceAccountInfo : IAccountInfo
    {
        public string BaseUrl => "https://api.binance.com/";
        public string TradeUrl => "api/v3/myTrades?";

        public List<IFilledTrade> GetTrades(IExchangeUser user, string symbol)
        {
            string response;
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();

            string url = BaseUrl + TradeUrl;
            string parameters = "recvWindow=10000&symbol=" + symbol + "&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-MBX-APIKEY", user.ApiPublicKey);
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<object> trades = JsonConvert.DeserializeObject<List<object>>(response);
            List<IFilledTrade> listOfTrades = new List<IFilledTrade>();
            foreach (var trade in trades)
            {
                BinanceFilledTrade tradeInfo = new BinanceFilledTrade();
                string tradeString = trade.ToString();
                tradeString = tradeString.Trim('{', '}');
                string[] tradeStrings = tradeString.Split(',');
                tradeInfo.OrderId = Convert.ToInt64(tradeStrings[1].Substring(10));
                tradeInfo.Symbol = tradeStrings[0].Substring(15).Trim('"');
                tradeInfo.Price = Convert.ToDecimal(tradeStrings[4].Substring(13).Trim('"').Replace('.', ','));
                tradeInfo.Qty = Convert.ToDecimal(tradeStrings[5].Substring(11).Trim('"').Replace('.', ','));
                tradeInfo.QuoteQty = Convert.ToDecimal(tradeStrings[6].Substring(16).Trim('"').Replace('.', ','));
                tradeInfo.Commission = Convert.ToDecimal(tradeStrings[7].Substring(18).Trim('"').Replace('.', ','));
                tradeInfo.TimeStamp = BinanceApiUser.ConvertTimeStampToDateTime(Convert.ToDouble(tradeStrings[9].Substring(12).Trim('"')));
                tradeInfo.IsBuyer = Convert.ToBoolean(tradeStrings[10].Substring(15).Trim('"'));
                tradeInfo.IsMaker = Convert.ToBoolean(tradeStrings[11].Substring(15).Trim('"'));
                if (tradeInfo.IsBuyer)
                {
                    tradeInfo.Side = Sides.BUY;
                }
                else
                {
                    tradeInfo.Side = Sides.SELL;
                }

                listOfTrades.Add(tradeInfo);
            }
            listOfTrades.Reverse();
            return listOfTrades;
        }
    }
}
