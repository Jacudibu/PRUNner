using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        public PlanetaryBase PlanetaryBase { get; }
        
        [Reactive] public int Amount { get; set; }
        private readonly double _fertilityBonus;

        public ObservableCollection<PlanetBuildingProductionElement> AvailableRecipes { get; }
        
        public ObservableCollection<PlanetBuildingProductionQueueElement> Production { get; } = new();
        
        public PlanetBuilding() // This feels like a hack, but otherwise we can't set the Design.DataContext in BuildingRow...
        {
            Building = null!;
            PlanetaryBase = null!;
            AvailableRecipes = null!;
            _fertilityBonus = 0;
        }

        public static PlanetBuilding FromInfrastructureBuilding(PlanetaryBase planetaryBase, BuildingData building)
        {
            return new(planetaryBase, building);
        }
        
        public static PlanetBuilding FromProductionBuilding(PlanetaryBase planetaryBase, PlanetData planet, BuildingData building)
        {
            return new(planetaryBase, planet, building);
        }
        
        private PlanetBuilding(PlanetaryBase planetaryBase, BuildingData building)
        {
            PlanetaryBase = planetaryBase;
            Building = building;
            AvailableRecipes = null!;
        }
        
        private PlanetBuilding(PlanetaryBase planetaryBase, PlanetData planet, BuildingData building) : this(planetaryBase, building)
        {
            _fertilityBonus = building.AffectedByFertility ? planet.Fertility * 0.3 : 0;
            
            if (Building.Category == BuildingCategory.Resources)
            {
                AvailableRecipes = GetResourceRecipeList(planet, building);
            }
            else
            {
                AvailableRecipes = new ObservableCollection<PlanetBuildingProductionElement>(Building.Production.Select(x =>  new PlanetBuildingProductionElement(x)));
            }
            
            AddProduction();
        }

        private ObservableCollection<PlanetBuildingProductionElement> GetResourceRecipeList(PlanetData planet, BuildingData building)
        {
            IEnumerable<ResourceData> resources;
            switch (building.Ticker)
            {
                case Names.Buildings.COL:
                    resources = planet.Resources.Where(x => x.ResourceType == ResourceType.Gaseous);
                    break;
                case Names.Buildings.EXT:
                    resources = planet.Resources.Where(x => x.ResourceType == ResourceType.Mineral);
                    break;
                case Names.Buildings.RIG:
                    resources = planet.Resources.Where(x => x.ResourceType == ResourceType.Liquid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(building.Ticker), building.Ticker, null);
            }

             return new ObservableCollection<PlanetBuildingProductionElement>(resources.Select(x => new PlanetBuildingProductionElement(x)));
        }

        public int CalculateNeededArea()
        {
            return Building.AreaCost * Amount;
        }

        public delegate void OnProductionUpdateEvent();
        public event OnProductionUpdateEvent? OnProductionUpdate;
        
        public PlanetBuildingProductionQueueElement AddProduction()
        {
            var production = new PlanetBuildingProductionQueueElement(this);
            Production.Add(production);
            production.Changed.Subscribe(_ => OnProductionUpdate?.Invoke());
            return production;
        }

        public void RemoveProduction(PlanetBuildingProductionQueueElement element)
        {
            Production.Remove(element);
            OnProductionUpdate?.Invoke();
        }

        public void UpdateProductionEfficiency(WorkforceSatisfaction workforceSatisfaction, ExpertAllocation expertAllocation, Headquarters hq)
        {
            var expertBonus = expertAllocation.GetEfficiencyBonus(Building.Expertise);
            var hqBonus = hq.GetFactionEfficiencyFactorForIndustry(Building.Expertise);
            var satisfaction = 0d;
            satisfaction += workforceSatisfaction.Pioneers * Building.WorkforceRatio.Pioneers;
            satisfaction += workforceSatisfaction.Settlers * Building.WorkforceRatio.Settlers;
            satisfaction += workforceSatisfaction.Technicians * Building.WorkforceRatio.Technicians;
            satisfaction += workforceSatisfaction.Engineers * Building.WorkforceRatio.Engineers;
            satisfaction += workforceSatisfaction.Scientists * Building.WorkforceRatio.Scientists;
            
            foreach (var recipe in AvailableRecipes)
            {
                recipe.UpdateProductionEfficiency(satisfaction + expertBonus + hqBonus + _fertilityBonus);
            }
        }
    }   
}