using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class AdvancedBuildingConfiguration : ReactiveObject
    {
        private double _productionLineCondition = 100;
        private double _productionLineAge = 0;

        public double ProductionLineCondition
        {
            get => _productionLineCondition;
            set
            {
                this.RaiseAndSetIfChanged(ref _productionLineCondition, value, nameof(ProductionLineCondition));
                RecalculateAgeBasedOnCondition();
            }
        }

        public double ProductionLineAge
        {
            get => _productionLineAge;
            set
            {
                this.RaiseAndSetIfChanged(ref _productionLineAge, value, nameof(ProductionLineAge));
                RecalculateConditionBasedOnAge();
            }
        }

        [Reactive] public bool UseEfficiencyOverride { get; set; }
        [Reactive] public double EfficiencyOverride { get; set; } = 100;

        private const double C = 100.87;
        private void RecalculateConditionBasedOnAge()
        {
            _productionLineCondition = 100 * ((2d / 3d) / (1 + Math.Exp(0.07156 * (_productionLineAge - C))) + (1d / 3d));
            this.RaisePropertyChanged(nameof(ProductionLineCondition));
        }

        private void RecalculateAgeBasedOnCondition()
        {
            // TODO, that way people could figure out how many days it would take to reach a certain condition
        }
    }
}