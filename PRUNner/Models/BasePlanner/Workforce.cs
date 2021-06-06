using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class Workforce : ReactiveObject
    {
        [Reactive] public int Pioneers { get; private set; }
        [Reactive] public int Settlers { get; private set; }
        [Reactive] public int Technicians { get; private set; }
        [Reactive] public int Engineers { get; private set; }
        [Reactive] public int Scientists { get; private set; }

        public void Reset()
        {
            Pioneers = 0;
            Settlers = 0;
            Technicians = 0;
            Engineers = 0;
            Scientists = 0;
        }

        public void Add(PlannedBuilding building)
        {
            Pioneers += building.Building.Pioneers * building.Amount;
            Settlers += building.Building.Settlers * building.Amount;
            Technicians += building.Building.Technicians * building.Amount;
            Engineers += building.Building.Engineers * building.Amount;
            Scientists += building.Building.Scientists * building.Amount;
        }
    }
}