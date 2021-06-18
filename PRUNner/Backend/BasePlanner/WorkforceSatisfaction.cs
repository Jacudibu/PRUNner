using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class WorkforceSatisfaction : ReactiveObject
    {
        [Reactive] public double Pioneers { get; private set; }
        [Reactive] public double Settlers { get; private set; }
        [Reactive] public double Technicians { get; private set; }
        [Reactive] public double Engineers { get; private set; }
        [Reactive] public double Scientists { get; private set; }
        
        public void Recalculate(ProvidedConsumables consumables, PlanetWorkforceFilledJobRatio filledJobRatio)
        {
            Pioneers = filledJobRatio.Pioneers * WorkforceSatisfactionFactors.Pioneers.Calculate(consumables);
            Settlers = filledJobRatio.Settlers *  WorkforceSatisfactionFactors.Settlers.Calculate(consumables);
            Technicians = filledJobRatio.Technicians *  WorkforceSatisfactionFactors.Technicians.Calculate(consumables);
            Engineers = filledJobRatio.Engineers * WorkforceSatisfactionFactors.Engineers.Calculate(consumables);
            Scientists = filledJobRatio.Scientists * WorkforceSatisfactionFactors.Scientists.Calculate(consumables);
        }
    }
}