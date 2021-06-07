using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend.Data;
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

        public ObservableCollection<PlanetBuildingProduction> Production { get; } = new();
        
        public PlanetBuilding() // This feels like a hack, but otherwise we can't set the Design.DataContext in BuildingRow...
        {
            Building = null!;
        }
        
        public PlanetBuilding(BuildingData building)
        {
            Building = building;

            if (IsProductionBuilding)
            {
                AddProduction();
            }
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
            var recipe = Building.Production.First();
            var production = new PlanetBuildingProduction();
            production.ActiveRecipe = recipe;
            Production.Add(production);
        }
    }
}