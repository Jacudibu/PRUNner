using System.Windows.Input;
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
            PlanetFinderViewModel = new PlanetFinderViewModel();
        }
        
        public ICommand ViewPlanetFinder => ReactiveCommand.Create(() => ActiveView = PlanetFinderViewModel);
    }
}