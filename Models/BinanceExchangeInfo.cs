using BinanceApiLibrary.Deserialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceExchangeInfo : IExchangeInfo
    {
        public List<ISymbolInfo> ExchangeSymbolsInfo { get; set; }
        public List<ISimpleSymbol> ExchangeSymbols { get; set; }

        public BinanceExchangeInfo()
        {
            ExchangeSymbolsInfo = new List<ISymbolInfo>();
            ExchangeSymbols = new List<ISimpleSymbol>();
        }
        internal static BinanceExchangeInfo ConvertToExchangeInfo(BinanceExchangeInfoDeserialization exchangeInfoRaw)
        {
            BinanceExchangeInfo exchangeInfo = new BinanceExchangeInfo();

            foreach (var symbol in exchangeInfoRaw.Symbols)
            {
                BinanceSymbolInfo symbolInfo = new BinanceSymbolInfo()
                {
                    Symbol = symbol.Symbol,
                    BaseAsset = symbol.BaseAsset,
                    QuoteAsset = symbol.QuoteAsset,
                    QuotePrecision = Convert.ToDecimal(symbol.QuotePrecision),
                    Filters = new List<ISymbolFilter>()
                };

                foreach (var filter in symbol.Filters)
                {
                    symbolInfo.Filters.Add(new BinanceSymbolFilter
                    {
                        FilterType = filter.FilterType,
                        MinPrice = filter.MinPrice is null ? 0 : Convert.ToDecimal(filter.MinPrice.Replace('.', ',')),
                        MaxPrice = filter.MaxPrice is null ? 0 : Convert.ToDecimal(filter.MaxPrice.Replace('.', ',')),
                        PriceScale = filter.PriceScale is null ? 0 : Convert.ToDecimal(filter.PriceScale.Replace('.', ',')),
                        MinQty = filter.MinQty is null ? 0 : Convert.ToDecimal(filter.MinQty.Replace('.', ',')),
                        MinVal = filter.MinVal is null ? 0 : Convert.ToDecimal(filter.MinVal.Replace('.', ',')),
                    });
                }

                exchangeInfo.ExchangeSymbolsInfo.Add(symbolInfo);

                exchangeInfo.ExchangeSymbols.Add(new BinanceSimpleSymbol
                {
                    Symbol = symbol.Symbol.ToUpper(),
                    BaseAsset = symbol.BaseAsset.ToUpper(),
                    QuoteAsset = symbol.QuoteAsset.ToUpper()
                });
            }

            return exchangeInfo;
        }
        public IExchangeInfo GetExchangeInfo()
        {
            string url = "https://www.binance.com/api/v3/exchangeInfo";

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            BinanceExchangeInfoDeserialization symbols = BinanceExchangeInfoDeserialization.DeserializeExchangeInfo(response);
            return ConvertToExchangeInfo(symbols);
        }
    }
}
