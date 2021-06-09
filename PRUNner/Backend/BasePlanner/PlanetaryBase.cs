using System;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetaryBase : ReactiveObject
    {
        public PlanetData Planet { get; }

        public PlanetaryBaseInfrastructure InfrastructureBuildings { get; } = new();
        public ObservableCollection<PlanetBuilding> ProductionBuildings { get; } = new();

        public Headquarters Headquarters { get; } = new();
        public ExpertAllocation ExpertAllocation { get; } = new();
        public PlanetWorkforce WorkforceRequired { get; } = new();
        public PlanetWorkforce WorkforceCapacity { get; } = new();
        public PlanetWorkforce WorkforceRemaining { get; } = new();
        public WorkforceSatisfaction WorkforceSatisfaction { get; } = new();
        public PlanetProductionTable ProductionTable { get; } = new();
        
        public ProvidedConsumables ProvidedConsumables { get; } = new();
        
        public int AreaTotal { get; } = Constants.BaseArea;
        [Reactive] public int AreaDeveloped { get; private set; }
        [Reactive] public int AreaAvailable { get; private set; } = Constants.BaseArea;

        public PlanetaryBase(PlanetData planet)
        {
            Planet = planet;
            foreach (var infrastructureBuilding in InfrastructureBuildings.All)
            {
                infrastructureBuilding.Changed.Subscribe(_ => OnBuildingChange());
            }

            ProvidedConsumables.Changed.Subscribe(_ =>
            {
                WorkforceSatisfaction.Recalculate(ProvidedConsumables, WorkforceCapacity, WorkforceRequired);
            });

            WorkforceRemaining.Changed.Subscribe(_ =>
            {
                WorkforceSatisfaction.Recalculate(ProvidedConsumables, WorkforceCapacity, WorkforceRequired);
            });
            
            WorkforceSatisfaction.Recalculate(ProvidedConsumables, WorkforceCapacity, WorkforceRequired);

            WorkforceSatisfaction.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            Headquarters.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            
            // TODO: Figure out why this here does not work, then use Observable.Merge instead of subscribing a dozen times.
            // ExpertAllocation.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Agriculture.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Chemistry.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Construction.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Electronics.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.FoodIndustries.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.FuelRefining.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Manufacturing.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.Metallurgy.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            ExpertAllocation.ResourceExtraction.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            
            OnBuildingChange();
        }

        private void RecalculateBuildingEfficiencies()
        {
            foreach (var building in ProductionBuildings)
            {
                building.UpdateProductionEfficiency(WorkforceSatisfaction, ExpertAllocation, Headquarters);
            }
            
            ProductionTable.Update(ProductionBuildings);
        }

        public void AddBuilding(BuildingData building)
        {
            var addedBuilding = ProductionBuildings.SingleOrDefault(x => x.Building == building);
            if (addedBuilding == null)
            {
                addedBuilding = PlanetBuilding.FromProductionBuilding(Planet, building);
                addedBuilding.Changed.Subscribe(_ => OnBuildingChange());
                ProductionBuildings.Add(addedBuilding);
            }

            addedBuilding.Amount++;
        }

        private void OnBuildingChange()
        {
            RecalculateWorkforce();
            RecalculateSpace();
            ProductionTable.Update(ProductionBuildings);
        }

        private void RecalculateWorkforce()
        {
            WorkforceRequired.Reset();
            foreach (var building in ProductionBuildings)
            {
                WorkforceRequired.Add(building.Building.Workforce, building.Amount);
            }

            WorkforceCapacity.Reset();
            foreach (var building in InfrastructureBuildings.All)
            {
                WorkforceCapacity.Add(building.Building.AdditionalWorkforceSpace, building.Amount);
            }

            WorkforceRemaining.SetRemainingWorkforce(WorkforceRequired, WorkforceCapacity);
        }

        private void RecalculateSpace()
        {
            var usedArea = ProductionBuildings.Sum(x => x.CalculateNeededArea())
                + InfrastructureBuildings.All.Sum(x => x.CalculateNeededArea());
            
            AreaDeveloped = usedArea;
            AreaAvailable = AreaTotal - usedArea;
        }
    }
}