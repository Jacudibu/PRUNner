using PRUNner.App.Models;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class BasePlannerViewModel : ViewModelBase
    {
        public PlanetaryBase ActiveBase { get; private set; }

        public BuildingTextBox AddBuildingTextBox { get; } = new();
        
        public void StartNewBase(PlanetData planetData)
        {
            ActiveBase = new PlanetaryBase(planetData);
        }

        public void SetActiveBase(PlanetaryBase planetaryBase)
        {
            ActiveBase = planetaryBase;
            this.RaisePropertyChanged(nameof(ActiveBase));
        }

        public void AddBuilding()
        {
            if (AddBuildingTextBox.Building == null)
            {
                return;
            }

            ActiveBase.AddBuilding(AddBuildingTextBox.Building);
        }
    }
}