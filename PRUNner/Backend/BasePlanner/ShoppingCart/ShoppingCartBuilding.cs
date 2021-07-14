using System.Collections.Generic;
using System.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCartBuilding : ReactiveObject
    {
        public PlanetBuilding Building { get; }

        public int TotalAmount { get; set; }
        [Reactive] public int PlannedAmount { get; set; }

        public List<MaterialIO> RequiredMaterials { get; set; } = new();
        
        public ShoppingCartBuilding(PlanetBuilding building)
        {
            Building = building;

        }

        public void SetupRequiredMaterials(List<MaterialData> allMaterials)
        {
            RequiredMaterials.Clear();
            foreach (var materialData in allMaterials)
            {
                var amount = Building.BuildingMaterials
                    .SingleOrDefault(x => x.Material.Equals(materialData))?.Amount ?? 0;
                
                RequiredMaterials.Add(new MaterialIO(materialData, amount, 0));
            }
        }
    }
}