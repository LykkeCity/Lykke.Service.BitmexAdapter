using System;

namespace Lykke.Service.BitmexApi
{
    public class Position
    {
        /// <summary>
        /// Date and time of generate this data snapshot.
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// 1 / initMarginReq.
        /// </summary>
        public double? Leverage { get; set; }
        /// <summary>
        /// The current position amount in contracts.
        /// </summary>
        public int? CurrentQty { get; set; }
        /// <summary>
        /// The current cost of the position in the settlement currency of the symbol (currency).
        /// </summary>
        public double? CurrentCost { get; set; }
        /// <summary>
        /// Position still is open.
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// The mark price of the symbol in quoteCurrency.
        /// </summary>
        /// <remarks>Current Price by symbol.</remarks>
        public double? MarkPrice { get; set; }
        /// <summary>
        /// The currentQty at the mark price in the settlement currency of the symbol (currency).
        /// </summary>
        public double? MarkValue { get; set; }
        /// <summary>
        /// unrealisedGrossPnl.
        /// </summary>
        public double? UnrealisedPnl { get; set; }
        /// <summary>
        /// unrealisedGrossPnl in %.
        /// </summary>
        public double? UnrealisedPnlPcnt { get; set; }

        /// <summary>
        /// Avg open price of current position.
        /// </summary>
        public double? AvgEntryPrice { get; set; }

        public double? BreakEvenPrice { get; set; }

        /// <summary>
        /// Once markPrice reaches this price, this position will be liquidated.
        /// </summary>
        public double? LiquidationPrice { get; set; }
        /// <summary>
        /// The contract for this position.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Your unique account ID
        /// </summary>
        public int Account { get; set; }
        /// <summary>
        /// The margin currency for this position.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Meta data of the symbol.
        /// </summary>
        public string Underlying { get; set; }
        /// <summary>
        /// Meta data of the symbol, All prices are in the quoteCurrency
        /// </summary>
        public string QuoteCurrency { get; set; }
        /// <summary>
        /// The maximum of the maker, taker, and settlement fee.
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// The initial margin requirement. This will be at least the symbol's default initial maintenance margin, but can be higher if you choose lower leverage.
        /// </summary>
        public decimal InitMarginReq { get; set; }
        /// <summary>
        /// The maintenance margin requirement. This will be at least the symbol's default maintenance maintenance margin, but can be higher if you choose a higher risk limit.
        /// </summary>
        public decimal MaintMarginReq { get; set; }
        /// <summary>
        /// This is a function of your maintMarginReq.
        /// </summary>
        public decimal RiskLimit { get; set; }
        /// <summary>
        /// True/false depending on whether you set cross margin on this position.
        /// </summary>
        public bool CrossMargin { get; set; }
        /// <summary>
        ///  Indicates where your position is in the ADL queue.
        /// </summary>
        public decimal? DeleveragePercentile { get; set; }
        /// <summary>
        /// The value of realised PNL that has transferred to your wallet for this position.
        /// </summary>
        public decimal RebalancedPnl { get; set; }
        /// <summary>
        /// The value of realised PNL that has transferred to your wallet for this position since the position was closed.
        /// </summary>
        public decimal PrevRealisedPnl { get; set; }

        public decimal PrevUnrealisedPnl { get; set; }
        public decimal PrevClosePrice { get; set; }
        public DateTime OpeningTimestamp { get; set; }
        public decimal OpeningQty { get; set; }
        public decimal OpeningCost { get; set; }
        public decimal OpeningComm { get; set; }
        public decimal OpenOrderBuyQty { get; set; }
        public decimal OpenOrderBuyCost { get; set; }
        public decimal OpenOrderBuyPremium { get; set; }
        public decimal OpenOrderSellQty { get; set; }
        public decimal OpenOrderSellCost { get; set; }
        public decimal OpenOrderSellPremium { get; set; }
        public decimal ExecBuyQty { get; set; }
        public decimal ExecBuyCost { get; set; }
        public decimal ExecSellQty { get; set; }
        public decimal ExecSellCost { get; set; }
        public decimal ExecQty { get; set; }
        public decimal ExecCost { get; set; }
        public decimal ExecComm { get; set; }
        public DateTime CurrentTimestamp { get; set; }
        /// <summary>
        /// The current commission of the position in the settlement currency of the symbol (currency).
        /// </summary>
        public decimal CurrentComm { get; set; }
        /// <summary>
        /// The realised cost of this position calculated with regard to average cost accounting.
        /// </summary>
        public decimal RealisedCost { get; set; }
        /// <summary>
        /// currentCost - realisedCost.
        /// </summary>
        public decimal UnrealisedCost { get; set; }
        /// <summary>
        /// The absolute value of your open orders for this symbol.
        /// </summary>
        public decimal GrossOpenCost { get; set; }
        /// <summary>
        /// The amount your bidding above the mark price in the settlement currency of the symbol (currency).
        /// </summary>
        public decimal GrossOpenPremium { get; set; }

        public decimal GrossExecCost { get; set; }
        public decimal MiskValue { get; set; }
        /// <summary>
        /// Value of position in units of underlying.
        /// </summary>
        public decimal HomeNotional { get; set; }
        /// <summary>
        /// Value of position in units of quoteCurrency.
        /// </summary>
        public decimal ForeignNotional { get; set; }
        public string PosState { get; set; }
        public decimal PosCost { get; set; }
        public decimal PosCost2 { get; set; }
        public decimal PosCross { get; set; }
        public decimal PosInit { get; set; }
        public decimal PosComm { get; set; }
        public decimal PosLoss { get; set; }
        public decimal PosMargin { get; set; }
        public decimal PosMaint { get; set; }
        public decimal PosAllowance { get; set; }
        public decimal TaxableMargin { get; set; }
        public decimal InitMargin { get; set; }
        public decimal MaintMargin { get; set; }
        public decimal SessionMargin { get; set; }
        public decimal TargetExcessMargin { get; set; }
        public decimal VarMargin { get; set; }
        public decimal RrealisedGrossPnl { get; set; }
        public decimal RealisedTax { get; set; }
        /// <summary>
        /// The negative of realisedCost.
        /// </summary>
        public decimal RealisedPnl { get; set; }
        /// <summary>
        /// markValue - unrealisedCost.
        /// </summary>
        public decimal UnrealisedGrossPnl { get; set; }
        public decimal LongBankrupt { get; set; }
        public decimal ShortBankrupt { get; set; }
        public decimal TaxBase { get; set; }
        public decimal IndicativeTaxRate { get; set; }
        public decimal IndicativeTax { get; set; }
        public decimal UnrealisedTax { get; set; }
        public decimal UnrealisedRoePcnt { get; set; }
        public decimal? SimpleQty { get; set; }
        public decimal? SimpleCost { get; set; }
        public decimal? SimpleValue { get; set; }
        public decimal? SimplePnl { get; set; }
        public decimal? SimplePnlPcnt { get; set; }
        public decimal? AvgCostPrice { get; set; }
        public decimal? MarginCallPrice { get; set; }
        /// <summary>
        /// Once markPrice reaches this price, this position will have no equity.
        /// </summary>
        public decimal? BankruptPrice { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal LlastPrice { get; set; }
        public decimal LastValue { get; set; } 
    }
}
