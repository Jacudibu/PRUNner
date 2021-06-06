using System;
using System.Collections.Immutable;
using System.Linq;
using FIOImport.POCOs.Buildings;
using PRUNner.Backend.Data.BaseClasses;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Enums;

namespace PRUNner.Backend.Data
{
    public class BuildingData : GameData<BuildingData, FioBuilding>
    {
        public ImmutableArray<MaterialIO> BuildingCosts { get; private set; }
        public ImmutableArray<ProductionData> Production { get; private set; }

        public string Name { get; private set; } = null!;
        public string Ticker { get; private set; } = null!;
        public IndustryType Expertise { get; private set; }
        public int Pioneers { get; private set; }
        public int Settlers { get; private set; }
        public int Technicians { get; private set; }
        public int Engineers { get; private set; }
        public int Scientists { get; private set; }
        public int AreaCost { get; private set; }

        
        internal override string GetIdFromPoco(FioBuilding poco) => poco.Ticker;
        internal override string GetFioIdFromPoco(FioBuilding poco) => poco.Ticker;

        internal override void PostProcessData(FioBuilding poco)
        {
            Name = poco.Name;
            Ticker = poco.Ticker;
            Pioneers = poco.Pioneers;
            Settlers = poco.Settlers;
            Technicians = poco.Technicians;
            Engineers = poco.Engineers;
            Scientists = poco.Scientists;
            AreaCost = poco.AreaCost;
            if (poco.Expertise != null)
            {
                Expertise = Enum.Parse<IndustryType>(poco.Expertise.Replace("_", string.Empty), true);
            }

            BuildingCosts = poco.BuildingCosts.Select(x => new MaterialIO(x)).ToImmutableArray();
            Production = poco.Recipes.Select(x => new ProductionData(this, x)).ToImmutableArray();
        }
    }
}