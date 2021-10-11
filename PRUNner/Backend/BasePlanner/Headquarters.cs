using System;
using System.Collections.Generic;
using System.Linq;
using PRUNner.Backend.Data.Enums;
using PRUNner.Backend.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class Headquarters : ReactiveObject
    {
        [Reactive] public int UsedHQSlots { get; set; } = 1;
        [Reactive] public int TotalHQSlots { get; set; } = 2;

        [Reactive] public Faction Faction { get; set; } = Faction.None;

        // TODO: This feels terribly hacky but I have no clue how else to do it right now... :D
        public List<Faction> AvailableFactions { get; } = Enum.GetValues<Faction>().ToList();

        public double GetFactionEfficiencyFactorForIndustry(IndustryType industryType)
        {
            var baseValue = industryType switch
            {
                IndustryType.Agriculture => Faction == Faction.Hortus ? 0.03 : 0,
                IndustryType.Chemistry => Faction == Faction.OutsideRegion ? 0.02 : 0,
                IndustryType.Construction => Faction == Faction.Moria ? 0.03 : 0,
                IndustryType.Electronics => Faction == Faction.Antares ? 0.05 : 0,
                IndustryType.FoodIndustries => Faction == Faction.Hortus ? 0.02 : 0,
                IndustryType.FuelRefining => Faction == Faction.OutsideRegion ? 0.02 : 0,
                IndustryType.Manufacturing => Faction == Faction.Benten ? 0.05 : 0,
                IndustryType.Metallurgy => Faction == Faction.Moria ? 0.02 : 0,
                IndustryType.ResourceExtraction => Faction == Faction.OutsideRegion ? 0.02 : 0,
                _ => throw new ArgumentOutOfRangeException(nameof(industryType), industryType, null)
            };

            var factor = -2 * ((double)UsedHQSlots / TotalHQSlots) + 3;
            return baseValue * factor;
        }
    }
}