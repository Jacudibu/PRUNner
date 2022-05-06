using System.Collections.ObjectModel;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data.Components;

namespace PRUNner
{
    public static class GlobalSettings
    {
        public static ObservableCollection<MaterialPriceDataQueryElement> PriceDataPreferenceOrder { get; private set; } = PriceDataPreferences.Default.PriceDataQueryPreferences;
        
        public static string? IgnoreUpdateTag { get; set; }
    }
}