using BinanceApiLibrary.Deserialization;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceWalletInfo : IWalletInfo
    {
        public string BaseUrl { get => "https://api.binance.com/"; }
        public string AccountInfoUrl { get => "api/v3/account?"; }

        public List<ICryptoBalance> GetWalletInfo(IExchangeUser user)
        {
            string response;
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();

            string url = BaseUrl + AccountInfoUrl;
            string parameters = "recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            List<BinanceAccountBalanceDeserialization> rawInfo = BinanceAccountWalletDeserialization.DeserializeWalletInfo(response);
            List<ICryptoBalance> balances = new List<ICryptoBalance>();

            foreach (var info in rawInfo)
            {
                balances.Add(BinanceCryptoBalance.ConvertToCryptoBalance(info));
            }

            balances.RemoveAll(b => b.Asset.StartsWith("LD"));
            BinanceCryptoBalance.CountRubValue(balances);
            return balances;
        }

        public decimal GetAccountTotalBalance(List<ICryptoBalance> balances)
        {
            decimal totalBalance = 0;
            foreach(var balance in balances)
            {
                totalBalance += balance.RubValue;
            }

            return totalBalance;
        }
    }
}
