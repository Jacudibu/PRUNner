using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;

namespace PRUNner.App.ViewModels
{
    public class EmpireViewModel : ViewModelBase
    {
        public Empire Empire { get; set; } = new();

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
    }
}