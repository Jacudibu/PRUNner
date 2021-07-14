using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using NLog;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCart : ReactiveObject
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly PlanetaryBase _planetaryBase;

        public List<ShoppingCartBuilding> Buildings { get; } = new();
        public List<ShoppingCartMaterial> Materials { get; private set; } = new();

        private bool _ignoreUpdatesDueToInitialization;
        
        public ShoppingCart()
        {
            _planetaryBase = null!;
            Logger.Warn("Empty ShoppingCart constructor called! This only exists for the editor previews and is likely to break something.");
        }
        
        public ShoppingCart(PlanetaryBase planetaryBase)
        {
            _planetaryBase = planetaryBase;
        }

        public void UpdateBuildings()
        {
            _ignoreUpdatesDueToInitialization = true;
            
            foreach (var building in Buildings)
            {
                building.TotalAmount = 0;
            }

            var allMaterials = CollectAllRequiredMaterials(_planetaryBase);
            
            foreach (var building in _planetaryBase.InfrastructureBuildings.All)
            {
                AddOrUpdateBuilding(building, building.Amount, allMaterials);
            }
            
            foreach (var building in _planetaryBase.ProductionBuildings)
            {
                AddOrUpdateBuilding(building, building.Amount, allMaterials);
            }
            
            Buildings.RemoveMany(Buildings.Where(x => x.TotalAmount == 0));
            
            SetupMaterials(allMaterials);

            _ignoreUpdatesDueToInitialization = false;
            SumTotalMaterials();
            
            this.RaisePropertyChanged(nameof(Buildings));
            this.RaisePropertyChanged(nameof(Materials));
        }

        private void SetupMaterials(List<MaterialData> allMaterials)
        {
            List<ShoppingCartMaterial> newMats = new();
            
            foreach (var material in allMaterials)
            {
                var element = Materials.SingleOrDefault(x => x.Material == material);
                if (element == null)
                {
                    element = new ShoppingCartMaterial(material);
                    newMats.Add(element);
                }
                else
                {
                    newMats.Add(element);
                }
            }

            Materials = newMats;
        }

        private void SumTotalMaterials()
        {
            if (_ignoreUpdatesDueToInitialization)
            {
                return;
            }
            
            foreach (var material in Materials)
            {
                material.TotalAmount = 0;
            }
            
            foreach (var building in Buildings)
            {
                for (var i = 0; i < building.RequiredMaterials.Count; i++)
                {
                    Materials[i].TotalAmount += building.RequiredMaterials[i].Amount * building.PlannedAmount;
                }
            }
        }

        private static List<MaterialData> CollectAllRequiredMaterials(PlanetaryBase planetaryBase)
        {
            return planetaryBase.InfrastructureBuildings.All
                    .Where(x => x.Amount > 0)
                    .SelectMany(x => x.BuildingMaterials)
                .Concat(planetaryBase.ProductionBuildings
                    .Where(x => x.Amount > 0)
                    .SelectMany(x => x.BuildingMaterials))
                .Select(x => x.Material)
                .Distinct()
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Ticker)
                .ToList();
        }

        private void AddOrUpdateBuilding(PlanetBuilding building, int amount, List<MaterialData> allMaterials)
        {
            if (amount <= 0)
            {
                return;
            }
            
            var element = Buildings.SingleOrDefault(x => x.Building.Building == building.Building);
            if (element == null)
            {
                element = new ShoppingCartBuilding(building);
                element.WhenPropertyChanged(x => x.PlannedAmount).Subscribe(_ => SumTotalMaterials());
                Buildings.Add(element);
            }

            element.TotalAmount = amount;
            element.SetupRequiredMaterials(allMaterials);
        }
    }
}