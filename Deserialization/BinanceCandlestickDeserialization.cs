using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceCandlestickDeserialization
    {
        public string OpenTime { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }


        public static List<BinanceCandlestickDeserialization> DeserializeCandlestick(string jsonString)
        {
            List<BinanceCandlestickDeserialization> result = new List<BinanceCandlestickDeserialization>();

            string[] candles = jsonString.Split(new string[] { "]," }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < candles.Length; i++)
            {
                string[] candleInfo = candles[i].Split(',');

                for(int j = 0; j < candleInfo.Length; j++)
                {
                    candleInfo[j] = candleInfo[j].Trim('[').Trim('[').Trim('"');
                }
                result.Add(new BinanceCandlestickDeserialization
                {
                    OpenTime = candleInfo[0],
                    Open = candleInfo[1],
                    High = candleInfo[2],
                    Low = candleInfo[3],
                    Close = candleInfo[4],
                    Volume = candleInfo[5]
                });
            }
            return result;
        }

        public override string ToString()
        {
            return string.Format($"Open Time: {OpenTime}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}\n");
        }
    }
}
