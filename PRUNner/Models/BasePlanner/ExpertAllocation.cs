using System;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class Expert : ReactiveObject
    {
        public IndustryType Expertise { get; }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if (value is < 0 or > 5) 
                {
                    return;
                }

                this.RaiseAndSetIfChanged(ref _count, value);
                RecalculateEfficiencyGain();
            }
        }

        [Reactive] public double EfficiencyGain { get; private set; }

        public Expert(IndustryType expertise)
        {
            Expertise = expertise;
            Count = 0;
            EfficiencyGain = 0;

            this.WhenAnyValue(x => x.Count).Subscribe(_ => RecalculateEfficiencyGain());
        }

        public Expert() { }

        private void RecalculateEfficiencyGain()
        {
            EfficiencyGain = Count switch
            {
                0 => 0,
                1 => 0.036,
                2 => 0.0696,
                3 => 0.1248,
                4 => 0.1974,
                _ => 0.2840,
            };
        }
    }
    
    public class ExpertAllocation
    {
        [Reactive] public Expert Agriculture { get; set; } = new(IndustryType.Agriculture); 
        [Reactive] public Expert Chemistry { get; set; } = new(IndustryType.Chemistry); 
        [Reactive] public Expert Construction { get; set; } = new(IndustryType.Construction); 
        [Reactive] public Expert Electronics { get; set; } = new(IndustryType.Electronics); 
        [Reactive] public Expert FoodIndustries { get; set; } = new(IndustryType.FoodIndustries); 
        [Reactive] public Expert FuelRefining { get; set; } = new Expert(IndustryType.FuelRefining); 
        [Reactive] public Expert Manufacturing { get; set; } = new(IndustryType.Manufacturing); 
        [Reactive] public Expert Metallurgy { get; set; } = new(IndustryType.Metallurgy); 
        [Reactive] public Expert ResourceExtraction { get; set; } = new(IndustryType.ResourceExtraction); 
    }
}