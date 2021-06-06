using System;
using System.Collections.Generic;
using System.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.Enums;
using PRUNner.Backend.PlanetFinder;
using PRUNner.Models;
using ReactiveUI;

namespace PRUNner.ViewModels
{
    public class PlanetFinderViewModel : ViewModelBase
    {
        public MaterialTextBox Item1 { get; } = new();
        public MaterialTextBox Item2 { get; } = new();
        public MaterialTextBox Item3 { get; } = new();
        public MaterialTextBox Item4 { get; } = new();

        public LastPlanetFinderSearch LastSearch { get; } = new();

        public bool DisplayFertile { get; set; }
        public bool DisplayRocky { get; set; } = true;
        public bool DisplayGaseous { get; set; }
        public bool DisplayLowGravity { get; set; }
        public bool DisplayLowPressure { get; set; }
        public bool DisplayLowTemperature { get; set; }
        public bool DisplayHighGravity { get; set; }
        public bool DisplayHighPressure { get; set; }
        public bool DisplayHighTemperature { get; set; }

        public OptionalPlanetFinderDataObject OptionalFinderData { get; } = new();
        public string OptionalDataExtraSystemName { get; private set; } = "";
        public bool DisplayOptionalDataExtraSystemName { get; private set; }
        
        private bool _showPaginationAndHeaders;
        public bool ShowPaginationAndHeaders
        {
            get => _showPaginationAndHeaders;
            set => this.RaiseAndSetIfChanged(ref _showPaginationAndHeaders, value);
        }

        private bool _noResultsFound;
        public bool NoResultsFound
        {
            get => _noResultsFound;
            set =>  this.RaiseAndSetIfChanged(ref _noResultsFound, value);
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => this.RaiseAndSetIfChanged(ref _totalPages, value);
        }
        
        private List<PlanetFinderSearchResult> _allResults = new();
        private IEnumerable<PlanetFinderSearchResult> _currentlyShownSearchResults = new List<PlanetFinderSearchResult>();
        public IEnumerable<PlanetFinderSearchResult> CurrentlyShownSearchResults
        {
            get => _currentlyShownSearchResults;
            private set => this.RaiseAndSetIfChanged(ref _currentlyShownSearchResults, value);
        }

        public int ItemsPerPage { get; set; } = 15;

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

            var tickers = new List<MaterialData?>() {Item1.Material, Item2.Material, Item3.Material, Item4.Material}
                .Where(x => x != null)
                .ToArray();

            LastSearch.Update(this);

            if (OptionalFinderData.ExtraSystem.System != null)
            {
                OptionalDataExtraSystemName = OptionalFinderData.ExtraSystem.SystemName;
                DisplayOptionalDataExtraSystemName = true;
            }
            else
            {
                OptionalDataExtraSystemName = "";
                DisplayOptionalDataExtraSystemName = false;
            }
            this.RaisePropertyChanged(nameof(OptionalDataExtraSystemName));
            this.RaisePropertyChanged(nameof(DisplayOptionalDataExtraSystemName));

            var optionalData = new OptionalPlanetFinderData
            {
                AdditionalSystem = OptionalFinderData.ExtraSystem.System
            };

            _allResults = PlanetFinder.Find(filterCriteria, tickers!, optionalData).ToList();
            TotalPages = _allResults.Count / ItemsPerPage + 1;
            ShowPaginationAndHeaders = _allResults.Count > 0;
            NoResultsFound = _allResults.Count == 0;
            ResetView();
        }

        private void ResetView()
        {
            CurrentPage = 1;
            CurrentlyShownSearchResults = _allResults.Take(ItemsPerPage);
        }
        
        public void NextPage()
        {
            CurrentPage++;
            if (CurrentPage > TotalPages)
            {
                CurrentPage = 1;
            }
            
            CurrentlyShownSearchResults = _allResults.Skip(ItemsPerPage * CurrentPage - ItemsPerPage).Take(ItemsPerPage);
        }
        
        public void PreviousPage()
        {
            CurrentPage--;
            if (CurrentPage < 1)
            {
                CurrentPage = TotalPages;
            }
            
            CurrentlyShownSearchResults = _allResults.Skip(ItemsPerPage * CurrentPage - ItemsPerPage).Take(ItemsPerPage);
        }

        private string _currentSortMode = "none";
        private void Sort(string sortModeName, SortOrder defaultSortOrder, Func<PlanetFinderSearchResult, object> comparer)
        {
            if (_currentSortMode.Equals(sortModeName))
            {
                _allResults = defaultSortOrder == SortOrder.Ascending 
                    ? _allResults.OrderByDescending(comparer).ToList()
                    : _allResults.OrderBy(comparer).ToList();
                _currentSortMode = sortModeName + "Inverse";
            }
            else
            {
                _allResults = defaultSortOrder == SortOrder.Ascending 
                    ? _allResults.OrderBy(comparer).ToList()
                    : _allResults.OrderByDescending(comparer).ToList();
                _currentSortMode = sortModeName;
            }
            
            ResetView();
        }
        
        public void SortByName() => Sort(nameof(SortByName), SortOrder.Ascending, x => x.Planet.Name);
        public void SortByBuildingMaterials() => Sort(nameof(SortByBuildingMaterials), SortOrder.Ascending, x => x.Planet.PlanetFinderCache.BuildingMaterialString);
        public void SortByRes1() => Sort(nameof(SortByRes1),  SortOrder.Descending, x => x.Resource1Yield);
        public void SortByRes2() => Sort(nameof(SortByRes2),  SortOrder.Descending, x => x.Resource2Yield);
        public void SortByRes3() => Sort(nameof(SortByRes3),  SortOrder.Descending, x => x.Resource3Yield);
        public void SortByRes4() => Sort(nameof(SortByRes4),  SortOrder.Descending, x => x.Resource4Yield);
        public void SortByAntaresDistance() => Sort(nameof(SortByAntaresDistance), SortOrder.Ascending, x => x.Planet.PlanetFinderCache.DistanceToAntares);
        public void SortByBentenDistance() => Sort(nameof(SortByBentenDistance), SortOrder.Ascending, x => x.Planet.PlanetFinderCache.DistanceToBenten);
        public void SortByHortusDistance() => Sort(nameof(SortByHortusDistance), SortOrder.Ascending, x => x.Planet.PlanetFinderCache.DistanceToHortus);
        public void SortByMoriaDistance() => Sort(nameof(SortByMoriaDistance), SortOrder.Ascending, x => x.Planet.PlanetFinderCache.DistanceToMoria);
        public void SortByExtraSystemDistance() => Sort(nameof(SortByExtraSystemDistance), SortOrder.Ascending, x => x.DistanceToExtraSystem);
    }
}