﻿namespace Lykke.Service.BitmexApi
{
    /// <summary>
    /// The following execInsts are supported. If using multiple, separate with a comma (e.g. LastPrice,Close).
    /// </summary>
    public enum ExecutionInstructions
    {
        /// <summary>
        /// Also known as a Post-Only order. If this order would have executed on placement, it will cancel instead.
        /// </summary>
        ParticipateDoNotInitiate,

        /// <summary>
        /// Used by stop and if-touched orders to determine the triggering price. Use only one.
        /// By default, 'MarkPrice' is used. Also used for Pegged orders to define the value of 'LastPeg'.
        /// </summary>
        MarkPrice,

        /// <summary>
        /// Used by stop and if-touched orders to determine the triggering price. Use only one.
        /// By default, 'MarkPrice' is used. Also used for Pegged orders to define the value of 'LastPeg'.
        /// </summary>
        LastPrice,

        /// <summary>
        /// Used by stop and if-touched orders to determine the triggering price. Use only one.
        /// By default, 'MarkPrice' is used. Also used for Pegged orders to define the value of 'LastPeg'.
        /// </summary>
        IndexPrice,

        /// <summary>
        /// A 'ReduceOnly' order can only reduce your position, not increase it.
        /// If you have a 'ReduceOnly' limit order that rests in the order book while the position is reduced by other orders,
        /// then its order quantity will be amended down or canceled. If there are multiple 'ReduceOnly'
        /// orders the least aggressive will be amended first.
        /// </summary>
        ReduceOnly,

        /// <summary>
        /// 'Close' implies 'ReduceOnly'. A 'Close' order will cancel other active limit orders with the same side and symbol
        /// if the open quantity exceeds the current position. This is useful for stops: by canceling these orders,
        /// a 'Close' Stop is ensured to have the margin required to execute,
        /// and can only execute up to the full size of your position. If orderQty is not specified,
        /// a 'Close' order has an orderQty equal to your current position's size.
        /// 
        ///  * Note that a Close order without an orderQty requires a side,
        ///    so that BitMEX knows if it should trigger above or below the stopPx.
        /// </summary>
        Close
    }
}
