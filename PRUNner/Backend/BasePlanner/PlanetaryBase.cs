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
        private bool _loading;
        private readonly Empire _empire;
        public PlanetData Planet { get; }

        public PlanetaryBaseInfrastructure InfrastructureBuildings { get; }
        public ObservableCollection<PlanetBuilding> ProductionBuildings { get; } = new();

        public ExpertAllocation ExpertAllocation { get; } = new();
        public PlanetWorkforce WorkforceRequired { get; } = new();
        public PlanetWorkforce WorkforceCapacity { get; } = new();
        public PlanetWorkforce WorkforceRemaining { get; } = new();
        public WorkforceSatisfaction WorkforceSatisfaction { get; } = new();
        public PlanetProductionTable ProductionTable { get; }
        
        public ProvidedConsumables ProvidedConsumables { get; } = new();
        
        public int AreaTotal { get; } = Constants.BaseArea;
        [Reactive] public int AreaDeveloped { get; private set; }
        [Reactive] public int AreaAvailable { get; private set; } = Constants.BaseArea;
        [Reactive] public double NetProfit { get; private set; }

        public PlanetaryBase(Empire empire, PlanetData planet)
        {
            BeginLoading();

            InfrastructureBuildings = new PlanetaryBaseInfrastructure(this);
            
            _empire = empire;
            Planet = planet;
            ProductionTable = new PlanetProductionTable(this);
            foreach (var infrastructureBuilding in InfrastructureBuildings.All)
            {
                infrastructureBuilding.Changed.Subscribe(_ => OnBuildingChange());
            }

            ProvidedConsumables.Changed.Subscribe(_ =>
            {
                WorkforceSatisfaction.Recalculate(ProvidedConsumables, WorkforceCapacity, WorkforceRequired);
            });

            WorkforceSatisfaction.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            _empire.Headquarters.Changed.Subscribe(_ => RecalculateBuildingEfficiencies());
            
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
         
            FinishLoading();
        }

        private void RecalculateBuildingEfficiencies()
        {
            if (_loading)
            {
                return;
            }
            
            foreach (var building in ProductionBuildings)
            {
                building.UpdateProductionEfficiency(WorkforceSatisfaction, ExpertAllocation, _empire.Headquarters);
            }
            
            ProductionTable.Update(ProductionBuildings);
        }

        public PlanetBuilding AddBuilding(BuildingData building)
        {
            var addedBuilding = ProductionBuildings.SingleOrDefault(x => x.Building == building);
            if (addedBuilding == null)
            {
                addedBuilding = PlanetBuilding.FromProductionBuilding(this, Planet, building);
                addedBuilding.Changed.Subscribe(_ => OnBuildingChange());
                addedBuilding.OnProductionUpdate += OnProductionChange;
                ProductionBuildings.Add(addedBuilding);
            }

            addedBuilding.Amount++;
            return addedBuilding;
        }

        private void OnBuildingChange()
        {
            if (_loading)
            {
                return;
            }
            
            RecalculateWorkforce();
            RecalculateSpace();
            OnProductionChange();
        }

        private void OnProductionChange()
        {
            ProductionTable.Update(ProductionBuildings);
            NetProfit = ProductionTable.Rows.Sum(x => x.Value);
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
            WorkforceSatisfaction.Recalculate(ProvidedConsumables, WorkforceCapacity, WorkforceRequired);
        }

        private void RecalculateSpace()
        {
            var usedArea = ProductionBuildings.Sum(x => x.CalculateNeededArea())
                + InfrastructureBuildings.All.Sum(x => x.CalculateNeededArea());
            
            AreaDeveloped = usedArea;
            AreaAvailable = AreaTotal - usedArea;
        }

        public void BeginLoading()
        {
            _loading = true;
        }

        public void FinishLoading()
        {
            _loading = false;
            RecalculateWorkforce();
            RecalculateBuildingEfficiencies();
            RecalculateSpace();
            OnProductionChange();     
        }

        public void RemoveProductionBuilding(PlanetBuilding building)
        {
            ProductionBuildings.Remove(building);
        }

        public void OnPriceDataUpdate()
        {
            OnProductionChange();
        }
    }
}