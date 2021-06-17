using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class EmpireViewModel : ViewModelBase
    {
        public Empire Empire { get; private set; } = new();

        public MainWindowViewModel? MainWindow { get; }

        public EmpireViewModel()
        { }
        
        public EmpireViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindow = mainWindowViewModel;
        }
        
        public PlanetaryBase StartNewBase(PlanetData planetData)
        {
            return Empire.AddPlanetaryBase(planetData);
        }

        public void ViewBase(PlanetaryBase planetaryBase)
        {
            MainWindow?.BasePlannerViewModel.SetActiveBase(planetaryBase);
            MainWindow?.ViewBasePlanner();
        }

        public void SetEmpire(Empire empire)
        {
            Empire = empire;
            this.RaisePropertyChanged(nameof(Empire));
        }
    }
}