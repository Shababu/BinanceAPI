# Binance API

The following library consists of classes that allows you to:
+ Get market information from [Binance](https://www.binance.com/).
+ Place limit/market orders
+ Trade automatically 

BinanceAPI library was uploaded to [NuGet](https://www.nuget.org/packages/BinanceAPI/).

## BinanceAccountInfo 📝
**BinanceAccountInfo** class allows user to fetch information about filled orders.

### Example:

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceAccountInfo binanceAccountInfo = new BinanceAccountInfo();

List<IFilledTrade> trades = binanceAccountInfo.GetTrades(user, "XRPUSDT");
foreach (var trade in trades)
{
    Console.WriteLine(trade.ToString() + "\n");
}
~~~

__Result:__

~~~
Order Id: 395764946
Symbol: XRPUSDT
Price: 0,81570000
Qty: 22,00000000
QuoteQty: 17,94540000
Commission: 0,01794540
TimeStamp: 16.12.2021 22:10:32
IsBuyer: False
IsMaker: True
Side: SELL
~~~
----
## BinanceWalletInfo 💼
**BinanceWalletInfo** class allows user to fetch information about current balances on account.

### Example:

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceWalletInfo binanceWalletInfo = new BinanceWalletInfo();
List<ICryptoBalance> balances = binanceWalletInfo.GetWalletInfo(user);
decimal totalInRub = binanceWalletInfo.GetAccountTotalBalance(balances);

foreach (var balance in balances)
{
    Console.WriteLine(balance);
}

Console.WriteLine($"Total balance in rub: {totalInRub}");
~~~

__Result:__

~~~
Asset BNB
Total: 0,00000008
Free: 0,00000008
Locked: 0,00000000
RubValue: 0,002776486800000000000000

Asset XRP
Total: 0,30038300
Free: 0,30038300
Locked: 0,00000000
RubValue: 17,950317652683000000000000

Asset RUB
Total: 54,46000000
Free: 54,46000000
Locked: 0,00000000
RubValue: 54,46000000

Asset SHIB
Total: 4062137,15
Free: 4062137,15
Locked: 0,00
RubValue: 8792,172921271575000000

Asset SGB
Total: 18,36496913
Free: 18,36496913
Locked: 0,00000000
RubValue: 0

Total balance in rub: 8864,586015411058000000000000
~~~
----
## BinanceExchangeInfo 📖
**BinanceExchangeInfo** class allows user to fetch information about trading pairs listed on Binance.

### Example:

~~~C#
IExchangeInfo exchangeInfo = new BinanceExchangeInfo();
exchangeInfo = exchangeInfo.GetExchangeInfo();

foreach(var symbol in exchangeInfo.ExchangeSymbolsInfo)
{
    Console.WriteLine(symbol);
}
~~~

__Result:__

~~~
Symbol: ELFBTC
Base Asset: ELF
Quote Asset: BTC
Quote Precision: 8

Symbol: ELFETH
Base Asset: ELF
Quote Asset: ETH
Quote Precision: 8

...
~~~
----
## BinanceMarketInfo 📈
**BinanceMarketInfo** class allows user to:

+ Get current price of a specific asset
+ Get current prices of all assets listed on Binance
+ Get full information about specific asset 
+ Get full information about all assets listed on Binance
+ Get a set of chart candlesticks
+ Get information about orderbook

### Example 1 (Get current price of a specific asset):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
Console.WriteLine(binanceMarketInfo.GetPrice("BTCUSDT"));
~~~

__Result:__

~~~
BTC Price: 40163,67000000
~~~

### Example 2 (Get current prices of all assets listed on Binance):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
List<Cryptocurrency> allSymbols = binanceMarketInfo.GetPrice();

foreach(var symbol in allSymbols)
{
    Console.WriteLine(symbol);
}
~~~

__Result:__

~~~
Symbol: ETHBTC
Price: 0,07567700

Symbol: LTCBTC
Price: 0,00260700

...
~~~

### Example 3 (Get full information about specific asset):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
IAssetStatus stats = binanceMarketInfo.Get24HourStatOnAsset("BTCUSDT");

Console.WriteLine(stats);
~~~

__Result:__

~~~
Symbol: BTCUSDT
PriceChange: -786,95000000
PriceChangePercent: -1,923
WeightedAvgPrice: 40096,44916295
PrevClosePrice: 40912,69000000
LastPrice: 40125,74000000
LastQty: 0,05000000
OpenPrice: 40912,69000000
HighPrice: 40989,99000000
LowPrice: 39200,00000000
Volume: 65796,91645000
QuoteVolume: 2638222715,51628790
~~~

### Example 4 (Get full information about all assets listed on Binance):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
List<IAssetStatus> assets = binanceMarketInfo.Get24HourStatOnAllAssets();

foreach (var asset in assets)
{
    Console.WriteLine(asset);
}
~~~

__Result:__

~~~
Symbol: FISBUSD
PriceChange: 0,03620000
PriceChangePercent: 5,701
WeightedAvgPrice: 0,69751975
PrevClosePrice: 0,63230000
LastPrice: 0,67120000
LastQty: 489,00000000
OpenPrice: 0,63500000
HighPrice: 0,73500000
LowPrice: 0,63130000
Volume: 1971995,00000000
QuoteVolume: 1375505,45030000

Symbol: FISUSDT
PriceChange: 0,03700000
PriceChangePercent: 5,834
WeightedAvgPrice: 0,69056892
PrevClosePrice: 0,63370000
LastPrice: 0,67120000
LastQty: 631,00000000
OpenPrice: 0,63420000
HighPrice: 0,73500000
LowPrice: 0,62850000
Volume: 11241680,00000000
QuoteVolume: 7763154,77820000

...
~~~

### Example 5 (Get a set of chart candlesticks):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
List<ICandle> candles = binanceMarketInfo.GetCandles("XRPUSDT", "5m", 10);

foreach (var candle in candles)
{
    Console.WriteLine(candle);
}
~~~

__Result:__

~~~
Open Time: 12.04.2022 16:40:00, Open: 0,7156, High: 0,7168, Low: 0,714, Close: 0,7165, Volume: 1270190

Open Time: 12.04.2022 16:45:00, Open: 0,7166, High: 0,7182, Low: 0,7157, Close: 0,7166, Volume: 919646

Open Time: 12.04.2022 16:50:00, Open: 0,7166, High: 0,7175, Low: 0,7163, Close: 0,7166, Volume: 900217

Open Time: 12.04.2022 16:55:00, Open: 0,7165, High: 0,717, Low: 0,7145, Close: 0,7156, Volume: 2132952

Open Time: 12.04.2022 17:00:00, Open: 0,7157, High: 0,716, Low: 0,713, Close: 0,7139, Volume: 725217

Open Time: 12.04.2022 17:05:00, Open: 0,7138, High: 0,7148, Low: 0,713, Close: 0,7132, Volume: 1621247

Open Time: 12.04.2022 17:10:00, Open: 0,7133, High: 0,7139, Low: 0,7112, Close: 0,7118, Volume: 1595582

Open Time: 12.04.2022 17:15:00, Open: 0,7119, High: 0,7122, Low: 0,709, Close: 0,7095, Volume: 1581439

Open Time: 12.04.2022 17:20:00, Open: 0,7095, High: 0,7113, Low: 0,7066, Close: 0,7068, Volume: 1922451

Open Time: 12.04.2022 17:25:00, Open: 0,7068, High: 0,7082, Low: 0,7054, Close: 0,7056, Volume: 874951
~~~

### Example 6 (Get information about orderbook):

~~~C#
BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
List<IDepth> binanceDepth = binanceMarketInfo.GetDepth("BTCUSDT", 10);

foreach (var item in binanceDepth)
{
    Console.WriteLine(item);
}
~~~

__Result:__

~~~
bid Price: 39903,87000000, Quantity: 2,85331000
bid Price: 39903,32000000, Quantity: 0,00030000
bid Price: 39901,94000000, Quantity: 0,80000000
bid Price: 39901,33000000, Quantity: 0,00935000
bid Price: 39899,73000000, Quantity: 0,00551000
bid Price: 39899,72000000, Quantity: 0,18795000
bid Price: 39899,71000000, Quantity: 0,13378000
bid Price: 39899,51000000, Quantity: 0,00620000
bid Price: 39899,31000000, Quantity: 0,06000000
ask Price: 39903,88000000, Quantity: 0,78974000
ask Price: 39905,95000000, Quantity: 0,33832000
ask Price: 39905,96000000, Quantity: 0,02677000
ask Price: 39907,40000000, Quantity: 0,13124000
ask Price: 39907,50000000, Quantity: 0,01326000
ask Price: 39908,20000000, Quantity: 0,00620000
ask Price: 39909,32000000, Quantity: 0,28109000
ask Price: 39909,87000000, Quantity: 0,33957000
ask Price: 39909,96000000, Quantity: 0,01049000
~~~
----

## BinanceTrader 💲
**BinanceTrader** class allows user to:
+ Place limit orders
+ Get information about existing limit order
+ Cancel limit order
+ Place market orders
+ Trade automatically

### Example 1 (Place limit order):

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceTrader binanceTrader = new BinanceTrader();
ILimitOrder limitOrder = binanceTrader.PlaceNewLimitOrder(user, "SHIBUSDT", Sides.SELL, 3000000, 0.00004M);

Console.WriteLine(limitOrder);
~~~

__Result:__

~~~
Symbol: SHIBUSDT
Order Id: 1297817845
Price: 0,00004000
Quantity: 3000000,00
ExecutedQty: 0,00
Status: NEW
Side: SELL
Time: 12.04.2022 18:26:20
~~~

### Example 2 (Place limit order, Get info about the placed order, Cancel order):

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceTrader binanceTrader = new BinanceTrader();

ILimitOrder limitOrder = binanceTrader.PlaceNewLimitOrder(user, "SHIBUSDT", Sides.SELL, 3000000, 0.00004M);
Console.WriteLine($"New limit order has been placed:\n\n{limitOrder}\n");

limitOrder = binanceTrader.GetOrderInfo(user, limitOrder.OrderId.ToString(), limitOrder.Symbol);
Console.WriteLine($"Order info:\n\n{limitOrder}\n");

limitOrder = binanceTrader.CancelLimitOrder(user, limitOrder.Symbol, limitOrder.OrderId.ToString());
Console.WriteLine($"Limit order has been canceled:\n\n{limitOrder}\n");
~~~

__Result:__

~~~
New limit order has been placed:

Symbol: SHIBUSDT
Order Id: 1298034217
Price: 0,00004000
Quantity: 3000000,00
ExecutedQty: 0,00
Status: NEW
Side: SELL
Time: 12.04.2022 18:54:30


Order info:

Symbol: SHIBUSDT
Order Id: 1298034217
Price: 0,00004000
Quantity: 3000000,00
ExecutedQty: 0,00
Status: NEW
Side: SELL
Time: 12.04.2022 18:54:30


Limit order has been canceled:

Symbol: SHIBUSDT
Order Id: 1298034217
Price: 0,00004000
Quantity: 3000000,00
ExecutedQty: 0,00
Status: CANCELED
Side: SELL
Time: 12.04.2022 18:54:30
~~~

### Example 3 (Place market order):

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceTrader binanceTrader = new BinanceTrader();

ILimitOrder limitOrder = binanceTrader.PlaceNewMarketOrder(user, "SHIBUSDT", Sides.SELL, 410000);
Console.WriteLine($"New limit order has been sent\n\n{limitOrder}\n");
~~~

__Result:__

~~~
Market order has been sent

Symbol: SHIBUSDT
Order Id: 1298065491
Price: 0,00000000
Quantity: 410000,00
ExecutedQty: 410000,00
Status: FILLED
Side: SELL
Time: 12.04.2022 18:57:30
~~~

### Example 4 (Auto traiding):

~~~C#
BinanceApiUser user = new BinanceApiUser("public API key", "private API key");
BinanceTrader binanceTrader = new BinanceTrader();

List<SpotPosition> positions = new List<SpotPosition>()
{
    new SpotPosition("SHIB", "BUSD", 0.000030M, 0.000031M, 1000000, true, true, false),
    new SpotPosition("SHIB", "BUSD", 0.000031M, 0.000032M, 1000000, true, true, false),
    new SpotPosition("SHIB", "BUSD", 0.000032M, 0.000033M, 1000000, true, true, false),
};

foreach (var position in positions)
{
    position.BuyOrderPlaced += BuyOrderPlaced;
    position.BuyOrderExecuted += BuyOrderExecuted;
    position.SellOrderPlaced += SellOrderPlaced;
    position.SellOrderExecuted += SellOrderExecuted;

    Console.WriteLine(position);
}

while (true)
{
    binanceTrader.AutoTrade(user, positions, false);

    foreach (var position in positions)
    {
        Console.WriteLine(position);
    }
}
~~~

~~~C#
public static void BuyOrderPlaced(object sender, EventArgs e)
{
    Console.WriteLine("Buy order placed");
}
public static void BuyOrderExecuted(object sender, EventArgs e)
{
    Console.WriteLine("Buy order executed");
}
public static void SellOrderPlaced(object sender, EventArgs e)
{
    Console.WriteLine("Sell order placed");
}
public static void SellOrderExecuted(object sender, EventArgs e)
{
    Console.WriteLine("Sell order executed");
}
~~~

__Result:__

~~~
Symbol: SHIBBUSD
Order Id:
Status:
Buying Price: 0,000030
Selling Price: 0,000031
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: False
Amount Web: 1000000
Buying Price Web: 0.000030
Selling Price Web: 0.000031

Symbol: SHIBBUSD
Order Id:
Status:
Buying Price: 0,000031
Selling Price: 0,000032
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: False
Amount Web: 1000000
Buying Price Web: 0.000031
Selling Price Web: 0.000032

Symbol: SHIBBUSD
Order Id:
Status:
Buying Price: 0,000032
Selling Price: 0,000033
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: False
Amount Web: 1000000
Buying Price Web: 0.000032
Selling Price Web: 0.000033

Sell order placed
Sell order placed
Sell order placed

Symbol: SHIBBUSD
Order Id: 416050425
Status: NEW
Buying Price: 0,000030
Selling Price: 0,000031
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: True
Amount Web: 1000000
Buying Price Web: 0.000030
Selling Price Web: 0.000031

Symbol: SHIBBUSD
Order Id: 416050664
Status: NEW
Buying Price: 0,000031
Selling Price: 0,000032
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: True
Amount Web: 1000000
Buying Price Web: 0.000031
Selling Price Web: 0.000032

Symbol: SHIBBUSD
Order Id: 416050710
Status: NEW
Buying Price: 0,000032
Selling Price: 0,000033
Amount: 1000000
Distance: 0,000001
Is Bought: True
IsBuyOrderPlaced: True
IsSellOrderPlaced: True
Amount Web: 1000000
Buying Price Web: 0.000032
Selling Price Web: 0.000033
~~~
