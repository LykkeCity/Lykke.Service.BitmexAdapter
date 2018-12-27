using System;
using Newtonsoft.Json;

namespace Lykke.Service.BitmexApi
{
    public class OrderResult
    { 
        public string OrderID { get; set; }
        public string ClOrdID { get; set; }
        public string ClOrdLinkID { get; set; }
        public int Account { get; set; }
        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public string SimpleOrderQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? DisplayQty { get; set; }
        public decimal? StopPx { get; set; }
        public decimal? PegOffsetValue { get; set; }
        public string PegPriceType { get; set; }
        public string Currency { get; set; }
        public string SettlCurrency { get; set; }
        public OrderType OrdType { get; set; }
        public string TimeInForce { get; set; }
        public string ExecInst { get; set; }
        public string ContingencyType { get; set; }
        public string ExDestination { get; set; }
        public string Triggered { get; set; }
        public bool WorkingIndicator { get; set; }
        public string OrdRejReason { get; set; }
        public decimal? SimpleLeavesQty { get; set; }
        public decimal LeavesQty { get; set; }
        public decimal? SimpleCumQty { get; set; }
        public decimal CumQty { get; set; }

        [JsonProperty(PropertyName = "avgPx")]
        public decimal? AvgExecutedPrice { get; set; }

        public string MultiLegReportingType { get; set; }
        public string Text { get; set; }
        public DateTime TransactTime { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
