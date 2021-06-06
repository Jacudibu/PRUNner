using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        
        [Reactive] public int Amount { get; set; }

        public PlanetBuilding(BuildingData building)
        {
            Building = building;
        }

        public void Add()
        {
            Amount++;
        }

        public void Reduce()
        {
            Amount--;
        }
    }
}