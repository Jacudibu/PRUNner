using PRUNner.Backend.Data.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetWorkforce : ReactiveObject
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

        public void Add(BuildingWorkforce workforce, int factor)
        {
            Pioneers += workforce.Pioneers * factor;
            Settlers += workforce.Settlers * factor;
            Technicians += workforce.Technicians * factor;
            Engineers += workforce.Engineers * factor;
            Scientists += workforce.Scientists * factor;
        }

        public void SetRemainingWorkforce(PlanetWorkforce required, PlanetWorkforce capacity)
        {
            Pioneers = capacity.Pioneers - required.Pioneers;
            Settlers = capacity.Settlers - required.Settlers;
            Technicians = capacity.Technicians - required.Technicians;
            Engineers = capacity.Engineers - required.Engineers;
            Scientists = capacity.Scientists - required.Scientists;
        }
    }
}