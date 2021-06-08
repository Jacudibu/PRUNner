using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuildingProductionElement : ReactiveObject
    {
        public List<MaterialIO> Inputs { get; }
        public List<MaterialIO> Outputs { get; }
        public string DurationString { get; private set; } = null!;
        
        private readonly long _baseDurationMs;

        public PlanetBuildingProductionElement(ProductionData productionData)
        {
            Inputs = productionData.Inputs.ToList();
            Outputs = productionData.Outputs.ToList();
            _baseDurationMs = productionData.DurationInMilliseconds;
            ParseDurationString();
        }

        public PlanetBuildingProductionElement(ResourceData resourceData)
        {
            Inputs = new List<MaterialIO>();
            Outputs = new List<MaterialIO>() {new(resourceData.Material, 1) };
            _baseDurationMs = 12 * 60 * 60 * 1000;
            ParseDurationString();
        }

        private void ParseDurationString()
        {
            var timespan = TimeSpan.FromMilliseconds(_baseDurationMs);

            var builder = new StringBuilder();
            if (timespan.Days > 0)
            {
                builder.Append(timespan.Days);
                builder.Append("d ");
            }
            
            if (timespan.Hours > 0)
            {
                builder.Append(timespan.Hours);
                builder.Append("h ");
            }
            
            if (timespan.Minutes > 0)
            {
                builder.Append(timespan.Minutes);
                builder.Append('m');
            }

            DurationString = builder.ToString();
        }
    }
    
    public class PlanetBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        
        [Reactive] public int Amount { get; set; }
        
        public bool IsProductionBuilding => Building.Category != BuildingCategory.Infrastructure;
        public List<PlanetBuildingProductionElement> AvailableRecipes { get; }
        
        public ObservableCollection<PlanetBuildingProductionQueueElement> Production { get; } = new();
        
        public PlanetBuilding() // This feels like a hack, but otherwise we can't set the Design.DataContext in BuildingRow...
        {
            Building = null!;
        }

        public static PlanetBuilding FromInfrastructureBuilding(BuildingData building)
        {
            return new(building);
        }
        
        public static PlanetBuilding FromProductionBuilding(PlanetData planet, BuildingData building)
        {
            return new(planet, building);
        }
        
        private PlanetBuilding(BuildingData building)
        {
            Building = building;
            AvailableRecipes = null!;
        }
        
        private PlanetBuilding(PlanetData planet, BuildingData building)
        {
            Building = building;
            
            if (Building.Category == BuildingCategory.Resources)
            {
                AvailableRecipes = GetResourceRecipeList(planet, building);
            }
            else
            {
                AvailableRecipes = Building.Production.Select(x =>  new PlanetBuildingProductionElement(x)).ToList();
            }
            
            AddProduction();
        }

        private List<PlanetBuildingProductionElement> GetResourceRecipeList(PlanetData planetData, BuildingData building)
        {
            IEnumerable<ResourceData> resources;
            switch (building.Ticker)
            {
                case Names.Buildings.COL:
                    resources = planetData.Resources.Where(x => x.ResourceType == ResourceType.Gaseous);
                    break;
                case Names.Buildings.EXT:
                    resources = planetData.Resources.Where(x => x.ResourceType == ResourceType.Mineral);
                    break;
                case Names.Buildings.RIG:
                    resources = planetData.Resources.Where(x => x.ResourceType == ResourceType.Liquid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(building.Ticker), building.Ticker, null);
            }

             return resources.Select(x => new PlanetBuildingProductionElement(x)).ToList();
        }

        public int CalculateNeededArea()
        {
            return Building.AreaCost * Amount;
        }
        
        public void Add()
        {
            Amount++;
        }

        public void Reduce()
        {
            if (Amount == 0)
            {
                return;
            }
            
            Amount--;
        }

        public void AddProduction()
        {
            var production = new PlanetBuildingProductionQueueElement(this);
            Production.Add(production);
        }

        public void RemoveProduction(PlanetBuildingProductionQueueElement element)
        {
            Production.Remove(element);
        }
    }
}