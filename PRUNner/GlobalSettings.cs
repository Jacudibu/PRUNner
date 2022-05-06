using System.Collections.ObjectModel;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Enums;

namespace PRUNner
{
    public static class GlobalSettings
    {
        public static ObservableCollection<MaterialPriceDataQueryElement> PriceDataPreferenceOrder { get; private set; } = new()
        {
            MaterialPriceDataQueryElement.PlanetOverrides,
            MaterialPriceDataQueryElement.EmpireOverrides,
            new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Bid),
            new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Average),
            new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.MMBuy),
            new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Ask),
            new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.MMSell),
        };
        
        public static string? IgnoreUpdateTag { get; set; }
    }
}