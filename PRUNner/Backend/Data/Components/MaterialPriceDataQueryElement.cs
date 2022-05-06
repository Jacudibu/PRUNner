using System;
using PRUNner.Backend.Enums;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceDataQueryElement
    {
        public PriceDataQueryType QueryType;
        public string ExchangeCode = "";
        public ExchangePriceType PriceType;

        public static readonly MaterialPriceDataQueryElement EmpireOverrides = new() {QueryType = PriceDataQueryType.EmpireOverrides};
        public static readonly MaterialPriceDataQueryElement PlanetOverrides = new() {QueryType = PriceDataQueryType.PlanetOverrides};

        private MaterialPriceDataQueryElement()
        { }
        
        public MaterialPriceDataQueryElement(PriceDataQueryType queryType, string exchangeCode, ExchangePriceType priceType)
        {
            QueryType = queryType;
            ExchangeCode = exchangeCode;
            PriceType = priceType;
        }
        
        public override string ToString()
        {
            return QueryType switch
            {
                PriceDataQueryType.EmpireOverrides => nameof(PriceDataQueryType.EmpireOverrides),
                PriceDataQueryType.PlanetOverrides => nameof(PriceDataQueryType.PlanetOverrides),
                PriceDataQueryType.Exchange => ExchangeCode + PriceType,
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
                result.ExchangeCode = "NC1";
                result.PriceType = ExchangePriceType.MMBuy;
            }
            else if (value.Equals(nameof(ExchangePriceType.MMSell)))
            {
                result.ExchangeCode = "NC1";
                result.PriceType = ExchangePriceType.MMSell;
            }
            else
            {
                // TODO: This obviously breaks miserably if any exchange ID ever has more than 3 letters...
                // TODO: Instead, we could iterate through all exchanges and see if the string starts with their exchange code.
                result.ExchangeCode = value[..3];
                result.PriceType = Enum.Parse<ExchangePriceType>(value[3..]);
            }

            return result;
        }
    }
}