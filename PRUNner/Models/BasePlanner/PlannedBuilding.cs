using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlannedBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        
        [Reactive] public int Amount { get; set; }

        public PlannedBuilding(BuildingData building)
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