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

        private readonly PlanetFinderViewModel _planetFinderViewModel;

        public MainWindowViewModel()
        {
            RxApp.DefaultExceptionHandler = new AnonymousObserver<Exception>(exception => throw exception);
            DataParser.LoadAndParseFromCache();
            
            _planetFinderViewModel = new PlanetFinderViewModel();

            ActiveView = _planetFinderViewModel;
        }
        
        public void ViewPlanetFinder()
        {
            ActiveView = _planetFinderViewModel;
        }
    }
}