using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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

        public ObservableCollection<ShoppingCartBuilding> Buildings { get; } = new();
        public ObservableCollection<ShoppingCartMaterial> Materials { get; } = new();

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
                if (building.Amount > 0)
                {
                    AddOrUpdateBuilding(building, building.Amount, 0, allMaterials);
                }
            }
            
            foreach (var building in _planetaryBase.ProductionBuildings)
            {
                AddOrUpdateBuilding(building, building.Amount, 0, allMaterials);
            }

            SetupMaterials(allMaterials);

            _ignoreUpdatesDueToInitialization = false;
            SumTotalMaterials();
            
            this.RaisePropertyChanged(nameof(Buildings));
            this.RaisePropertyChanged(nameof(Materials));
        }

        public void AddBuilding(BuildingData? building, int quantity = 1)
        {
            if (building != null)
            {
                var allMaterials = CollectAllRequiredMaterials(_planetaryBase);
                var infrastructureBuilding = PlanetBuilding.FromInfrastructureBuilding(_planetaryBase, building!);
                AddOrUpdateBuilding(infrastructureBuilding, 0, 0, allMaterials);

                var element = Buildings.Single(e => e.Building.Building.Ticker == building.Ticker);
                element.PlannedAmount += quantity;
            }
        }

        private void SetupMaterials(List<MaterialData> allMaterials)
        {
            Materials.Clear();
            var newMats = Materials;
            
            foreach (var material in allMaterials)
            {
                var element = Materials.SingleOrDefault(x => x.Material == material);
                if (element == null)
                {
                    element = new ShoppingCartMaterial(material, _planetaryBase);
                    newMats.Add(element);
                }
                else
                {
                    newMats.Add(element);
                }
            }
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
                foreach(var requiredMaterial in building.RequiredMaterials)
                {
                    var material = Materials.Single(e => e.Material == requiredMaterial.Material);
                    material.TotalAmount += requiredMaterial.Amount * building.PlannedAmount;
                }
            }
        }

        private List<MaterialData> CollectAllRequiredMaterials(PlanetaryBase planetaryBase)
        {
            return Buildings
                .SelectMany(e => e.RequiredMaterials)
                .Select(e => e.Material)
                .Distinct()
                .OrderBy(e => e.Category)
                .ThenBy(e => e.Ticker)
                .ToList();
        }

        private void AddOrUpdateBuilding(PlanetBuilding building, int amount, int planned, List<MaterialData> allMaterials)
        {
            var materialAdded = false;

            var element = Buildings.SingleOrDefault(x => x.Building.Building == building.Building);
            if (element == null)
            {
                element = new ShoppingCartBuilding(building);
                element.WhenPropertyChanged(x => x.PlannedAmount).Subscribe(_ => SumTotalMaterials());
                Buildings.Add(element);

                foreach(var materialData in building.BuildingMaterials.Select(e => e.Material))
                {
                    if (!allMaterials.Contains(materialData))
                    {
                        allMaterials.Add(materialData);
                    }

                    var material = Materials.SingleOrDefault(e => e.Material.Ticker == materialData.Ticker);
                    if (material == null)
                    {
                        materialAdded = true;
                        Materials.Add(new ShoppingCartMaterial(materialData, _planetaryBase));
                    }
                }
            }

            element.TotalAmount = amount;
            element.PlannedAmount += planned;

            if (materialAdded)
            {
                foreach(var buildingElement in Buildings)
                {
                    buildingElement.SetupRequiredMaterials(allMaterials);
                }
            }
            else
            {
                element.SetupRequiredMaterials(allMaterials);
            }
        }
    }
}