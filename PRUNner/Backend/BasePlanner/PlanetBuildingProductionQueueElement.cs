using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetBuildingProductionQueueElement : ReactiveObject
    {
        public PlanetBuildingProductionQueueElement(PlanetBuilding building)
        {
            Building = building;
            ActiveRecipe = Building.AvailableRecipes!.FirstOrDefault();
        }

        public PlanetBuilding Building { get; }
        [Reactive] public PlanetBuildingProductionRecipe? ActiveRecipe { get; set; }
        [Reactive] public double Percentage { get; set; } = 100;

        public void Remove()
        {
            Building.RemoveProduction(this);
        }
    }
}