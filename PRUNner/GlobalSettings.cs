using System.Collections.ObjectModel;
using PRUNner.Backend.Enums;

namespace PRUNner
{
    public static class GlobalSettings
    {
        public static ObservableCollection<PriceDataPollType> PriceDataPreferenceOrder { get; private set; } = new()
        {
            PriceDataPollType.Custom,
            PriceDataPollType.NC1Bid,
            PriceDataPollType.NC1Average,
            PriceDataPollType.MMBuy,
            PriceDataPollType.NC1Ask,
            PriceDataPollType.MMSell
        };
    }
}