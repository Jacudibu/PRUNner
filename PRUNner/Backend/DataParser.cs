using FIOImport;
using FIOImport.Pocos;
using NLog;
using PRUNner.Backend.Data;

namespace PRUNner.Backend
{
    public static class DataParser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static void LoadAndParseFromCache()
        {
            var rawData = FioImporter.LoadAllFromCache();

            Logger.Info("Parsing Materials");
            foreach (var material in rawData.AllMaterials)
            {
                MaterialData.CreateFrom(material);
            }

            Logger.Info("Parsing Planets");
            foreach (var planet in rawData.AllPlanets)
            {
                PlanetData.CreateFrom(planet);
            }

            Logger.Info("Parsing Systems");
            foreach (var system in rawData.AllSystems)
            {
                SystemData.CreateFrom(system);
            }

            Logger.Info("Parsing Buildings");
            foreach (var building in rawData.AllBuildings)
            {
                BuildingData.CreateFrom(building);
            }
            
            MaterialData.PostProcessData(rawData.AllMaterials);
            PlanetData.PostProcessData(rawData.AllPlanets);
            SystemData.PostProcessData(rawData.AllSystems);
            BuildingData.PostProcessData(rawData.AllBuildings);
            
            LoadPriceData(rawData.RainPrices);
        }

        public static void UpdatePriceData()
        {
            LoadPriceData(FioImporter.DownloadPrices());
            Logger.Info("Price data has been updated!");
        }
        
        private static void LoadPriceData(FioRainPrices[] fioPrices)
        {
            foreach (var price in fioPrices)
            {
                MaterialData.Get(price.Ticker)?.PriceData.Update(price);
            }
        }
    }
}