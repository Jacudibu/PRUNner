using System;
using System.Collections.Generic;
using FIOImport.POCOs;
using PRUNner.Backend.Data.BaseClasses;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Backend.Data
{
    public class MaterialData : GameData<MaterialData, FioMaterial>
    {
        public List<ProductionData> Usages { get; } = new();
        public List<ProductionData> Recipes { get; } = new();
        public MaterialCategory Category { get; private set; }
        public string Ticker { get; private set; }
        public string Name { get; private set; }
        public double Weight { get; private set; }
        public double Volume { get; private set; }

        internal override string GetIdFromPoco(FioMaterial poco) => poco.Ticker;
        internal override string GetFioIdFromPoco(FioMaterial poco) => poco.MatId;

        internal override void PostProcessData(FioMaterial poco)
        {
            Category = Enum.Parse<MaterialCategory>(SanitizeCategoryString(poco.CategoryName), true);
            Name = poco.Name;
            Ticker = poco.Ticker;
            Weight = poco.Weight;
            Volume = poco.Volume;
        }

        private static string SanitizeCategoryString(string category)
        {
            return category
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty);
        }

        public void AddUsage(ProductionData productionData)
        {
            Usages.Add(productionData);
        }

        public void AddRecipe(ProductionData productionData)
        {
            Recipes.Add(productionData);
        }
    }
}