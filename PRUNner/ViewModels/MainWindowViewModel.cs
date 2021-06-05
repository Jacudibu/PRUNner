using System;
using System.Reactive;
using PRUNner.Backend;
using ReactiveUI;

namespace PRUNner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _activeView;
        public ViewModelBase ActiveView
        {
            get => _activeView;
            set => this.RaiseAndSetIfChanged(ref _activeView, value);
        }

        private PlanetFinderViewModel PlanetFinderViewModel;

        public MainWindowViewModel()
        {
            RxApp.DefaultExceptionHandler = new AnonymousObserver<Exception>(exception => throw exception);
            ParsedData.LoadAndParseFromCache();
            
            PlanetFinderViewModel = new PlanetFinderViewModel();

            ActiveView = PlanetFinderViewModel;
        }
        
        public void ViewPlanetFinder()
        {
            ActiveView = PlanetFinderViewModel;
        }
    }
}