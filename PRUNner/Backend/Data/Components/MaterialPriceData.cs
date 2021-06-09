using FIOImport.Pocos;
using ReactiveUI;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceData : ReactiveObject
    {
        public double? MMBuy { get; set; } = new();
        public double? MMSell { get; set; } = new();
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
    }
}