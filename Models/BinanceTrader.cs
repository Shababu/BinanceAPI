using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
  
    public class BinanceTrader : ITrader
    {
        public ILimitOrder PlaceNewLimitOrder(IExchangeUser user, string symbol, Sides side, decimal quantity, decimal price)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/order?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&side={side}&type=LIMIT&timeInForce=GTC&quantity={quantity.ToString().Replace(",",".")}&price={price.ToString().Replace(",", ".")}&recvWindow=20000&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HTTPrequest.Method = "POST";
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }
            
            return BinanceLimitOrder.ConvertToLimitOrder(BinanceLimitOrderDeserialization.DeserializeLimitOrder(response));
        }
        public ILimitOrder PlaceNewMarketOrder(IExchangeUser user, string symbol, Sides side, decimal quantity)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/order?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&side={side}&type=MARKET&quantity={quantity.ToString().Replace(",", ".")}&recvWindow=20000&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HTTPrequest.Method = "POST";
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return BinanceLimitOrder.ConvertToLimitOrder(BinanceLimitOrderDeserialization.DeserializeLimitOrder(response));
        }
        public void AutoTrade(IExchangeUser user, List<SpotPosition> positions, bool increaseAmount)
        {
            BinanceTrader trader = new BinanceTrader();
            ILimitOrder newLimitOrder;
            foreach (var position in positions)
            {
                if (!position.IsBought && !position.IsBuyOrderPlaced)
                {
                    newLimitOrder = PlaceNewLimitOrder(user, position.Symbol, Sides.BUY, position.Amount, position.BuyingPrice);
                    UpdateOrderStatusAndId(position, newLimitOrder);                        
                    position.IsBuyOrderPlaced = true;
                    position.InspectSpotPosition();
                }

                else if (!position.IsBought && position.IsBuyOrderPlaced)
                {
                    position.Status = trader.GetOrderStatus(user, position, position.Symbol);
                    if (position.Status == "FILLED")
                    {
                        position.IsBought = true;
                        position.InspectSpotPosition();

                        newLimitOrder = PlaceNewLimitOrder(user, position.Symbol, Sides.SELL, position.Amount, position.SellingPrice);
                        UpdateOrderStatusAndId(position, newLimitOrder);
                        position.IsSellOrderPlaced = true;
                        position.InspectSpotPosition();

                        continue;
                    }
                }

                else if (position.IsBought && !position.IsSellOrderPlaced)
                {
                    newLimitOrder = PlaceNewLimitOrder(user, position.Symbol, Sides.SELL, position.Amount, position.SellingPrice);
                    UpdateOrderStatusAndId(position, newLimitOrder);
                    position.IsSellOrderPlaced = true;
                    position.InspectSpotPosition();
                }

                if (position.IsBought && position.IsSellOrderPlaced)
                {
                    position.Status = trader.GetOrderStatus(user, position, position.Symbol);
                    if (position.Status == "FILLED")
                    {
                        if (increaseAmount == true)
                        {
                            IncreaseAmount(position);
                        }

                        position.IsBought = position.IsBuyOrderPlaced = position.IsSellOrderPlaced = false;
                        position.InspectSpotPosition();
                    }
                }
            }
        }
        public void UpdateOrderStatusAndId(SpotPosition position, ILimitOrder newLimitOrder)
        {
            position.OrderId = newLimitOrder.OrderId.ToString();
            position.Status = newLimitOrder.Status.ToString();
        }
        public void IncreaseAmount(SpotPosition position)
        {
            decimal newDollarAmount = (((position.Amount * position.SellingPrice) - (position.BuyingPrice * position.Amount)) * .99M) + (position.BuyingPrice * position.Amount);  

            position.Amount = newDollarAmount / position.BuyingPrice;
            string middleValueInt = position.Amount.ToString();
            string middleValueDecimal = middleValueInt.Split(',')[1].Substring(0, 1);
            middleValueInt = middleValueInt.Split(',')[0];
            position.Amount = Convert.ToDecimal((middleValueInt + "," + middleValueDecimal));
        }
        public ILimitOrder GetOrderInfo(IExchangeUser user, string orderId, string symbol)
        {
            BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/order?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&orderId={orderId}&recvWindow=10000&timestamp=" + binanceMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return BinanceLimitOrder.ConvertToLimitOrder(BinanceLimitOrderDeserialization.DeserializeLimitOrder(response));
        }
        public string GetOrderStatus(IExchangeUser user, SpotPosition position, string symbol)
        {
            BinanceTrader trader = new BinanceTrader();
            return trader.GetOrderInfo(user, position.OrderId, symbol).Status;
        }
        public List<ILimitOrder> GetOpenOrders(IExchangeUser user, string symbol) 
        {
            BinanceMarketInfo bitrueMarketInfo = new BinanceMarketInfo();
            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/openOrders?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&recvWindow=5000&timestamp=" + bitrueMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HTTPrequest.Method = "GET";
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            List<BinanceLimitOrderDeserialization> rawOrders = BinanceLimitOrderDeserialization.DeserializeLimitOrders(response);
            List<ILimitOrder> orders = new List<ILimitOrder>();

            foreach (var rawOrder in rawOrders)
            {
                orders.Add(BinanceLimitOrder.ConvertToLimitOrder(rawOrder));
            }

            return orders;
        }
        public ILimitOrder CancelLimitOrder(IExchangeUser user, string symbol, string orderId)
        {
            BinanceMarketInfo bitrueMarketInfo = new BinanceMarketInfo();
            ILimitOrder order = GetOrderInfo(user, orderId, symbol);

            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/order?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&orderId={orderId}&timestamp=" + bitrueMarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HTTPrequest.Method = "DELETE";
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            order.Status = "CANCELED";

            return order;
        }
    }
}
