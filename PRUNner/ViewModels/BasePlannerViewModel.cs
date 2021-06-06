using PRUNner.Backend.Data;
using PRUNner.Models;
using PRUNner.Models.BasePlanner;

namespace PRUNner.ViewModels
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