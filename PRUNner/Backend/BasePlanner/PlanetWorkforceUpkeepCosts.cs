using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetWorkforceUpkeepCosts : ReactiveObject
    {
        private readonly PlanetaryBase _planetaryBase;
        
        public PlanetWorkforceUpkeepCosts(PlanetaryBase planetaryBase)
        {
            _planetaryBase = planetaryBase;
        }

        [Reactive] public double Pioneers { get; private set; }
        [Reactive] public double Settlers { get; private set; }
        [Reactive] public double Technicians { get; private set; }
        [Reactive] public double Engineers { get; private set; }
        [Reactive] public double Scientists { get; private set; }
        
        public void Recalculate()
        {
            Pioneers = Recalculate(WorkforceConsumptionFactors.Pioneers);
            Settlers = Recalculate(WorkforceConsumptionFactors.Settlers);
            Technicians = Recalculate(WorkforceConsumptionFactors.Technicians);
            Engineers = Recalculate(WorkforceConsumptionFactors.Engineers);
            Scientists = Recalculate(WorkforceConsumptionFactors.Scientists);
        }

        private double Recalculate(WorkforceConsumptionFactors factors)
        {
            return factors.CalculateCosts(_planetaryBase);
        }
    }
}