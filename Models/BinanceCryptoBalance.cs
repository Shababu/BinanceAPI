using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary 
{
    public class BinanceCryptoBalance : ICryptoBalance
    {
        public string Asset { get; set; }
        public decimal Total { get; set; }
        public decimal Free { get; set; }
        public decimal Locked { get; set; }
        public decimal RubValue { get; set; }

        internal static BinanceCryptoBalance ConvertToCryptoBalance(BinanceAccountBalanceDeserialization accountBalance)
        {
            BinanceCryptoBalance binanceCrypto = new BinanceCryptoBalance()
            {
                Asset = accountBalance.Asset,
                Total = accountBalance.Total,
                Free = Convert.ToDecimal(accountBalance.Free.ToString().Replace(".", ",")),
                Locked = Convert.ToDecimal(accountBalance.Locked.ToString().Replace(".", ",")),
                RubValue = accountBalance.RubValue,
            };

            return binanceCrypto;
        }

        public override string ToString()
        {
            return string.Format($"Asset {Asset}\nTotal: {Total}\nFree: {Free}\nLocked: {Locked}\nRubValue: {RubValue}\n");
        }

        internal static void CountRubValue(List<ICryptoBalance> balances)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            List<Cryptocurrency> allPairs = binanceMarketInfo.GetPrice();
            decimal rubPrice = allPairs.Where(pair => pair.Symbol == "USDTRUB").First().Price;

            foreach(var balance in balances)
            {
                if (balance.Asset == "RUB")
                {
                    balance.RubValue = balance.Total;
                }
                else if (balance.Asset.Contains("USD"))
                {
                    balance.RubValue = balance.Total * rubPrice;
                }
                else
                {
                    try
                    {
                        balance.RubValue = allPairs.Where(crypto => crypto.Symbol == balance.Asset + "USDT").First().Price * balance.Total * rubPrice;
                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
