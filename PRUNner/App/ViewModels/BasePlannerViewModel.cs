using PRUNner.App.Models;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;

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