using System.Collections.Immutable;
using System.Linq;
using FIOImport.POCOs.Buildings;

namespace PRUNner.Backend.Data.Components
{
    public class ProductionData
    {
        public BuildingData Building { get; }
        public ImmutableArray<MaterialIO> Inputs { get; }
        public ImmutableArray<MaterialIO> Outputs { get; }
        public long DurationInMilliseconds { get; }
        public string RecipeName { get; }

        public ProductionData(BuildingData building, FioRecipe poco)
        {
            Building = building;
            RecipeName = poco.RecipeName;
            Inputs = poco.Inputs.Select(x => new MaterialIO(x, poco.DurationMs)).ToImmutableArray();
            Outputs = poco.Outputs.Select(x => new MaterialIO(x, poco.DurationMs)).ToImmutableArray();
            DurationInMilliseconds = poco.DurationMs;
            
            foreach (var input in Inputs)
            {
                input.Material.AddUsage(this);
            }

            foreach (var output in Outputs)
            {
                output.Material.AddRecipe(this);
            }
        }
    }
}