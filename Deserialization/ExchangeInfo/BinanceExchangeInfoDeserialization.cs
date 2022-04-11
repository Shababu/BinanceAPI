using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceExchangeInfoDeserialization
    {
        public BinanceSymbolsInfoDeserialization[] Symbols { get; set; }

        public static BinanceExchangeInfoDeserialization DeserializeExchangeInfo(string json)
        {
            BinanceExchangeInfoDeserialization symbols = JsonConvert.DeserializeObject<BinanceExchangeInfoDeserialization>(json);
            return symbols;
        }
    }
}
