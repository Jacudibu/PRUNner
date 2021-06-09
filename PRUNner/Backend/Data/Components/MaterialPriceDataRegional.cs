using ReactiveUI;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialPriceDataRegional : ReactiveObject
    {
        public double Average { get; private set; }
        public double? Ask { get; private set; }
        public double? Bid { get; private set; }

        internal void Update(double average, double? ask, double? bid)
        {
            Average = average;
            Ask = ask;
            Bid = bid;
        }
    }
}