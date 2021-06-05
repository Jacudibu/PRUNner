using System.Collections.Generic;
using System.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using ReactiveUI;

namespace PRUNner.ViewModels
{
    public class PlanetFinderViewModel : ViewModelBase
    {
        public string Item1 { get; set; } = "";
        public string Item2 { get; set; } = "";
        public string Item3 { get; set; } = "";
        public string Item4 { get; set; } = "";

        public bool DisplayItem1 => !string.IsNullOrWhiteSpace(Item1);
        public bool DisplayItem2 => !string.IsNullOrWhiteSpace(Item2);
        public bool DisplayItem3 => !string.IsNullOrWhiteSpace(Item3);
        public bool DisplayItem4 => !string.IsNullOrWhiteSpace(Item4);
        
        public bool DisplayFertile { get; set; }
        public bool DisplayRocky { get; set; } = true;
        public bool DisplayGaseous { get; set; }
        public bool DisplayLowGravity { get; set; }
        public bool DisplayLowPressure { get; set; }
        public bool DisplayLowTemperature { get; set; }
        public bool DisplayHighGravity { get; set; }
        public bool DisplayHighPressure { get; set; }
        public bool DisplayHighTemperature { get; set; }

        public void Search()
        {
            var filterCriteria = new FilterCriteria()
            {
                ExcludeGaseous = !DisplayGaseous,
                ExcludeRocky = !DisplayRocky,
                ExcludeInfertile = DisplayFertile,
                ExcludeLowGravity = !DisplayLowGravity,
                ExcludeLowPressure = !DisplayLowPressure,
                ExcludeLowTemperature = !DisplayLowTemperature,
                ExcludeHighGravity = !DisplayHighGravity,
                ExcludeHighPressure = !DisplayHighPressure,
                ExcludeHighTemperature = !DisplayHighTemperature
            };

            var tickers = new List<string>() {Item1, Item2, Item3, Item4}
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => MaterialData.AllItems[x])
                .ToArray();
            
            this.RaisePropertyChanged(nameof(DisplayItem1));
            this.RaisePropertyChanged(nameof(DisplayItem2));
            this.RaisePropertyChanged(nameof(DisplayItem3));
            this.RaisePropertyChanged(nameof(DisplayItem4));         
            this.RaisePropertyChanged(nameof(Item1));
            this.RaisePropertyChanged(nameof(Item2));
            this.RaisePropertyChanged(nameof(Item3));
            this.RaisePropertyChanged(nameof(Item4));
            
            var result = PlanetFinder.Find(filterCriteria, tickers).OrderBy(x => x.DistancePyrgos).ToList();
            SearchResults = result;
        }

        private IEnumerable<PlanetFinderSearchResult> _searchResults = new List<PlanetFinderSearchResult>();
        public IEnumerable<PlanetFinderSearchResult> SearchResults
        {
            get => _searchResults;
            private set => this.RaiseAndSetIfChanged(ref _searchResults, value);
        }
    }
}