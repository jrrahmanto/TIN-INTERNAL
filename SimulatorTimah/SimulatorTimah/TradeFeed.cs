using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTimah
{
    class TradeFeed
    {
        public int Id { get; set; }
        public DateTime BusinessDate { get; set; }
        public int Sequence { get; set; }
        public DateTime TradeTime { get; set; }
        public int TradeTimeOffset { get; set; }
        public string ProductCode { get; set; }
        public string ContractMonth { get; set; }
        public string OptionCode { get; set; }
        public decimal StrikePriceOption { get; set; }
        public int EntitasTradeVersion { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string ContraIndicator { get; set; }
        public string ExchangeRef { get; set; }
        public string SellerClearingCode { get; set; }
        public string SellerExchangeCode { get; set; }
        public string SellerTraderType { get; set; }
        public string SellerAccountCode { get; set; }
        public string SellerGiveUpCode { get; set; }
        public decimal SellerCommisionGiveUp { get; set; }
        public string SellerTradeTypeCode { get; set; }
        public int SellerTotalLegs { get; set; }
        public string SellerRef { get; set; }
        public string BuyerClearingCode { get; set; }
        public string BuyerExchangeCode { get; set; }
        public string BuyerTraderType { get; set; }
        public string BuyerAccountCode { get; set; }
        public string BuyerGiveUpCode { get; set; }
        public decimal BuyerCommisionGiveUp { get; set; }
        public string BuyerTradeTypeCode { get; set; }
        public int BuyerTotalLegs { get; set; }
        public string BuyerRef { get; set; }
        public string Flag { get; set; }
        public string trade_id_detail { get; set; }
    }
}
