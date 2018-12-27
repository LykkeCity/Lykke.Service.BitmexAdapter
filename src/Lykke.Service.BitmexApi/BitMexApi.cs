using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lykke.Service.BitmexApi
{
    public interface IBitMexApi
    {
        Task<List<OrderBook>> GetOrderBookAsync(string symbol, int depth);

        /// <summary>
        /// Create limit order. Order execute by price.
        /// </summary>
        /// <param name="symbol">The contract (instrument)</param>
        /// <param name="side">Order side - Buy or Sell</param>
        /// <param name="price">Execute price</param>
        /// <param name="quantity">Order Volume in QUOTE asset</param>
        /// <param name="doNotInitiate">Orden cannot open new position, pnly close or partition change</param>
        /// <param name="notDisplayQuantity">Shows the order as hidden, keeps us from moving price away from our own orders</param>
        /// <param name="clientOrderId"> Optional Client Order ID. This clOrdID will come back on the order and any related executions.</param>
        /// <returns></returns>
        Task<OrderResult> LimitOrderAsync(string symbol, OrderSide side, decimal price, int quantity, 
            bool doNotInitiate = false, bool notDisplayQuantity = true, string clientOrderId = null);

        /// <summary>
        /// Create market order. Order execute imidiatle by current price.
        /// </summary>
        /// <param name="symbol">The contract (instrument)</param>
        /// <param name="side">Order side - Buy or Sell</param>
        /// <param name="quantity">Order Volume in QUOTE asset</param>
        /// <returns></returns>
        Task<OrderResult> MarketOrderAsync(string symbol, OrderSide side, int quantity);

        /// <summary>
        /// Create order. General method with all possible params.
        /// </summary>
        /// <param name="symbol">Instrument symbol.e.g. 'XBTUSD'.</param>
        /// <param name="side">Order side.Valid options: Buy, Sell.Defaults to 'Buy' unless orderQty is negative.</param>
        /// <param name="orderQty">Order quantity in units of the instrument (i.e.contracts).</param>
        /// <param name="price">Optional limit price for 'Limit', 'StopLimit', and 'LimitIfTouched' orders.</param>
        /// <param name="simpleOrderQty">Deprecated: simple orders are not supported after 2018/10/26</param>
        /// <param name="displayQty">Optional quantity to display in the book. Use 0 for a fully hidden order.</param>
        /// <param name="stopPx">Optional trigger price for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders.Use a price below the current price for stop-sell orders and buy-if-touched orders. Use execInst of 'MarkPrice' or 'LastPrice' to define the current price used for triggering.</param>
        /// <param name="clOrdID"> Optional Client Order ID. This clOrdID will come back on the order and any related executions.</param>
        /// <param name="clOrdLinkID">Deprecated: linked orders are not supported after 2018/11/10.</param>
        /// <param name="pegOffsetValue">Optional trailing offset from the current price for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders; use a negative offset for stop-sell orders and buy-if-touched orders.Optional offset from the peg price for 'Pegged' orders.</param>
        /// <param name="pegPriceType">Optional peg price type. Valid options: LastPeg, MidPricePeg, MarketPeg, PrimaryPeg, TrailingStopPeg.</param>
        /// <param name="ordType">Order type.Valid options: Market, Limit, Stop, StopLimit, MarketIfTouched, LimitIfTouched, MarketWithLeftOverAsLimit, Pegged.Defaults to 'Limit' when price is specified.Defaults to 'Stop' when stopPx is specified.Defaults to 'StopLimit' when price and stopPx are specified.</param>
        /// <param name="timeInForce">Time in force.Valid options: Day, GoodTillCancel, ImmediateOrCancel, FillOrKill.Defaults to 'GoodTillCancel' for 'Limit', 'StopLimit', 'LimitIfTouched', and 'MarketWithLeftOverAsLimit' orders.</param>
        /// <param name="execInst">Optional execution instructions.Valid options: ParticipateDoNotInitiate, AllOrNone, MarkPrice, IndexPrice, LastPrice, Close, ReduceOnly, Fixed. 'AllOrNone' instruction requires displayQty to be 0. 'MarkPrice', 'IndexPrice' or 'LastPrice' instruction valid for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders.</param>
        /// <param name="contingencyType">Deprecated: linked orders are not supported after 2018/11/10.</param>
        /// <param name="text">Optional order annotation.e.g. 'Take profit'.</param>
        /// <returns></returns>
        Task<OrderResult> CreateOrderAsync(
            string symbol,
            decimal orderQty,
            OrderSide side,
            decimal? price = null,
            string simpleOrderQty = null,
            decimal? displayQty = null,
            decimal? stopPx = null,
            string clOrdID = null,
            string clOrdLinkID = null,
            decimal? pegOffsetValue = null,
            string pegPriceType = null,
            string ordType = null,
            string timeInForce = null,
            string execInst = null,
            string contingencyType = null,
            string text = null
        );

        Task<string> CancelAllOpenOrdersAsync(string symbol, string note = "cancel all orders by api");
        Task<string> CancelOrderAsync(string orderId, string test = "cancel order by ID");
        Task<List<Instrument>> GetActiveInstrumentsAsync();
        Task<List<Instrument>> GetInstrumentAsync(string symbol);
        Task<List<Candle>> GetCandleHistoryAsync(string symbol, int count, string size);

        /// <summary>
        /// Get only open positions
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<List<Position>> GetOpenPositionsAsync(string symbol);

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>
        Task<List<Position>> GetPositionsAsync();

        /// <summary>
        /// Get only active orders
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<List<Order>> GetOpenOrdersAsync(string symbol);

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <param name="reverse"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<string> GetOrdersAsync(string symbol, int? count = null, int? start = null, bool reverse = true,
            DateTime? startTime = null, DateTime? endTime = null);

        Task<string> EditOrderPriceAsync(string orderId, double price);
    }

    public class BitMexApi : IBitMexApi
    {
        // "https://testnet.bitmex.com"
        private readonly string _domain;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly int _rateLimit;
        private readonly long _beginTicks;

        public BitMexApi(string bitmexKey = "", string bitmexSecret = "", string bitmexDomain = "", int rateLimit = 5000)
        {
            _apiKey = bitmexKey;
            _apiSecret = bitmexSecret;
            _rateLimit = rateLimit;
            _domain = bitmexDomain;

            _beginTicks = new DateTime(2018, 1, 1).Ticks;
        }

        #region API Connector

        private string BuildQueryData(Dictionary<string, string> param)
        {
            if (param == null)
                return "";

            StringBuilder b = new StringBuilder();
            foreach (var item in param)
                b.Append($"&{item.Key}={WebUtility.UrlEncode(item.Value)}");

            try
            {
                return b.ToString().Substring(1);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string BuildJSON(Dictionary<string, string> param)
        {
            if (param == null)
                return "";

            var json = JsonConvert.SerializeObject(param);
            return json;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hexs = BitConverter.ToString(ba);
            hexs = hexs.Replace("-", "").ToLower();
            return hexs;
        }

        private long GetNonce() => DateTime.UtcNow.Ticks - _beginTicks;

        private byte[] hmacsha256(byte[] keyByte, byte[] messageBytes)
        {
            using (var hash = new HMACSHA256(keyByte))
            {
                return hash.ComputeHash(messageBytes);
            }
        }

        private async Task<string> Query(string method, string function, Dictionary<string, string> param = null, bool auth = false, bool json = false)
        {
            string paramData = json ? BuildJSON(param) : BuildQueryData(param);
            string url = "/api/v1" + function + ((method == "GET" && paramData != "") ? "?" + paramData : "");
            string postData = (method != "GET") ? paramData : "";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_domain + url);
            webRequest.Method = method;

            if (auth)
            {
                string nonce = GetNonce().ToString();
                string message = method + url + nonce + postData;
                byte[] signatureBytes = hmacsha256(Encoding.UTF8.GetBytes(_apiSecret), Encoding.UTF8.GetBytes(message));
                string signatureString = ByteArrayToString(signatureBytes);

                webRequest.Headers.Add("api-nonce", nonce);
                webRequest.Headers.Add("api-key", _apiKey);
                webRequest.Headers.Add("api-signature", signatureString);
            }

            try
            {
                if (postData != "")
                {
                    webRequest.ContentType = json ? "application/json" : "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(postData);
                    using (var stream = webRequest.GetRequestStream())
                    {
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }

                using (WebResponse webResponse = await webRequest.GetResponseAsync())
                using (Stream str = webResponse.GetResponseStream())
                using (StreamReader sr = new StreamReader(str))
                {
                    var result = await sr.ReadToEndAsync();
                    return result;
                }
            }
            catch (WebException wex)
            {
                using (HttpWebResponse response = (HttpWebResponse)wex.Response)
                {
                    if (response == null)
                        throw;

                    using (Stream str = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(str))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

        #region Our Calls
        public async Task<List<OrderBook>> GetOrderBookAsync(string symbol, int depth)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["depth"] = depth.ToString();
            string res = await Query("GET", "/orderBook/L2", param);
            return JsonConvert.DeserializeObject<List<OrderBook>>(res);
        }

        /// <summary>
        /// Create limit order. Order execute by price.
        /// </summary>
        /// <param name="symbol">The contract (instrument)</param>
        /// <param name="side">Order side - Buy or Sell</param>
        /// <param name="price">Execute price</param>
        /// <param name="quantity">Order Volume in QUOTE asset</param>
        /// <param name="doNotInitiate">Orden cannot open new position, pnly close or partition change</param>
        /// <param name="notDisplayQuantity">Shows the order as hidden, keeps us from moving price away from our own orders</param>
        /// <param name="clientOrderId"> Optional Client Order ID. This clOrdID will come back on the order and any related executions.</param>
        /// <returns></returns>
        public async Task<OrderResult> LimitOrderAsync(string symbol, OrderSide side, decimal price, int quantity, 
            bool doNotInitiate = false, bool notDisplayQuantity = true, string clientOrderId = null)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["side"] = side.ToString();
            param["orderQty"] = quantity.ToString();
            param["ordType"] = OrderType.Limit.ToString();
            if (doNotInitiate)  param["execInst"] = ExecutionInstructions.ParticipateDoNotInitiate.ToString();
            if (notDisplayQuantity) param["displayQty"] = 0.ToString();
            param["price"] = price.ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(clientOrderId)) param["clOrdId"] = clientOrderId;

            var responce = await Query("POST", "/order", param, true);
            return JsonConvert.DeserializeObject<OrderResult>(responce);
        }

        /// <summary>
        /// Create market order. Order execute imidiatle by current price.
        /// </summary>
        /// <param name="symbol">The contract (instrument)</param>
        /// <param name="side">Order side - Buy or Sell</param>
        /// <param name="quantity">Order Volume in QUOTE asset</param>
        /// <returns></returns>
        public async Task<OrderResult> MarketOrderAsync(string symbol, OrderSide side, int quantity)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["side"] = side.ToString();
            param["orderQty"] = quantity.ToString();
            param["ordType"] = OrderType.Market.ToString();
            var responce = await Query("POST", "/order", param, true);
            return JsonConvert.DeserializeObject<OrderResult>(responce);
        }

        /// <summary>
        /// Create order. General method with all possible params.
        /// </summary>
        /// <param name="symbol">Instrument symbol.e.g. 'XBTUSD'.</param>
        /// <param name="side">Order side.Valid options: Buy, Sell.Defaults to 'Buy' unless orderQty is negative.</param>
        /// <param name="orderQty">Order quantity in units of the instrument (i.e.contracts).</param>
        /// <param name="price">Optional limit price for 'Limit', 'StopLimit', and 'LimitIfTouched' orders.</param>
        /// <param name="simpleOrderQty">Deprecated: simple orders are not supported after 2018/10/26</param>
        /// <param name="displayQty">Optional quantity to display in the book. Use 0 for a fully hidden order.</param>
        /// <param name="stopPx">Optional trigger price for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders.Use a price below the current price for stop-sell orders and buy-if-touched orders. Use execInst of 'MarkPrice' or 'LastPrice' to define the current price used for triggering.</param>
        /// <param name="clOrdID"> Optional Client Order ID. This clOrdID will come back on the order and any related executions.</param>
        /// <param name="clOrdLinkID">Deprecated: linked orders are not supported after 2018/11/10.</param>
        /// <param name="pegOffsetValue">Optional trailing offset from the current price for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders; use a negative offset for stop-sell orders and buy-if-touched orders.Optional offset from the peg price for 'Pegged' orders.</param>
        /// <param name="pegPriceType">Optional peg price type. Valid options: LastPeg, MidPricePeg, MarketPeg, PrimaryPeg, TrailingStopPeg.</param>
        /// <param name="ordType">Order type.Valid options: Market, Limit, Stop, StopLimit, MarketIfTouched, LimitIfTouched, MarketWithLeftOverAsLimit, Pegged.Defaults to 'Limit' when price is specified.Defaults to 'Stop' when stopPx is specified.Defaults to 'StopLimit' when price and stopPx are specified.</param>
        /// <param name="timeInForce">Time in force.Valid options: Day, GoodTillCancel, ImmediateOrCancel, FillOrKill.Defaults to 'GoodTillCancel' for 'Limit', 'StopLimit', 'LimitIfTouched', and 'MarketWithLeftOverAsLimit' orders.</param>
        /// <param name="execInst">Optional execution instructions.Valid options: ParticipateDoNotInitiate, AllOrNone, MarkPrice, IndexPrice, LastPrice, Close, ReduceOnly, Fixed. 'AllOrNone' instruction requires displayQty to be 0. 'MarkPrice', 'IndexPrice' or 'LastPrice' instruction valid for 'Stop', 'StopLimit', 'MarketIfTouched', and 'LimitIfTouched' orders.</param>
        /// <param name="contingencyType">Deprecated: linked orders are not supported after 2018/11/10.</param>
        /// <param name="text">Optional order annotation.e.g. 'Take profit'.</param>
        /// <returns></returns>
        public async Task<OrderResult> CreateOrderAsync(
            string symbol,
            decimal orderQty,
            OrderSide side,
            decimal? price = null,
            string simpleOrderQty = null,
            decimal? displayQty = null,
            decimal? stopPx = null,
            string clOrdID = null,
            string clOrdLinkID = null,
            decimal? pegOffsetValue = null,
            string pegPriceType = null,
            string ordType = null,
            string timeInForce = null,
            string execInst = null,
            string contingencyType = null,
            string text = null
            )
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["side"] = side.ToString();
            param["orderQty"] = orderQty.ToString(CultureInfo.InvariantCulture);

            if (price != null) param["price"] = price.ToString();
            if (simpleOrderQty != null) param["simpleOrderQty"] = price.ToString();
            if (displayQty != null) param["displayQty"] = price.ToString();
            if (stopPx != null) param["stopPx"] = price.ToString();
            if (clOrdID != null) param["clOrdID"] = price.ToString();
            if (clOrdLinkID != null) param["clOrdLinkID"] = price.ToString();
            if (pegOffsetValue != null) param["pegOffsetValue"] = price.ToString();
            if (pegPriceType != null) param["pegPriceType"] = price.ToString();
            if (ordType != null) param["ordType"] = price.ToString();
            if (timeInForce != null) param["timeInForce"] = price.ToString();
            if (execInst != null) param["execInst"] = price.ToString();
            if (contingencyType != null) param["contingencyType"] = price.ToString();
            if (text != null) param["text"] = price.ToString();
            var responce = await Query("POST", "/order", param, true);
            return JsonConvert.DeserializeObject<OrderResult>(responce);
        }


        public async Task<string> CancelAllOpenOrdersAsync(string symbol, string note = "cancel all orders by api")
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["text"] = note;
            return await Query("DELETE", "/order/all", param, true, true);
        }

        public async Task<string> CancelOrderAsync(string orderId, string test = "cancel order by ID")
        {
            var param = new Dictionary<string, string>();
            param["orderID"] = orderId;
            param["text"] = test;
            return await Query("DELETE", "/order", param, true, true);
        }

        public async Task<List<Instrument>> GetActiveInstrumentsAsync()
        {
            string res = await Query("GET", "/instrument/active");
            return JsonConvert.DeserializeObject<List<Instrument>>(res);
        }

        public async Task<List<Instrument>> GetInstrumentAsync(string symbol)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            string res = await Query("GET", "/instrument", param);
            return JsonConvert.DeserializeObject<List<Instrument>>(res);
        }

        public async Task<List<Candle>> GetCandleHistoryAsync(string symbol, int count, string size)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["count"] = count.ToString();
            param["reverse"] = true.ToString();
            param["partial"] = false.ToString();
            param["binSize"] = size;
            string res = await Query("GET", "/trade/bucketed", param);
            return JsonConvert.DeserializeObject<List<Candle>>(res).OrderByDescending(a => a.TimeStamp).ToList();
        }

        /// <summary>
        /// Get only open positions
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<List<Position>> GetOpenPositionsAsync(string symbol)
        {
            var param = new Dictionary<string, string>();
            string res = await Query("GET", "/position", param, true);
            return JsonConvert.DeserializeObject<List<Position>>(res).Where(a => a.Symbol == symbol && a.IsOpen == true).OrderByDescending(a => a.TimeStamp).ToList();
        }

        /// <summary>
        /// Get all positions
        /// </summary>
        /// <returns></returns>
        public async Task<List<Position>> GetPositionsAsync()
        {
            var param = new Dictionary<string, string>();
            string res = await Query("GET", "/position", param, true);
            return JsonConvert.DeserializeObject<List<Position>>(res).OrderByDescending(a => a.TimeStamp).ToList();
        }

        /// <summary>
        /// Get only active orders
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOpenOrdersAsync(string symbol)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["reverse"] = true.ToString();
            string res = await Query("GET", "/order", param, true);
            return JsonConvert.DeserializeObject<List<Order>>(res).Where(a => a.OrdStatus == "New" || a.OrdStatus == "PartiallyFilled").OrderByDescending(a => a.TimeStamp).ToList();
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <param name="reverse"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<string> GetOrdersAsync(string symbol, int? count = null, int? start = null, bool reverse = true,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            //param["filter"] = "{\"open\":true}";
            //param["columns"] = "";
            if (count.HasValue) param["count"] = count.ToString();
            if (start.HasValue) param["start"] = start.ToString();
            param["reverse"] = reverse.ToString();
            if (startTime.HasValue) param["startTime"] = startTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (endTime.HasValue) param["endTime"] = endTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            return await Query("GET", "/order", param, true);
        }

        public async Task<string> EditOrderPriceAsync(string orderId, double price)
        {
            var param = new Dictionary<string, string>();
            param["orderID"] = orderId;
            param["price"] = price.ToString(CultureInfo.InvariantCulture);
            return await Query("PUT", "/order", param, true, true);
        }


        #endregion



        #region RateLimiter

        private long lastTicks = 0;
        private object thisLock = new object();

        private void RateLimit()
        {
            lock (thisLock)
            {
                long elapsedTicks = DateTime.Now.Ticks - lastTicks;
                var timespan = new TimeSpan(elapsedTicks);
                if (timespan.TotalMilliseconds < _rateLimit)
                    Thread.Sleep(_rateLimit - (int)timespan.TotalMilliseconds);
                lastTicks = DateTime.Now.Ticks;
            }
        }

        #endregion RateLimiter
    }
}
