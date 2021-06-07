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
        
        
        [Reactive] public string PioneersString { get; private set; } = "";
        [Reactive] public string SettlersString { get; private set; } = "";
        [Reactive] public string TechniciansString { get; private set; } = "";
        [Reactive] public string EngineersString { get; private set; } = "";
        [Reactive] public string ScientistsString { get; private set; } = "";

        public void Recalculate(ProvidedConsumables consumables, PlanetWorkforce capacity, PlanetWorkforce required)
        {
            Pioneers = GetWorkforceHabitationPercentage(capacity.Pioneers, required.Pioneers) * WorkforceSatisfactionFactors.Pioneers.Calculate(consumables);
            Settlers = GetWorkforceHabitationPercentage(capacity.Settlers, required.Settlers) *  WorkforceSatisfactionFactors.Settlers.Calculate(consumables);
            Technicians = GetWorkforceHabitationPercentage(capacity.Technicians, required.Technicians) *  WorkforceSatisfactionFactors.Technicians.Calculate(consumables);
            Engineers = GetWorkforceHabitationPercentage(capacity.Engineers, required.Engineers) *  0.7944; // WorkforceSatisfactionFactors.Engineers.Calculate(consumables); // TODO: we don't know the ratios yet
            Scientists = GetWorkforceHabitationPercentage(capacity.Scientists, required.Scientists) *  0.7944; // WorkforceSatisfactionFactors.Scientists.Calculate(consumables);
            
            PioneersString = Pioneers.ToString("P1");
            SettlersString = Settlers.ToString("P1");
            TechniciansString = Technicians.ToString("P1");
            EngineersString = Engineers.ToString("P1");
            ScientistsString = Scientists.ToString("P1");
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