using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData.Binding;
using Newtonsoft.Json.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Enums;
using ReactiveUI;

namespace PRUNner.Backend.BasePlanner
{
    public class PriceDataPreferences : ReactiveObject
    {
        public ObservableCollection<MaterialPriceDataQueryElement> PriceDataQueryPreferences { get; private init; } = new();

        public static PriceDataPreferences CreateDefault()
        {
            var result = new PriceDataPreferences();

            var exchange = CommodityExchangeData.GetAll().First();
            result.AddNewElement(PriceDataQueryType.PlanetOverrides, exchange, ExchangePriceType.Bid);
            result.AddNewElement(PriceDataQueryType.EmpireOverrides, exchange, ExchangePriceType.Bid);
            result.AddNewElement(PriceDataQueryType.Exchange, exchange, ExchangePriceType.Bid);
            result.AddNewElement(PriceDataQueryType.Exchange, exchange, ExchangePriceType.Average);
            result.AddNewElement(PriceDataQueryType.Exchange, exchange, ExchangePriceType.MMBuy);
            result.AddNewElement(PriceDataQueryType.Exchange, exchange, ExchangePriceType.Ask);
            result.AddNewElement(PriceDataQueryType.Exchange, exchange, ExchangePriceType.MMSell);

            return result;
        }

        public void AddNewDefaultElement()
        {
            AddNewElement(PriceDataQueryType.Exchange, CommodityExchangeData.GetAll().First(), ExchangePriceType.Ask);
        }

        public void AddNewElement(PriceDataQueryType queryType, CommodityExchangeData exchange, ExchangePriceType priceType)
        {
            var element = new MaterialPriceDataQueryElement(queryType, exchange, priceType);
            AddNewElement(element);
        }

        private void AddNewElement(MaterialPriceDataQueryElement element)
        {
            element.WhenPropertyChanged(x => x.QueryType).Subscribe(_ => this.RaisePropertyChanged(nameof(PriceDataQueryPreferences)));
            element.WhenPropertyChanged(x => x.Exchange).Subscribe(_ => this.RaisePropertyChanged(nameof(PriceDataQueryPreferences)));
            element.WhenPropertyChanged(x => x.PriceType).Subscribe(_ => this.RaisePropertyChanged(nameof(PriceDataQueryPreferences)));
            PriceDataQueryPreferences.Add(element);
        }
        
        public void RemoveElement(MaterialPriceDataQueryElement element)
        {
            PriceDataQueryPreferences.Remove(element);
        }

        public JToken ToJson()
        {
            var priceDataArray = new JArray();
            foreach (var priceDataPollType in PriceDataQueryPreferences)
            {
                priceDataArray.Add(priceDataPollType.ToString());
            }

            return priceDataArray;
        }

        public void ParseJson(JToken? jToken)
        {
            if (jToken is not JArray jArray)
            {
                return;
            }
            
            PriceDataQueryPreferences.Clear();
            foreach (var element in jArray.Select(x => x.ToObject<string>()))
            {
                if (string.IsNullOrEmpty(element))
                {
                    continue;
                }

                if (element.Equals("Custom"))
                {
                    // version 0.2.1 and below
                    AddNewElement(MaterialPriceDataQueryElement.PlanetOverrides);
                    AddNewElement(MaterialPriceDataQueryElement.EmpireOverrides);
                }
                else
                {
                    AddNewElement(MaterialPriceDataQueryElement.FromString(element));
                }
            }
        }
    }
}