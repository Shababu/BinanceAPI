using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceWalletInfo : IWalletInfo
    {
        public string BaseUrl => "https://api.binance.com/";
        public string AccountInfoUrl => "api/v3/account?";

        public List<ICryptoBalance> GetWalletInfo(IExchangeUser user)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string response;

            string url = BaseUrl + AccountInfoUrl;
            string parameters = "recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-MBX-APIKEY", user.ApiPublicKey);
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
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
