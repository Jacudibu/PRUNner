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
        [Reactive] public double Percentage { get; set; } = 0;
        [Reactive] public short OrderSize { get; set; } = 1;
        [Reactive] public string DurationString { get; private set; } = null!;

        public void Remove()
        {
            Building.RemoveProduction(this);
        }

        public void UpdateDuration()
        {
            DurationString = ActiveRecipe == null 
                ? Utils.GetDurationString(double.PositiveInfinity) 
                : Utils.GetDurationString(ActiveRecipe.DurationInMilliseconds * OrderSize);
        }
        
        public void UpdatePercentage(double totalProductionTime)
        {
            if (ActiveRecipe == null)
            {
                Percentage = 0;
            }
            else
            {
                Percentage = OrderSize * ActiveRecipe.DurationInMilliseconds / totalProductionTime;
            }
        }
    }
}