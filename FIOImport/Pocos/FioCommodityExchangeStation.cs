using System;

namespace FIOImport.Pocos
{
    public class FioCommodityExchangeStation
    {
        public string NaturalId { get; set; }
        public string Name { get; set; }
        public string SystemId { get; set; }
        public string SystemNaturalId { get; set; }
        public string SystemName { get; set; }
        public object CommisionTimeEpochMs { get; set; }
        public string ComexId { get; set; }
        public string ComexName { get; set; }
        public string ComexCode { get; set; }
        public string WarehouseId { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public int CurrencyNumericCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyDecimals { get; set; }
        public object? GovernorId { get; set; }
        public object? GovernorUserName { get; set; }
        public string GovernorCorporationId { get; set; }
        public string GovernorCorporationName { get; set; }
        public string GovernorCorporationCode { get; set; }
        public string UserNameSubmitted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}