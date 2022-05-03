namespace FIOImport.Pocos
{
    public class FioCommodityExchange
    {
        public string ExchangeId { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeCode { get; set; }
        public object ExchangeOperatorId { get; set; }
        public object ExchangeOperatorCode { get; set; }
        public object ExchangeOperatorName { get; set; }
        public int CurrencyNumericCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyDecimals { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationNaturalId { get; set; }
    }
}