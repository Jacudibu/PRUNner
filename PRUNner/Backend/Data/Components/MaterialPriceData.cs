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

        public double GetPrice(bool isInput, PlanetaryBase planetaryBase)
        {
            return GetPrice(isInput, planetaryBase.Empire, planetaryBase);
        }

        public double GetPrice(bool isInput, Empire empire, PlanetaryBase? planetaryBase = null)
        {
            if (planetaryBase != null)
            {
                foreach (var query in planetaryBase.PriceDataPreferences.PriceDataQueryPreferences)
                {
                    var result = GetPrice(query, isInput, empire.PriceOverrides, planetaryBase.PriceOverrides);
                    if (result != null)
                    {
                        return (double) result;
                    }
                }
            }
            
            foreach (var query in empire.PriceDataPreferences.PriceDataQueryPreferences)
            {
                var result = GetPrice(query, isInput, empire.PriceOverrides, planetaryBase?.PriceOverrides);
                if (result != null)
                {
                    return (double) result;
                }
            }

            return 0;
        }

        private double? GetPrice(MaterialPriceDataQueryElement queryElement, bool isInput, PriceOverrides? empirePriceOverrides = null, PriceOverrides? planetPriceOverrides = null)
        {
            return queryElement.QueryType switch
            {
                PriceDataQueryType.EmpireOverrides => empirePriceOverrides?.GetOverrideForTicker(_materialData.Ticker) ?? null,
                PriceDataQueryType.PlanetOverrides => planetPriceOverrides?.GetOverrideForTicker(_materialData.Ticker) ?? null,
                PriceDataQueryType.Exchange => ExchangePrices[queryElement.Exchange.Id].Get(queryElement.PriceType, isInput),
                _ => throw new ArgumentOutOfRangeException(nameof(queryElement.QueryType), queryElement.QueryType, null)
            };
        }
    }
}