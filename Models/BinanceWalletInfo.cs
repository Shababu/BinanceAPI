using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceWalletInfo : IWalletInfo
    {
        private readonly string baseUrl = "https://api.binance.com/";

        public List<ICryptoBalance> GetWalletInfo(IExchangeUser user)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string response;

            string url = baseUrl + "api/v3/account?";
            string parameters = "recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp(DateTime.UtcNow);
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
        public List<IDeposit> GetRecentDeposits(IExchangeUser user, string coin = null, DateTime startTime = default(DateTime), DateTime endTime = default(DateTime))
        {
            string url = baseUrl + "sapi/v1/capital/withdraw/history?";
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string response;

            string parameters = "recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp(DateTime.UtcNow);

            if(coin != null)
            {
                parameters += $"&coin={coin}";
            }

            if (startTime != default(DateTime))
            {
                parameters += $"&startTime={binanceMarketInfo.GetTimestamp(startTime)}";

                if(endTime == default(DateTime) || endTime < startTime)
                {
                    endTime = startTime.AddDays(90);
                }
                else
                {
                    if (endTime.Subtract(startTime).Days > 90)
                    {
                        endTime = startTime.AddDays(90);
                    }
                }
                parameters += $"&endTime={binanceMarketInfo.GetTimestamp(endTime)}";
            }


            url += parameters + "&signature=" + user.Sign(parameters);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-MBX-APIKEY", user.ApiPublicKey);
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<BinanceDepositDeserialization> rawDeposits = BinanceDepositDeserialization.DeserializeDeposit(response);
            List<IDeposit> result = new List<IDeposit>();

            foreach (var rawDeposit in rawDeposits)
            {
                result.Add(BinanceDeposit.ConvertToDeposit(rawDeposit));
            }

            return result;
        }
        public List<IWithdrawal> GetRecentWithdrawals(IExchangeUser user, string coin = null, DateTime startTime = default(DateTime), DateTime endTime = default(DateTime))
        {
            string url = baseUrl + "sapi/v1/capital/withdraw/history?";
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string response;

            string parameters = "recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp(DateTime.UtcNow);

            if (coin != null)
            {
                parameters += $"&coin={coin}";
            }

            if (startTime != default(DateTime))
            {
                parameters += $"&startTime={binanceMarketInfo.GetTimestamp(startTime)}";

                if (endTime == default(DateTime) || endTime < startTime)
                {
                    endTime = startTime.AddDays(90);
                }
                else
                {
                    if (endTime.Subtract(startTime).Days > 90)
                    {
                        endTime = startTime.AddDays(90);
                    }
                }
                parameters += $"&endTime={binanceMarketInfo.GetTimestamp(endTime)}";
            }

            url += parameters + "&signature=" + user.Sign(parameters);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-MBX-APIKEY", user.ApiPublicKey);
                response = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }

            List<BinanceWithdrawalDeserialization> rawWithdrawals = BinanceWithdrawalDeserialization.DeserializeWithdrawal(response);
            List<IWithdrawal> result = new List<IWithdrawal>();

            foreach (var rawWithdrawal in rawWithdrawals)
            {
                result.Add(BinanceWithdrawal.ConvertToWithdrawal(rawWithdrawal));
            }

            return result;
        }
    }
}
