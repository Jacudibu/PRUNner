using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class EmpireViewModel : ViewModelBase
    {
        public Empire Empire { get; private set; } = new();

        private readonly MainWindowViewModel? _mainWindow;

        public EmpireViewModel()
        { }
        
        public EmpireViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
        
        public PlanetaryBase StartNewBase(PlanetData planetData)
        {
            return Empire.AddPlanetaryBase(planetData);
        }

        public void ViewBase(PlanetaryBase planetaryBase)
        {
            _mainWindow?.BasePlannerViewModel.SetActiveBase(planetaryBase);
            _mainWindow?.ViewBasePlanner();
        }

        public void SetEmpire(Empire empire)
        {
            Empire = empire;
            this.RaisePropertyChanged(nameof(Empire));
        }
    }
}