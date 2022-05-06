using System;
using System.Collections.Generic;
using FIOImport.Pocos;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Enums;
using ReactiveUI;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceData : ReactiveObject
    {
        private readonly MaterialData _materialData;
        public readonly Dictionary<string, MaterialPriceDataRegional> ExchangePrices = new();

        public MaterialPriceData(MaterialData materialData)
        {
            _materialData = materialData;
        }

        internal void Initialize()
        {
            foreach (var exchange in CommodityExchangeData.GetAll())
            {
                ExchangePrices[exchange.Id] = new MaterialPriceDataRegional();
            }
        }
        
        internal void Update(FioExchangeData exchangeData)
        {
            ExchangePrices[exchangeData.ExchangeCode].Update(exchangeData);
        }

        public double GetPrice(PlanetaryBase planetaryBase)
        {
            return GetPrice(planetaryBase.Empire, planetaryBase);
        }

        public double GetPrice(Empire empire, PlanetaryBase? planetaryBase = null)
        {
            return GetPrice(empire.PriceDataPreferences.PriceDataQueryPreferences, empire.PriceOverrides, planetaryBase?.PriceOverrides);
        }

        private double GetPrice(IEnumerable<MaterialPriceDataQueryElement> queries, PriceOverrides? empirePriceOverrides, PriceOverrides? planetPriceOverrides)
        {
            foreach (var query in queries)
            {
                var result = GetPrice(query, empirePriceOverrides, planetPriceOverrides);
                if (result != null)
                {
                    return (double) result;
                }
            }

            return 0;
        }

        private double? GetPrice(MaterialPriceDataQueryElement queryElement, PriceOverrides? empirePriceOverrides = null, PriceOverrides? planetPriceOverrides = null)
        {
            return queryElement.QueryType switch
            {
                PriceDataQueryType.EmpireOverrides => empirePriceOverrides?.GetOverrideForTicker(_materialData.Ticker) ?? null,
                PriceDataQueryType.PlanetOverrides => planetPriceOverrides?.GetOverrideForTicker(_materialData.Ticker) ?? null,
                PriceDataQueryType.Exchange => ExchangePrices[queryElement.Exchange.Id].Get(queryElement.PriceType),
                _ => throw new ArgumentOutOfRangeException(nameof(queryElement.QueryType), queryElement.QueryType, null)
            };
        }
    }
}