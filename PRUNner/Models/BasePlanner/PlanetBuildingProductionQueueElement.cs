using System.Linq;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuildingProductionQueueElement
    {
        public PlanetBuildingProductionQueueElement() {}
        
        public PlanetBuildingProductionQueueElement(PlanetBuilding building)
        {
            Building = building;
            ActiveRecipe = Building.AvailableRecipes.FirstOrDefault();
        }

        public PlanetBuilding Building { get; }
        public PlanetBuildingProductionElement? ActiveRecipe { get; set; }
        public double Percentage { get; set; } = 100;

        public void Remove()
        {
            Building.RemoveProduction(this);
        }
    }
}