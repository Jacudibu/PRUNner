using System;
using FIOImport.Pocos;
using PRUNner.Backend.Enums;
using ReactiveUI;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceDataRegional : ReactiveObject
    {
        public double Average { get; private set; }
        public double? Ask { get; private set; }
        public double? Bid { get; private set; }
        public double? MMBuy { get; private set; }
        public double? MMSell { get; private set; }

        public void Update(FioExchangeData exchangeData)
        {
            MMBuy = exchangeData.MMBuy;
            MMSell = exchangeData.MMSell;

            Ask = exchangeData.Ask;
            Average = exchangeData.PriceAverage;
            Bid = exchangeData.Bid;
        }

        public double? Get(ExchangePriceType configPriceType, bool isInput)
        {
            return configPriceType switch
            {
                ExchangePriceType.Ask => Ask,
                ExchangePriceType.Average => Average,
                ExchangePriceType.Bid => Bid,
                ExchangePriceType.MMBuy => MMBuy,
                ExchangePriceType.MMSell => MMSell,
                ExchangePriceType.Worse => isInput ? Ask : Bid,
                _ => throw new ArgumentOutOfRangeException(nameof(configPriceType), configPriceType, null)
            };
        }
    }
}