using System;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class AdvancedBuildingConfiguration : ReactiveObject
    {
        private double _productionLineCondition = 100;
        private int _productionLineAge = 0;

        public double ProductionLineCondition
        {
            get => _productionLineCondition;
            set
            {
                this.RaiseAndSetIfChanged(ref _productionLineCondition, value, nameof(ProductionLineCondition));
                RecalculateAgeBasedOnCondition();
            }
        }

        public int ProductionLineAge
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

        public bool IsAnyAdvancedConfigurationPresent => UseEfficiencyOverride || _productionLineAge != 0;

        public AdvancedBuildingConfiguration()
        {
            this.WhenPropertyChanged(x => x.UseEfficiencyOverride).Subscribe(_ => this.RaisePropertyChanged(nameof(IsAnyAdvancedConfigurationPresent)));
            this.WhenPropertyChanged(x => x.ProductionLineAge).Subscribe(_ => this.RaisePropertyChanged(nameof(IsAnyAdvancedConfigurationPresent)));
        }
        
        private const double C = 100.87;
        private void RecalculateConditionBasedOnAge()
        {
            _productionLineCondition = _productionLineAge switch
            {
                <= 0 => 100,
                >= 200 => 1d / 3d,
                _ => 100 * ((2d / 3d) / (1 + Math.Exp(0.07156 * (_productionLineAge - C))) + (1d / 3d))
            };

            this.RaisePropertyChanged(nameof(ProductionLineCondition));
        }

        private void RecalculateAgeBasedOnCondition()
        {
            // TODO, that way people could figure out how many days it would take to reach a certain condition
        }
    }
}