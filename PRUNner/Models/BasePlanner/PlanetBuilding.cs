using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        
        [Reactive] public int Amount { get; set; }

        public PlanetBuilding() // This feels like a hack, but otherwise we can't set the Design.DataContext in BuildingRow...
        {
            Building = null!;
        }
        
        public PlanetBuilding(BuildingData building)
        {
            Building = building;
        }

        public int CalculateNeededArea()
        {
            return Building.AreaCost * Amount;
        }
        
        public void Add()
        {
            Amount++;
        }

        public void Reduce()
        {
            if (Amount == 0)
            {
                return;
            }
            
            Amount--;
        }
    }
}