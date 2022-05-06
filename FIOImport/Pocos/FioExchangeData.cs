namespace FIOImport.Pocos
{
    public class FioExchangeData
    {
        public string MaterialTicker { get; set; }
        public string ExchangeCode { get; set; }
        public double? MMBuy { get; set; }
        public double? MMSell { get; set; }
        public double PriceAverage { get; set; }
        public int? AskCount { get; set; }
        public double? Ask { get; set; }
        public int Supply { get; set; }
        public int? BidCount { get; set; }
        public double? Bid { get; set; }
        public int Demand { get; set; }
    }
}