using System;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetaryBase : ReactiveObject
    {
        public PlanetData Planet { get; }

        public PlanetaryBaseInfrastructure InfrastructureBuildings { get; } = new();
        public ObservableCollection<PlanetBuilding> ProductionBuildings { get; } = new();

        public PlanetWorkforce WorkforceRequired { get; } = new();
        public PlanetWorkforce WorkforceCapacity { get; } = new();
        public PlanetWorkforce WorkforceCurrent { get; } = new();
        
        public int AreaTotal { get; } = Constants.BaseArea;
        [Reactive] public int AreaDeveloped { get; private set; } = 0;
        [Reactive] public int AreaAvailable { get; private set; } = Constants.BaseArea;

        public PlanetaryBase(PlanetData planet)
        {
            Planet = planet;
            foreach (var infrastructureBuilding in InfrastructureBuildings.All)
            {
                infrastructureBuilding.WhenAnyValue(x => x.Amount).Subscribe(_ => OnBuildingChange());
            }
            
            OnBuildingChange();
        }

        public void AddBuilding(BuildingData building)
        {
            var addedBuilding = ProductionBuildings.SingleOrDefault(x => x.Building == building);
            if (addedBuilding == null)
            {
                addedBuilding = new PlanetBuilding(building);
                addedBuilding.WhenAnyValue(x => x.Amount).Subscribe(_ => OnBuildingChange());
                ProductionBuildings.Add(addedBuilding);
            }

            addedBuilding.Amount++;
        }

        private void OnBuildingChange()
        {
            RecalculateWorkforce();
            RecalculateSpace();
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

            WorkforceCurrent.SetRemainingWorkforce(WorkforceRequired, WorkforceCapacity);
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