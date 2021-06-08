using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuilding : ReactiveObject
    {
        public BuildingData Building { get; }
        
        [Reactive] public int Amount { get; set; }
        
        public bool IsProductionBuilding => Building.Category != BuildingCategory.Infrastructure;
        public ObservableCollection<PlanetBuildingProductionElement> AvailableRecipes { get; }
        
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
                AvailableRecipes = new ObservableCollection<PlanetBuildingProductionElement>(Building.Production.Select(x =>  new PlanetBuildingProductionElement(x)));
            }
            
            AddProduction();
        }

        private ObservableCollection<PlanetBuildingProductionElement> GetResourceRecipeList(PlanetData planetData, BuildingData building)
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

             return new ObservableCollection<PlanetBuildingProductionElement>(resources.Select(x => new PlanetBuildingProductionElement(x)));
        }

        public int CalculateNeededArea()
        {
            return Building.AreaCost * Amount;
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
                recipe.UpdateProductionEfficiency(satisfaction + expertBonus + hqBonus);
            }
        }
    }   
}