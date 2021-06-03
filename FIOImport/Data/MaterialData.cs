using System;
using System.Collections.Generic;
using FIOImport.POCOs;

namespace FIOImport.Data
{
    public enum MaterialCategory
    {
        AgriculturalProducts,
        Alloys,
        Chemicals,
        ConstructionMaterials,
        ConstructionParts,
        ConstructionPrefabs,
        ConsumablesBasic,
        ConsumablesLuxury,
        Drones,
        ElectronicDevices,
        ElectronicParts,
        ElectronicPieces,
        ElectronicSystems,
        Elements,
        EnergySystems,
        Fuels,
        Gases,
        Liquids,
        MedicalEquipment,
        Metals,
        Minerals,
        Ores,
        Plastics,
        ShipEngines,
        ShipKits,
        ShipParts,
        ShipShields,
        SoftwareComponents,
        SoftwareSystems,
        SoftwareTools,
        Textiles,
        UnitPrefabs,
        Utility
    }
    
    public static class MaterialCategoryStrings
    {
        public const string AgriculturalProducts = "agricultural products";
        public const string Alloys = "alloys";
        public const string Chemicals = "chemicals";
        public const string ConstructionMaterials = "construction materials";
        public const string ConstructionParts = "construction parts";
        public const string ConstructionPrefabs = "construction prefabs";
        public const string ConsumablesBasic = "consumables (basic)";
        public const string ConsumablesLuxury = "consumables (luxury)";
        public const string Drones = "drones";
        public const string ElectronicDevices = "electronic devices";
        public const string ElectronicParts = "electronic parts";
        public const string ElectronicPieces = "electronic pieces";
        public const string ElectronicSystems = "electronic systems";
        public const string Elements = "elements";
        public const string EnergySystems = "energy systems";
        public const string Fuels = "fuels";
        public const string Gases = "gases";
        public const string Liquids = "liquids";
        public const string MedicalEquipment = "medical equipment";
        public const string Metals = "metals";
        public const string Minerals = "minerals";
        public const string Ores = "ores";
        public const string Plastics = "plastics";
        public const string ShipEngines = "ship engines";
        public const string ShipKits = "ship kits";
        public const string ShipParts = "ship parts";
        public const string ShipShields = "ship shields";
        public const string SoftwareComponents = "software components";
        public const string SoftwareSystems = "software systems";
        public const string SoftwareTools = "software tools";
        public const string Textiles = "textiles";
        public const string UnitPrefabs = "unit prefabs";
        public const string Utility = "utility";
    }
    
    public class MaterialData
    {
        public static Dictionary<string, MaterialData> AllMaterials = new Dictionary<string, MaterialData>();
        
        public readonly MaterialCategory Category;
        public readonly string Name;
        public readonly string Ticker;
        public readonly double Weight;
        public readonly double Volume;

        private MaterialData(Material material)
        {
            Category = Enum.Parse<MaterialCategory>(SanitizeCategoryString(material.CategoryName), true);
            Name = material.Name;
            Ticker = material.Ticker;
            Weight = material.Weight;
            Volume = material.Volume;
        }

        internal static MaterialData CreateFrom(Material material)
        {
            var result = new MaterialData(material);
            AllMaterials[result.Ticker] = result;
            return result;
        }

        private static string SanitizeCategoryString(string category)
        {
            return category
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty);
        }
    }
}