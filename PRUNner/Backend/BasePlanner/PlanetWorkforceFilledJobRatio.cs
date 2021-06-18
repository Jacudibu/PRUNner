using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetWorkforceFilledJobRatio : ReactiveObject
    {
        [Reactive] public double Pioneers { get; private set; }
        [Reactive] public double Settlers { get; private set; }
        [Reactive] public double Technicians { get; private set; }
        [Reactive] public double Engineers { get; private set; }
        [Reactive] public double Scientists { get; private set; }

        public void Recalculate(PlanetWorkforce capacity, PlanetWorkforce required)
        {
            Pioneers = CalculateWorkforceHabitationPercentage(capacity.Pioneers, required.Pioneers);
            Settlers = CalculateWorkforceHabitationPercentage(capacity.Settlers, required.Settlers);
            Technicians = CalculateWorkforceHabitationPercentage(capacity.Technicians, required.Technicians);
            Engineers = CalculateWorkforceHabitationPercentage(capacity.Engineers, required.Engineers);
            Scientists = CalculateWorkforceHabitationPercentage(capacity.Scientists, required.Scientists);
        }
        
        private static double CalculateWorkforceHabitationPercentage(double capacity, double required)
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