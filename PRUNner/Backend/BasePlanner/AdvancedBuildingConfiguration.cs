using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class AdvancedBuildingConfiguration : ReactiveObject
    {
        [Reactive] public double ProductionLineCondition { get; set; } = 100;
        [Reactive] public bool UseEfficiencyOverride { get; set; }
        [Reactive] public double EfficiencyOverride { get; set; } = 100;
    }
}