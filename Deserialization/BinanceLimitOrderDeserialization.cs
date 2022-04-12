using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceLimitOrderDeserialization
    {
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public string Price { get; set; }
        public string OrigQty { get; set; }
        public string ExecutedQty { get; set; }
        public string Status { get; set; }
        public string Side { get; set; }
        public string ConvertedTime { get; set; }
        public string Time { get; set; }
        public string TransactTime { get; set; }


        public static BinanceLimitOrderDeserialization DeserializeLimitOrder(string jsonString)
        {
            BinanceLimitOrderDeserialization limitOrder = JsonConvert.DeserializeObject<BinanceLimitOrderDeserialization>(jsonString);
            if (limitOrder.Time != null)
            {
                limitOrder.ConvertedTime = ConvertOrderTime(limitOrder.Time);
            }
            else
            {
                limitOrder.ConvertedTime = ConvertOrderTime(limitOrder.TransactTime);
            }
            return limitOrder;
        }

        public static List<BinanceLimitOrderDeserialization> DeserializeLimitOrders(string jsonString)
        {
            object[] trades = JsonConvert.DeserializeObject<object[]>(jsonString);
            List<BinanceLimitOrderDeserialization> orders = new List<BinanceLimitOrderDeserialization>();
            foreach (var order in trades)
            {
                BinanceLimitOrderDeserialization limitOrder = JsonConvert.DeserializeObject<BinanceLimitOrderDeserialization>(order.ToString());
                limitOrder.ConvertedTime = ConvertOrderTime(limitOrder.Time);
                orders.Add(limitOrder);
            }
            return orders;
        }

        public static string ConvertOrderTime(string timeString)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            double timestamp = Convert.ToDouble(timeString);
            return dateTime.AddMilliseconds(timestamp).ToLocalTime().ToString();

            //return (new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(Convert.ToInt64(timeString))).AddHours(3).ToString();
        }
    }
}
