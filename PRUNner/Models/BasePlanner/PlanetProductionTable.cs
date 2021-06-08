using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using ReactiveUI;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetProductionTable : ReactiveObject
    {
        public ObservableCollection<PlanetProductionRow> Rows { get; } = new();

        public void Update(IEnumerable<PlanetBuilding> buildings)
        {
            Rows.Clear();
            
            foreach (var building in buildings)
            {
                foreach (var production in building.Production)
                {
                    if (production.ActiveRecipe == null)
                    {
                        // Happens with EXT/COL/RIG when the planet has no resource to extract
                        continue;
                    }
                    
                    foreach (var input in production.ActiveRecipe.Inputs)
                    {
                        var amount = CalculateAmount(building, production, input);
                        AddInput(input.Material, amount);
                    }
                    
                    foreach (var output in production.ActiveRecipe!.Outputs)
                    {
                        var amount = CalculateAmount(building, production, output);
                        AddOutput(output.Material, amount);
                    }
                }
            }
            
            this.RaisePropertyChanged(nameof(Rows));
        }

        private const long MsPerDay = 24 * 60 * 60 * 1000;
        private double CalculateAmount(PlanetBuilding building, PlanetBuildingProductionQueueElement recipe, MaterialIO input)
        {
            var dailyBaseValue = MsPerDay / recipe.ActiveRecipe!.DurationInMilliseconds;
            return building.Amount * input.Amount * dailyBaseValue * recipe.Percentage * 0.01;
        }

        private void AddInput(MaterialData material, double amount)
        {
            var row = GetRow(material);
            row.Inputs += amount;
        } 
        
        private void AddOutput(MaterialData material, double amount)
        {
            var row = GetRow(material);
            row.Outputs += amount;
        }

        private PlanetProductionRow GetRow(MaterialData material)
        {
            var row = Rows.SingleOrDefault(x => x.Material == material);
            if (row != null)
            {
                return row;
            }
            
            row = new PlanetProductionRow(material);
            Rows.Add(row);
            return row;
        }
    }
}