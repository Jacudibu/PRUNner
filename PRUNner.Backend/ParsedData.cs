using System.Collections.Generic;
using FIOImport;
using PRUNner.Backend.Data;

namespace PRUNner.Backend
{
    public class ParsedData
    {
        public Dictionary<string, MaterialData> AllMaterials = MaterialData.AllItems;
        public Dictionary<string, PlanetData> AllPlanets = PlanetData.AllItems;
        public Dictionary<string, SystemData> AllSystems = SystemData.AllItems;

        internal ParsedData(RawData rawData)
        {
            foreach (var material in rawData.AllMaterials)
            {
                MaterialData.CreateFrom(material);
            }

            foreach (var planet in rawData.AllPlanets)
            {
                PlanetData.CreateFrom(planet);
            }

            foreach (var system in rawData.AllSystems)
            {
                SystemData.CreateFrom(system);
            }
            
            MaterialData.PostProcessData(rawData.AllMaterials);
            PlanetData.PostProcessData(rawData.AllPlanets);
            SystemData.PostProcessData(rawData.AllSystems);
        }
        
        public static ParsedData LoadAndParseFromCache()
        {
            var rawData = FioImporter.LoadAllFromCache();
            return new ParsedData(rawData);
        }
    }
}