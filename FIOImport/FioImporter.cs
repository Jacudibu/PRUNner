using System.IO;
using System.Linq;
using System.Net.Http;
using FIOImport.Pocos;
using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;
using Newtonsoft.Json;
using NLog;

namespace FIOImport
{
    public static class FioImporter
    {
        private static readonly HttpClient Client = new();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private const string CacheFolder = "FIOCache/";
        private const string PlanetFolder = CacheFolder + "Planets/";

        private const string AllBuildingsPath = CacheFolder + "allBuildings.json";
        private const string AllMaterialsPath = CacheFolder + "allMaterials.json";
        private const string AllSystemsPath = CacheFolder + "systemStars.json";
        private const string AllPlanetIdentifiersPath = CacheFolder + "allPlanetIdentifiers.json";
        private const string RainPricesPath = CacheFolder + "rainPrices.json";
        
        public static RawData ImportAll()
        {
            Directory.CreateDirectory(CacheFolder);
            Directory.CreateDirectory(PlanetFolder);
            
            return new RawData(DownloadBuildings(), DownloadMaterials(), DownloadAllPlanetData(), DownloadSystems(), DownloadPrices());
        }

        public static RawData LoadAllFromCache()
        {
            if (Directory.Exists(CacheFolder))
            {
                return new RawData(LoadBuildings(), LoadMaterials(), LoadAllPlanetData(), LoadSystems(), LoadPrices());
            }

            Logger.Info("Unable to find Cache folder, downloading all data from fio instead. This might take a little while.");
            return ImportAll();
        }

        public static FioBuilding[] LoadBuildings()
        {
            if (!File.Exists(AllBuildingsPath))
            {
                Logger.Info("Unable to locate cache file for buildings, downloading instead.");
                return DownloadBuildings();
            }

            Logger.Info("Loading Buildings...");
            var json = File.ReadAllText(AllBuildingsPath);
            return JsonConvert.DeserializeObject<FioBuilding[]>(json)!;
        }
        
        public static FioBuilding[] DownloadBuildings()
        {
            Logger.Info("Downloading Building data from FIO...");
            var json = Client.GetStringAsync("https://rest.fnar.net/building/allbuildings").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioBuilding[]>(json);
            
            File.WriteAllText(AllBuildingsPath, json);

            return result!;
        }        
        
        public static FioMaterial[] LoadMaterials()
        {
            var json = File.ReadAllText(AllMaterialsPath);
            var result = JsonConvert.DeserializeObject<FioMaterial[]>(json);

            return result!;
        }

        public static FioMaterial[] DownloadMaterials()
        {
            Logger.Info("Donwloading Material data from FIO...");
            var json = Client.GetStringAsync("https://rest.fnar.net/material/allmaterials").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioMaterial[]>(json);
            
            File.WriteAllText(AllMaterialsPath, json);

            return result!;
        }
        
        public static FioRainPrices[] LoadPrices()
        {
            if (!File.Exists(RainPricesPath))
            {
                Logger.Info("Unable to locate cache file for price data, downloading instead.");
                return DownloadPrices();
            }
            
            Logger.Info("Loading Price data from Cache...");
            var json = File.ReadAllText(RainPricesPath);
            return JsonConvert.DeserializeObject<FioRainPrices[]>(json)!;
        }
        
        public static FioRainPrices[] DownloadPrices()
        {
            Logger.Info("Importing Price data from Fio...");
            var json = Client.GetStringAsync("https://rest.fnar.net/rain/prices").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioRainPrices[]>(json);
            
            File.WriteAllText(RainPricesPath, json);

            return result!;
        }        
        
        public static FioPlanetIdentifier[] LoadPlanetIdentifiers()
        {
            var json = File.ReadAllText(AllPlanetIdentifiersPath);
            var result = JsonConvert.DeserializeObject<FioPlanetIdentifier[]>(json);

            return result!;
        }
        
        public static FioPlanetIdentifier[] ImportPlanetIdentifiers()
        {
            Logger.Info("Downloading Planet list from FIO...");
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/allplanets").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioPlanetIdentifier[]>(json);

            File.WriteAllText(AllPlanetIdentifiersPath, json);
            
            return result!;
        }

        public static FioPlanet[] LoadAllPlanetData()
        {
            if (!Directory.Exists(PlanetFolder))
            {
                Logger.Info("Unable to locate any planet data, going to download it instead instead.");
                return DownloadAllPlanetData();
            }
            
            Logger.Info("Loading Planet Data from Cache...");
            var files = Directory.GetFiles(PlanetFolder);
            return files.Select(LoadPlanetData).ToArray();
        }
        
        public static FioPlanet LoadPlanetData(string fileName)
        {
            var json = File.ReadAllText(fileName);
            var result = JsonConvert.DeserializeObject<FioPlanet>(json);

            return result!;
        }

        public static FioPlanet[] DownloadAllPlanetData()
        {
            var identifiers = ImportPlanetIdentifiers();
            var allPlanets = new FioPlanet[identifiers.Length];
            for (var i = 0; i < allPlanets.Length; i++)
            {
                Logger.Info("Downloading Planets [{0}/{1}]", i + 1, identifiers.Length);
                allPlanets[i] = LoadFromCacheOrDownloadPlanetData(identifiers[i].PlanetNaturalId);
            }

            return allPlanets;
        }
        
        public static FioPlanet LoadFromCacheOrDownloadPlanetData(string planetId)
        {
            if (File.Exists($"{PlanetFolder}{planetId}.json"))
            {
                Logger.Info("Cache for {0} detected, using that instead.", planetId);
                return LoadPlanetData($"{PlanetFolder}{planetId}.json");
            }
            
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/" + planetId).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioPlanet>(json);

            File.WriteAllText($"{PlanetFolder}{planetId}.json", json);

            return result!;
        }
        
        public static FioSystem[] LoadSystems()
        {
            if (!File.Exists(AllSystemsPath))
            {
                Logger.Info("Unable to locate system data cache file, downloading it instead..");
                return DownloadSystems();
            }
            
            Logger.Info("Loading System Data from Cache...");
            var json = File.ReadAllText(AllSystemsPath);
            var result = JsonConvert.DeserializeObject<FioSystem[]>(json);

            return result!;
        }

        public static FioSystem[] DownloadSystems()
        {
            Logger.Info("Downloading Systems from FIO...");
            var json = Client.GetStringAsync("https://rest.fnar.net/systemstars").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioSystem[]>(json);
            
            File.WriteAllText(AllSystemsPath, json);

            return result!;
        }
    }
}