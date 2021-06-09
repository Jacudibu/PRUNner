using FIOImport;
using PRUNner.Backend.Data;

namespace PRUNner.Backend
{
    public static class DataParser
    {
        public static void LoadAndParseFromCache()
        {
            var rawData = FioImporter.LoadAllFromCache();
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

            foreach (var building in rawData.AllBuildings)
            {
                BuildingData.CreateFrom(building);
            }
            
            MaterialData.PostProcessData(rawData.AllMaterials);
            PlanetData.PostProcessData(rawData.AllPlanets);
            SystemData.PostProcessData(rawData.AllSystems);
            BuildingData.PostProcessData(rawData.AllBuildings);
        }
    }
}