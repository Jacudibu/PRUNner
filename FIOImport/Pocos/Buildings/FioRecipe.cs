// ReSharper disable All
using System.Collections.Generic;

namespace FIOImport.POCOs.Buildings
{
    public class FioRecipe
    {
        public List<FioInput> Inputs { get; set; }
        public List<FioOutput> Outputs { get; set; }
        public int DurationMs { get; set; }
        public string RecipeName { get; set; }
    }
}