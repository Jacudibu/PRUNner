using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using FIOImport.Pocos;
using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;
using Newtonsoft.Json;
using NLog;

namespace FIOImport
{
    public abstract class Downloader<TInstance, TFioPoco> where TInstance : Downloader<TInstance, TFioPoco>, new()
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly HttpClient Client = new();

        protected abstract string DataName { get; }
        protected abstract string CachePath { get; }
        protected abstract string DownloadPath { get; }

        public static readonly TInstance Instance = new();

        public TFioPoco[] LoadFromCacheOrDownload()
        {
            if (!File.Exists(FioImporter.CacheFolder + CachePath))
            {
                Logger.Info("Unable to locate " + DataName + " cache file, downloading it instead..");
                return DownloadAndCache();
            }

            Logger.Info("Loading " + DataName + " from Cache...");
            return LoadFromCache(FioImporter.CacheFolder + CachePath)!;
        }

        public TFioPoco[] DownloadAndCache()
        {
            Logger.Info("Downloading " + DataName + " from FIO...");
            return DownloadAndCache(DownloadPath, FioImporter.CacheFolder + CachePath)!;
        } 
        
        private const int MaximumRetries = 5;
        private static TFioPoco[]? DownloadAndCache(string requestUri, string cacheFilePath)
        {
            var tries = 0;
            while (tries < MaximumRetries + 1)
            {
                try
                {
                    var json = Client.GetStringAsync(requestUri).GetAwaiter().GetResult();
                    File.WriteAllText(cacheFilePath, json);
                    return JsonConvert.DeserializeObject<TFioPoco[]>(json);
                }
                catch (HttpRequestException e)
                {
                    tries++;
                    Logger.Error(e, "Error whilst downlading data – waiting a bit, then retrying [{Tries}/{MaximumRetries}]", tries, MaximumRetries);
                    Thread.Sleep(2000);
                }
            }

            throw new Exception("Unable to query " + requestUri);
        }

        private static TFioPoco[]? LoadFromCache(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<TFioPoco[]>(json);
        }
    }

    internal class FioMaterialDownloader : Downloader<FioMaterialDownloader, FioMaterial>
    {
        protected override string DataName => "material data";
        protected override string CachePath => "allMaterials.json";
        protected override string DownloadPath => "https://rest.fnar.net/material/allmaterials";
    }
    
    internal class FioBuildingDownloader : Downloader<FioBuildingDownloader, FioBuilding>
    {
        protected override string DataName => "building data";
        protected override string CachePath => "allBuildings.json";
        protected override string DownloadPath => "https://rest.fnar.net/building/allbuildings";
    }
    
    internal class FioSystemDownloader : Downloader<FioSystemDownloader, FioSystem>
    {
        protected override string DataName => "system data";
        protected override string CachePath => "allSystems.json";
        protected override string DownloadPath => "https://rest.fnar.net/systemstars";
    }
    
    internal class FioCommodityExchangeDownloader : Downloader<FioCommodityExchangeDownloader, FioCommodityExchange>
    {
        protected override string DataName => "commodity exchange data";
        protected override string CachePath => "allCommodityExchanges.json";
        protected override string DownloadPath => "https://rest.fnar.net/global/comexexchanges";
    }

    internal class FioPlanetDownloader : Downloader<FioPlanetDownloader, FioPlanet>
    {
        protected override string DataName => "planet data";
        protected override string CachePath => "allPlanets.json";
        protected override string DownloadPath => "https://rest.fnar.net/planet/allplanets/full";
    }

    public class FioPriceDownloader : Downloader<FioPriceDownloader, FioRainPrices>
    {
        protected override string DataName => "price data";
        protected override string CachePath => "priceData.json";
        protected override string DownloadPath => "https://rest.fnar.net/rain/prices";
    }
}