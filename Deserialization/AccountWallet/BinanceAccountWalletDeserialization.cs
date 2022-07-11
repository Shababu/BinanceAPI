using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceAccountWalletDeserialization
    {
        public BinanceAccountBalanceDeserialization[] Balances { get; set; }
        
        public static List<BinanceAccountBalanceDeserialization> DeserializeWalletInfo(string jsonString)
        {
            BinanceAccountWalletDeserialization info = JsonConvert.DeserializeObject<BinanceAccountWalletDeserialization>(jsonString);
            List<BinanceAccountBalanceDeserialization> assets = info.Balances.Where(b => Convert.ToDouble(b.Free.Replace('.', ',')) > 0).ToList();
            return assets;
        }
    }
}
