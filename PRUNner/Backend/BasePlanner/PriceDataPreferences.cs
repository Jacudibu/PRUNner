using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend.Data;
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
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.Bid),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.Average),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.MMBuy),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.Ask),
                new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.MMSell),
            }
        };

        public void AddNewElement()
        {
            PriceDataQueryPreferences.Add(new MaterialPriceDataQueryElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.Ask));
        }
        
        public void RemoveElement(MaterialPriceDataQueryElement element)
        {
            PriceDataQueryPreferences.Remove(element);
        }

        public ImmutableArray<CommodityExchangeData> OrderedExchangeList => CommodityExchangeData.GetAll();
    }
}