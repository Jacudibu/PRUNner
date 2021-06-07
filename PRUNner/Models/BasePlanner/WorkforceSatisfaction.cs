using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class WorkforceSatisfaction : ReactiveObject
    {
        [Reactive] public double Pioneers { get; private set; }
        [Reactive] public double Settlers { get; private set; }
        [Reactive] public double Technicians { get; private set; }
        [Reactive] public double Engineers { get; private set; }
        [Reactive] public double Scientists { get; private set; }
        
        public void Recalculate(ProvidedConsumables consumables, PlanetWorkforce capacity, PlanetWorkforce required)
        {
            Pioneers = GetWorkforceHabitationPercentage(capacity.Pioneers, required.Pioneers) * WorkforceSatisfactionFactors.Pioneers.Calculate(consumables);
            Settlers = GetWorkforceHabitationPercentage(capacity.Settlers, required.Settlers) *  WorkforceSatisfactionFactors.Settlers.Calculate(consumables);
            Technicians = GetWorkforceHabitationPercentage(capacity.Technicians, required.Technicians) *  WorkforceSatisfactionFactors.Technicians.Calculate(consumables);
            Engineers = GetWorkforceHabitationPercentage(capacity.Engineers, required.Engineers) * WorkforceSatisfactionFactors.Engineers.Calculate(consumables);
            Scientists = GetWorkforceHabitationPercentage(capacity.Scientists, required.Scientists) * WorkforceSatisfactionFactors.Scientists.Calculate(consumables);
        }

        private static double GetWorkforceHabitationPercentage(double capacity, double required)
        {
            if (required == 0)
            {
                return 0;
            }

            if (capacity > required)
            {
                return 1;
            }

            return capacity / required;
        }
    }
}