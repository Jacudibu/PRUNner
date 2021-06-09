using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using FIOImport.Pocos;
using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;
using Newtonsoft.Json;

namespace FIOImport
{
    public static class FioImporter
    {
        private static readonly HttpClient Client = new HttpClient();

        private const string CacheFolder = "Cache/";
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
            
            return new RawData(ImportBuildings(), ImportMaterials(), ImportAllPlanetData(), ImportSystems(), ImportPrices());
        }

        public static RawData LoadAllFromCache()
        {
            if (Directory.Exists(CacheFolder))
            {
                return new RawData(LoadBuildings(), LoadMaterials(), LoadAllPlanetData(), LoadSystems(), LoadPrices());
            }

            Console.WriteLine("Unable to find cached data, downloading...");
            return ImportAll();
        }

        public static FioBuilding[] LoadBuildings()
        {
            var json = File.ReadAllText(AllBuildingsPath);
            var result = JsonConvert.DeserializeObject<FioBuilding[]>(json);

            return result!;
        }
        
        public static FioBuilding[] ImportBuildings()
        {
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

        public static FioMaterial[] ImportMaterials()
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/material/allmaterials").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioMaterial[]>(json);
            
            File.WriteAllText(AllMaterialsPath, json);

            return result!;
        }
        
        public static FioRainPrices[] LoadPrices()
        {
            var json = File.ReadAllText(RainPricesPath);
            var result = JsonConvert.DeserializeObject<FioRainPrices[]>(json);

            return result!;
        }
        
        public static FioRainPrices[] ImportPrices()
        {
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
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/allplanets").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioPlanetIdentifier[]>(json);

            File.WriteAllText(AllPlanetIdentifiersPath, json);
            
            return result!;
        }

        public static FioPlanet[] LoadAllPlanetData()
        {
            var files = Directory.GetFiles(PlanetFolder);
            return files.Select(LoadPlanetData).ToArray();
        }
        
        public static FioPlanet LoadPlanetData(string fileName)
        {
            var json = File.ReadAllText(fileName);
            var result = JsonConvert.DeserializeObject<FioPlanet>(json);

            return result!;
        }

        public static FioPlanet[] ImportAllPlanetData()
        {
            var identifiers = ImportPlanetIdentifiers();
            return identifiers.Select(x => ImportPlanetData(x.PlanetNaturalId)).ToArray();
        }
        
        public static FioPlanet ImportPlanetData(string planetId)
        {
            if (File.Exists($"{PlanetFolder}{planetId}.json"))
            {
                return LoadPlanetData($"{PlanetFolder}{planetId}.json");
            }
            
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/" + planetId).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioPlanet>(json);

            File.WriteAllText($"{PlanetFolder}{planetId}.json", json);

            return result!;
        }
        
        public static FioSystem[] LoadSystems()
        {
            var json = File.ReadAllText(AllSystemsPath);
            var result = JsonConvert.DeserializeObject<FioSystem[]>(json);

            return result!;
        }

        public static FioSystem[] ImportSystems()
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/systemstars").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<FioSystem[]>(json);
            
            File.WriteAllText(AllSystemsPath, json);

            return result!;
        }
    }
}