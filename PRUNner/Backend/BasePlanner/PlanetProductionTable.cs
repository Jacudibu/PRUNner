using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Enums;
using ReactiveUI;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetProductionTable : ReactiveObject
    {
        public readonly PlanetaryBase PlanetaryBase;
        public PlanetProductionTable(PlanetaryBase planetaryBase)
        {
            PlanetaryBase = planetaryBase;
        }

        public ObservableCollection<PlanetProductionRow> Rows { get; } = new();
        public ObservableCollection<PlanetProductionRow> Inputs { get; } = new();
        public ObservableCollection<PlanetProductionRow> Outputs { get; } = new();

        public void Update(IEnumerable<PlanetBuilding> buildings)
        {
            Rows.Clear();
            Inputs.Clear();
            Outputs.Clear();
            
            WorkforceConsumptionFactors.Pioneers.AddUsedConsumables(PlanetaryBase.ProvidedConsumables, PlanetaryBase.WorkforceRequired.Pioneers, PlanetaryBase.WorkforceCapacity.Pioneers, AddInput);
            WorkforceConsumptionFactors.Settlers.AddUsedConsumables(PlanetaryBase.ProvidedConsumables, PlanetaryBase.WorkforceRequired.Settlers, PlanetaryBase.WorkforceCapacity.Settlers, AddInput);
            WorkforceConsumptionFactors.Technicians.AddUsedConsumables(PlanetaryBase.ProvidedConsumables, PlanetaryBase.WorkforceRequired.Technicians, PlanetaryBase.WorkforceCapacity.Technicians, AddInput);
            WorkforceConsumptionFactors.Engineers.AddUsedConsumables(PlanetaryBase.ProvidedConsumables, PlanetaryBase.WorkforceRequired.Engineers, PlanetaryBase.WorkforceCapacity.Engineers, AddInput);
            WorkforceConsumptionFactors.Scientists.AddUsedConsumables(PlanetaryBase.ProvidedConsumables, PlanetaryBase.WorkforceRequired.Scientists, PlanetaryBase.WorkforceCapacity.Scientists, AddInput);
            
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

            Sort();
            
            foreach (var row in Rows)
            {
                if (Math.Abs(row.Balance) < 0.1)
                {
                    continue;
                }

                if (row.Balance > 0)
                {
                    Outputs.Add(row);
                }
                else
                {
                    Inputs.Add(row);
                }
            }
            
            this.RaisePropertyChanged(nameof(Rows));
            this.RaisePropertyChanged(nameof(Outputs));
            this.RaisePropertyChanged(nameof(Inputs));
        }

        private double CalculateAmount(PlanetBuilding building, PlanetBuildingProductionQueueElement recipe, MaterialIO input)
        {
            return building.Amount * building.Efficiency * input.DailyAmount * recipe.Percentage;
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
            
            row = new PlanetProductionRow(this, material);
            Rows.Add(row);
            return row;
        }
        
        private string _currentSortMode = nameof(SortByCategory);
        private SortOrder _currentSortOrder = SortOrder.Ascending;
        private Func<PlanetProductionRow, object> _currentSortComparer = x => x.Material.Category + "/" + x.Material.Name;

        private void Sort()
        {
            List<PlanetProductionRow> result;
            result = _currentSortOrder == SortOrder.Ascending
                ? Rows.OrderBy(_currentSortComparer).ToList()
                : Rows.OrderByDescending(_currentSortComparer).ToList();
            Rows.Clear();
            Rows.AddRange(result);
        }

        private void Sort(string sortModeName, SortOrder defaultSortOrder, Func<PlanetProductionRow, object> comparer)
        {
            if (_currentSortMode.Equals(sortModeName))
            {
                _currentSortOrder = SortOrder.Descending;
                _currentSortMode = sortModeName + "Inverse";
            }
            else
            {
                _currentSortOrder = SortOrder.Ascending;
                _currentSortMode = sortModeName;
                _currentSortComparer = comparer;
            }
            Sort();
        }
        
        public void SortByTicker() => Sort(nameof(SortByTicker), SortOrder.Ascending, x => x.Material.Ticker);
        public void SortByCategory() => Sort(nameof(SortByCategory), SortOrder.Ascending, x => x.Material.Category + "/" + x.Material.Name);
        public void SortByInputs() => Sort(nameof(SortByInputs), SortOrder.Descending, x => x.Inputs);
        public void SortByOutputs() => Sort(nameof(SortByOutputs), SortOrder.Descending, x => x.Outputs);
        public void SortByBalance() => Sort(nameof(SortByBalance), SortOrder.Descending, x => x.Balance);
        public void SortByValue() => Sort(nameof(SortByValue), SortOrder.Descending, x => x.Value);

        public void UpdatePriceData()
        {
            foreach (var row in Rows)
            {
                row.UpdatePriceData();
            }
        }
    }
}
