using System;
using System.Linq;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using PRUNner.Backend.UserDataParser;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public ViewModelBase ActiveView { get; private set; }

        public readonly EmpireViewModel EmpireViewModel;
        public readonly BasePlannerViewModel BasePlannerViewModel;
        public readonly PlanetFinderViewModel PlanetFinderViewModel;

        public MainWindowViewModel()
        {
            DataParser.LoadAndParseFromCache();
            
            EmpireViewModel = new EmpireViewModel(this);
            BasePlannerViewModel = new BasePlannerViewModel();
            PlanetFinderViewModel = new PlanetFinderViewModel();

            ActiveView = PlanetFinderViewModel;

            PlanetFinderSearchResult.OnOpenBasePlanner += PlanetFinderSelectPlanetEvent;
            
            LoadFromDisk();
        }

        public void ViewPlanetFinder()
        {
            ActiveView = PlanetFinderViewModel;
        }

        private void PlanetFinderSelectPlanetEvent(object? sender, PlanetData planet)
        {
            var planetaryBase = EmpireViewModel.StartNewBase(planet);
            BasePlannerViewModel.SetActiveBase(planetaryBase);
            ViewBasePlanner();
        }

        public void ViewEmpire()
        {
            ActiveView = EmpireViewModel;
        }
        
        public void ViewBasePlanner()
        {
            ActiveView = BasePlannerViewModel;
        }
        
        public void UpdatePriceData()
        {
            DataParser.UpdatePriceData();
        }

        public void BlameFio()
        {
            Console.WriteLine("Nah, too lazy");
        }
        
        public void SaveToDisk()
        {
            UserDataWriter.Save(EmpireViewModel.Empire);
        }

        public void LoadFromDisk()
        {
            EmpireViewModel.SetEmpire(UserDataReader.Load());
            BasePlannerViewModel.SetActiveBase(EmpireViewModel.Empire.PlanetaryBases.FirstOrDefault());
        }
    }
}