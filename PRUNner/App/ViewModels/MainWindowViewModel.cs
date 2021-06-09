using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public ViewModelBase ActiveView { get; private set; }

        private readonly PlanetFinderViewModel _planetFinderViewModel;
        private readonly BasePlannerViewModel _basePlannerViewModel;

        public MainWindowViewModel()
        {
            DataParser.LoadAndParseFromCache();
            
            _planetFinderViewModel = new PlanetFinderViewModel();
            _basePlannerViewModel = new BasePlannerViewModel();

            ActiveView = _planetFinderViewModel;

            PlanetFinderSearchResult.OnOpenBasePlanner += PlanetFinderSelectPlanetEvent;
        }

        public void ViewPlanetFinder()
        {
            ActiveView = _planetFinderViewModel;
        }

        private void PlanetFinderSelectPlanetEvent(object? sender, PlanetData e)
        {
            _basePlannerViewModel.StartNewBase(e);
            ViewBasePlanner();
        }

        public void ViewBasePlanner()
        {
            ActiveView = _basePlannerViewModel;
        }
    }
}