namespace Lykke.Service.BitmexApi
{
    public enum OrderType
    {
        /// <summary>
        /// The default order type. Specify an orderQty and price.
        /// </summary>
        Limit,

        /// <summary>
        /// A traditional Market order. A Market order will execute until filled or your bankruptcy price is reached,
        /// at which point it will cancel.
        /// </summary>
        Market,

        /// <summary>
        /// A market order that, after eating through the order book as far as permitted by available margin,
        /// will become a limit order. The difference between this type and Market only affects the behavior in thin books.
        /// Upon reaching the deepest possible price, if there is quantity left over,
        /// a Market order will cancel the remaining quantity. MarketWithLeftOverAsLimit will keep the remaining quantity in
        /// the books as a Limit.
        /// </summary>
        MarketWithLeftOverAsLimit,

        /// <summary>
        /// A Stop Market order. Specify an orderQty and stopPx. When the stopPx is reached, the order will be entered into the book.
        ///  * On sell orders, the order will trigger if the triggering price is lower than the stopPx. On buys, higher.
        ///  * Note: Stop orders do not consume margin until triggered. Be sure that the required margin is available in your account so that it may trigger fully.
        ///  * Close Stops don't require an orderQty. See Execution Instructions below.
        /// </summary>
        Stop,

        /// <summary>
        /// Like a Stop Market, but enters a Limit order instead of a Market order. Specify an orderQty, stopPx, and price.
        /// </summary>
        StopLimit,

        /// <summary>
        /// Similar to a Stop, but triggers are done in the opposite direction. Useful for Take Profit orders.
        /// </summary>
        MarketIfTouched,

        /// <summary>
        /// As above; use for Take Profit Limit orders.
        /// </summary>
        LimitIfTouched
    }
}
