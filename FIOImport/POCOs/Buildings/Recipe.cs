// ReSharper disable All
using System.Collections.Generic;

namespace FIOImport.POCOs.Buildings
{
    public class Recipe
    {
        public List<Input> Inputs { get; set; }
        public List<Output> Outputs { get; set; }
        public int DurationMs { get; set; }
        public string RecipeName { get; set; }
    }
}