using System;
using System.Collections.Generic;
// ReSharper disable All

namespace FIOImport.POCOs.Buildings
{
    public class FioBuilding
    {
        public List<FioBuildingCost> BuildingCosts { get; set; }
        public List<FioRecipe> Recipes { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Expertise { get; set; }
        public int Pioneers { get; set; }
        public int Settlers { get; set; }
        public int Technicians { get; set; }
        public int Engineers { get; set; }
        public int Scientists { get; set; }
        public int AreaCost { get; set; }
        public string UserNameSubmitted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}