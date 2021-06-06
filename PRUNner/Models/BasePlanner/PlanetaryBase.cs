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
        public ObservableCollection<PlannedBuilding> Buildings { get; } = new();
        
        public Workforce RequiredWorkforce { get; } = new();
        public Workforce AvailableWorkforce { get; } = new();

        public int TotalArea { get; } = Constants.BaseArea;
        [Reactive] public int UsedArea { get; private set; } = 0;
        [Reactive] public int RemainingArea { get; private set; } = Constants.BaseArea;

        public PlanetaryBase(PlanetData planet)
        {
            Planet = planet;
        }

        public void AddBuilding(BuildingData building)
        {
            var addedBuilding = Buildings.SingleOrDefault(x => x.Building == building);
            if (addedBuilding == null)
            {
                addedBuilding = new PlannedBuilding(building);
                addedBuilding.WhenAnyValue(x => x.Amount).Subscribe(_ => OnBuildingChange());
                Buildings.Add(addedBuilding);
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
            RequiredWorkforce.Reset();
            foreach (var building in Buildings)
            {
                RequiredWorkforce.Add(building);
            }
        }

        private void RecalculateSpace()
        {
            var usedArea = 0;
            foreach (var building in Buildings)
            {
                usedArea += building.Building.AreaCost * building.Amount;
            }

            UsedArea = usedArea;
            RemainingArea = TotalArea - usedArea;
        }
    }
}