using System.Collections.Generic;
using System.Collections.ObjectModel;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Enums;

namespace PRUNner.Backend.BasePlanner
{
    public class PriceDataPreferences
    {
        public ObservableCollection<MaterialPriceDataQueryElement> PriceDataQueryPreferences { get; private set; } = new();

        public static PriceDataPreferences Default => new()
        {
            PriceDataQueryPreferences =
            {
                MaterialPriceDataQueryElement.PlanetOverrides,
                MaterialPriceDataQueryElement.EmpireOverrides,
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Bid),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Average),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.MMBuy),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Ask),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.MMSell),
            }
        };

        public void AddNewElement()
        {
            PriceDataQueryPreferences.Add(new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, "NC1", ExchangePriceType.Ask));
        }
        
        public void RemoveElement(MaterialPriceDataQueryElement element)
        {
            PriceDataQueryPreferences.Remove(element);
        }
        
        public List<string> TemporaryExchangeCodes { get; } = new()
        {
            "NC1", "IC1", "AI1"
        };
    }
}