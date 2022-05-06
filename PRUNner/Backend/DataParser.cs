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
            
            InitializePriceData(rawData.AllCommodityExchanges, rawData.AllExchangeData);
            UpdatePriceData();
        }

        public static void UpdatePriceData()
        {
            LoadPriceData(FioExchangeDataDownloader.Instance.DownloadAndCache());
            Logger.Info("Price data has been updated!");
        }
        
        private static void LoadPriceData(FioExchangeData[] allExchangeData)
        {
            foreach (var exchangeData in allExchangeData)
            {
                MaterialData.Get(exchangeData.MaterialTicker)?.PriceData.Update(exchangeData);
            }
        }      
        
        private static void InitializePriceData(FioCommodityExchange[] allExchanges, FioExchangeData[] allExchangeData)
        {
            foreach (var exchangeData in allExchangeData)
            {
                MaterialData.Get(exchangeData.MaterialTicker)?.PriceData.Initialize(allExchanges);
            }
        }
    }
}