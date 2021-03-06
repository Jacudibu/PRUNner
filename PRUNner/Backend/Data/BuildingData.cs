using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FIOImport.POCOs.Buildings;
using PRUNner.Backend.Data.BaseClasses;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Backend.Data
{
    public class BuildingData : GameData<BuildingData, FioBuilding>
    {
        public ImmutableArray<MaterialIO> BuildingCosts { get; private set; }
        public ImmutableArray<ProductionData> Production { get; private set; }

        public string Name { get; private set; } = null!;
        public string Ticker { get; private set; } = null!;
        public bool AffectedByFertility { get; private set; }
        public IndustryType Expertise { get; private set; }
        public BuildingCategory Category { get; private set; }
        public int AreaCost { get; private set; }
        public BuildingWorkforce Workforce { get; private set; } = null!;
        public BuildingWorkforceRatio WorkforceRatio { get; private set; } = null!;
        public BuildingWorkforce AdditionalWorkforceSpace { get; private set; } = null!;

        internal override string GetIdFromPoco(FioBuilding poco) => poco.Ticker;
        internal override string GetFioIdFromPoco(FioBuilding poco) => poco.Ticker;

        internal override void PostProcessData(FioBuilding poco)
        {
            Name = poco.Name;
            Ticker = poco.Ticker;
            Category = DetermineCategory(poco);
            
            Workforce = new BuildingWorkforce(poco);
            WorkforceRatio = new BuildingWorkforceRatio(Workforce);
            AdditionalWorkforceSpace = DetermineAdditionalWorkforceSpace();

            AreaCost = poco.AreaCost;
            if (poco.Expertise != null)
            {
                Expertise = Enum.Parse<IndustryType>(poco.Expertise.Replace("_", string.Empty), true);
            }

            AffectedByFertility = Ticker.Equals(Names.Buildings.FRM) || Ticker.Equals(Names.Buildings.ORC);
            BuildingCosts = poco.BuildingCosts.Select(x => new MaterialIO(x)).ToImmutableArray();
            Production = poco.Recipes.Select(x => new ProductionData(this, x))
                .OrderBy(x => x.Outputs.FirstOrDefault()?.Material.Ticker ?? x.ToString()).ToImmutableArray();
        }

        private BuildingWorkforce DetermineAdditionalWorkforceSpace()
        {
            if (Category != BuildingCategory.Infrastructure)
            {
                return BuildingWorkforce.Zero;
            }

            return Ticker switch
            {
                Names.Buildings.HB1 => new BuildingWorkforce(100, 0, 0, 0, 0),
                Names.Buildings.HB2 => new BuildingWorkforce(0, 100, 0, 0, 0),
                Names.Buildings.HB3 => new BuildingWorkforce(0, 0, 100, 0, 0),
                Names.Buildings.HB4 => new BuildingWorkforce(0, 0, 0, 100, 0),
                Names.Buildings.HB5 => new BuildingWorkforce(0, 0, 0, 0, 100),
                
                Names.Buildings.HBB => new BuildingWorkforce(75, 75, 0, 0, 0),
                Names.Buildings.HBC => new BuildingWorkforce(0, 75, 75, 0, 0),
                Names.Buildings.HBM => new BuildingWorkforce(0, 0, 75, 75, 0),
                Names.Buildings.HBL => new BuildingWorkforce(0, 0, 0, 75, 75),
                
                _ => BuildingWorkforce.Zero
            };
        }

        private BuildingCategory DetermineCategory(FioBuilding poco)
        {
            if (poco.AreaCost == 0)
            {
                return (BuildingCategory) (-1);
            }

            if (poco.Scientists > 0) return BuildingCategory.Scientists;
            if (poco.Engineers > 0) return BuildingCategory.Engineers;
            if (poco.Technicians > 0) return BuildingCategory.Technicians;
            if (poco.Settlers > 0) return BuildingCategory.Settlers;

            if (poco.Ticker.Equals(Names.Buildings.COL) 
                || poco.Ticker.Equals(Names.Buildings.EXT) 
                || poco.Ticker.Equals(Names.Buildings.RIG))
            {
                return BuildingCategory.Resources;
            }
            
            if (poco.Pioneers > 0) return BuildingCategory.Pioneers;

            return BuildingCategory.Infrastructure;
        }

        public ImmutableArray<MaterialIO> GetBuildingMaterialsOnPlanet(PlanetData planet)
        {
            var result = new List<MaterialIO>(BuildingCosts);
            switch (planet.Type)
            {
                case PlanetType.Rocky:
                    result.Add(GetMaterialIO(Names.Materials.MCG, AreaCost * 4));
                    break;
                case PlanetType.Gaseous:
                    result.Add(GetMaterialIO(Names.Materials.AEF, (int) Math.Ceiling(AreaCost / 3d)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(planet.Type), planet.Type, null);
            }
            
            if (planet.IsLowGravity()) result.Add(GetMaterialIO(Names.Materials.MGC, 1));
            if (planet.IsHighGravity()) result.Add(GetMaterialIO(Names.Materials.BL, 1));
            if (planet.IsLowPressure()) result.Add(GetMaterialIO(Names.Materials.SEA, AreaCost * 1));
            if (planet.IsHighPressure()) result.Add(GetMaterialIO(Names.Materials.HSE, 1));
            if (planet.IsLowTemperature()) result.Add(GetMaterialIO(Names.Materials.INS, AreaCost * 10));
            if (planet.IsHighTemperature()) result.Add(GetMaterialIO(Names.Materials.TSH, 1));

            return result.ToImmutableArray();
        }

        private MaterialIO GetMaterialIO(string ticker, int amount)
        {
            return new(MaterialData.GetOrThrow(ticker), amount, Constants.MsPerDay * Constants.DaysUntilAllBuildingMaterialsAreLost);
        }
    }
}