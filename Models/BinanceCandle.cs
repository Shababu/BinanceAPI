using System;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceCandle : ICandle
    {
        public DateTime OpenTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        public BinanceCandle(DateTime openTime, double open, double high, double low, double close, double volume)
        {
            OpenTime = openTime;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        internal static BinanceCandle ConvertToCandle(BinanceCandlestickDeserialization candlestick)
        {
            double openTime = Convert.ToDouble(candlestick.OpenTime);
            DateTime dateTime = BinanceApiUser.ConvertTimeStampToDateTime(openTime);

            return new BinanceCandle(dateTime, Convert.ToDouble(candlestick.Open.Replace('.', ',')), Convert.ToDouble(candlestick.High.Replace('.', ',')), 
                Convert.ToDouble(candlestick.Low.Replace('.', ',')), Convert.ToDouble(candlestick.Close.Replace('.', ',')), 
                Convert.ToDouble(candlestick.Volume.Replace('.', ',')));            
        }

        public override string ToString()
        {
            return string.Format($"Open Time: {OpenTime}, Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}\n");
        }
    } 
}
