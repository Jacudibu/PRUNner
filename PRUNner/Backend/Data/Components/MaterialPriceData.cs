using System;
using System.Collections.Generic;
using FIOImport.Pocos;
using PRUNner.Backend.Enums;
using ReactiveUI;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceData : ReactiveObject
    {
        public double? Custom { get; set; }
        public double? MMBuy { get; private set; }
        public double? MMSell { get; private set; }
        public MaterialPriceDataRegional AI1 { get; } = new();
        public MaterialPriceDataRegional CI1 { get; } = new();
        public MaterialPriceDataRegional IC1 { get; } = new();
        public MaterialPriceDataRegional NC1 { get; } = new();

        internal void Update(FioRainPrices rainPrices)
        {
            MMBuy = rainPrices.MMBuy;
            MMSell = rainPrices.MMSell;
            AI1.Update(rainPrices.AI1Average, rainPrices.AI1AskPrice, rainPrices.AI1BidPrice);
            CI1.Update(rainPrices.CI1Average, rainPrices.CI1AskPrice, rainPrices.CI1BidPrice);
            IC1.Update(rainPrices.IC1Average, rainPrices.IC1AskPrice, rainPrices.IC1BidPrice);
            NC1.Update(rainPrices.NC1Average, rainPrices.NC1AskPrice, rainPrices.NC1BidPrice);
        }

        public double GetPrice()
        {
            return GetPrice(GlobalSettings.PriceDataPreferenceOrder);
        }
        
        public double GetPrice(IEnumerable<PriceDataPollType> pollTypes)
        {
            foreach (var pollType in pollTypes)
            {
                var result = GetPrice(pollType);
                if (result != null)
                {
                    return (double) result;
                }
            }

            return 0;
        }
        
        public double? GetPrice(PriceDataPollType pollType)
        {
            return pollType switch
            {
                PriceDataPollType.Custom => Custom,
                PriceDataPollType.MMBuy => MMBuy,
                PriceDataPollType.MMSell => MMSell,
                PriceDataPollType.AI1Average => AI1.Average,
                PriceDataPollType.AI1Bid => AI1.Bid,
                PriceDataPollType.AI1Ask => AI1.Ask,
                PriceDataPollType.CI1Average => CI1.Average,
                PriceDataPollType.CI1Bid => CI1.Bid,
                PriceDataPollType.CI1Ask => CI1.Ask,
                PriceDataPollType.IC1Average => IC1.Average,
                PriceDataPollType.IC1Bid => IC1.Bid,
                PriceDataPollType.IC1Ask => IC1.Ask,
                PriceDataPollType.NC1Average => NC1.Average,
                PriceDataPollType.NC1Bid => NC1.Bid,
                PriceDataPollType.NC1Ask => NC1.Ask,
                _ => throw new ArgumentOutOfRangeException(nameof(pollType), pollType, null)
            };
        }
    }
}