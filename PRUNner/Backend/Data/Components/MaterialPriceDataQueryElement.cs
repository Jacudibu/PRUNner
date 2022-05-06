using System;
using System.Linq;
using PRUNner.Backend.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceDataQueryElement : ReactiveObject
    {
        public PriceDataQueryType QueryType
        {
            get => _queryType;
            set
            {
                this.RaiseAndSetIfChanged(ref _queryType, value);
                this.RaisePropertyChanged(nameof(RequiresExchange));
            }
        }

        [Reactive] public CommodityExchangeData Exchange { get; set; }
        [Reactive] public ExchangePriceType PriceType { get; set; }

        public static MaterialPriceDataQueryElement EmpireOverrides => new() {QueryType = PriceDataQueryType.EmpireOverrides};
        public static MaterialPriceDataQueryElement PlanetOverrides => new() {QueryType = PriceDataQueryType.PlanetOverrides};
        private PriceDataQueryType _queryType;

        public bool RequiresExchange => QueryType == PriceDataQueryType.Exchange;

        private MaterialPriceDataQueryElement()
        {
            Exchange = CommodityExchangeData.GetAll().First();
        }
        
        public MaterialPriceDataQueryElement(PriceDataQueryType queryType, CommodityExchangeData exchange, ExchangePriceType priceType)
        {
            QueryType = queryType;
            Exchange = exchange;
            PriceType = priceType;
        }
        
        public override string ToString()
        {
            return QueryType switch
            {
                PriceDataQueryType.EmpireOverrides => nameof(PriceDataQueryType.EmpireOverrides),
                PriceDataQueryType.PlanetOverrides => nameof(PriceDataQueryType.PlanetOverrides),
                PriceDataQueryType.Exchange => Exchange.Id + PriceType,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static MaterialPriceDataQueryElement FromString(string value)
        {
            if (string.Equals(value, nameof(PriceDataQueryType.EmpireOverrides)))
            {
                return EmpireOverrides;
            }

            if (string.Equals(value, nameof(PriceDataQueryType.PlanetOverrides)))
            {
                return PlanetOverrides;
            }

            var result = new MaterialPriceDataQueryElement();
            result.QueryType = PriceDataQueryType.Exchange;
                
            // Temporary backwards compatibility, these two comparisons can be removed some time in the future
            if (value.Equals(nameof(ExchangePriceType.MMBuy)))
            {
                result.Exchange = CommodityExchangeData.GetAll().First();
                result.PriceType = ExchangePriceType.MMBuy;
            }
            else if (value.Equals(nameof(ExchangePriceType.MMSell)))
            {
                result.Exchange = CommodityExchangeData.GetAll().First();
                result.PriceType = ExchangePriceType.MMSell;
            }
            else
            {
                // TODO: This obviously breaks miserably if any exchange ID ever has more than 3 letters...
                // TODO: Instead, we could iterate through all exchanges and see if the string starts with their exchange code.
                result.Exchange = CommodityExchangeData.GetOrThrow(value[..3]);
                result.PriceType = Enum.Parse<ExchangePriceType>(value[3..]);
            }

            return result;
        }
    }
}