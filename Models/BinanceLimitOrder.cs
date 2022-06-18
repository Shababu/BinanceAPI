using System.Text;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceLimitOrder : ILimitOrder
    {
        public string Symbol { get; set; }
        public long OrderId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal ExecutedQty { get; set; }
        public string Status { get; set; }
        public string Side { get; set; }
        public DateTime Time { get; set; }

        public BinanceLimitOrder() { }

        public BinanceLimitOrder(string symbol, Sides side, decimal quantity, decimal price)
        {
            Symbol = symbol;
            Price = price;
            Quantity = quantity;
            if (side == Sides.BUY)
            {
                Side = "BUY";
            }
            else
            {
                Side = "SELL";
            }
        }

        internal static BinanceLimitOrder ConvertToLimitOrder(BinanceLimitOrderDeserialization order)
        {
            BinanceLimitOrder limitOrder = new BinanceLimitOrder()
            {
                Symbol = order.Symbol,
                OrderId = Convert.ToInt64(order.OrderId),
                Price = Convert.ToDecimal(order.Price.Replace('.', ',')),
                Quantity = Convert.ToDecimal(order.OrigQty.Replace('.', ',')),
                ExecutedQty = Convert.ToDecimal(order.ExecutedQty.Replace('.', ',')),
                Status = order.Status,
                Side = order.Side,
                Time = Convert.ToDateTime(order.ConvertedTime),
            };

            return limitOrder;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Symbol: {Symbol}\n");
            sb.Append($"Order Id: {OrderId}\n");
            sb.Append($"Price: {Price}\n");
            sb.Append($"Quantity: {Quantity}\n");
            sb.Append($"ExecutedQty: {ExecutedQty}\n");
            sb.Append($"Status: {Status}\n");
            sb.Append($"Side: {Side}\n");
            sb.Append($"Time: {Time}\n");

            return sb.ToString();
        }
    }
}
