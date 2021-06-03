using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private const string AllPlanetIdentifiersPath = CacheFolder + "allPlanetIdentifiers.json";
        
        public static RawData ImportAll()
        {
            Directory.CreateDirectory(CacheFolder);
            Directory.CreateDirectory(PlanetFolder);
            return new RawData(ImportBuildings(), ImportMaterials(), ImportPlanetIdentifiers(), new Planet[0]);
        }
        
        public static RawData LoadAllFromCache()
        {
            if (Directory.Exists(CacheFolder))
            {
                return new RawData(LoadBuildings(), LoadMaterials(), LoadPlanetIdentifiers(), LoadAllPlanetData());
            }

            Console.WriteLine("Unable to find cached data, downloading...");
            return ImportAll();
        }

        public static ParsedData LoadAndParseFromCache()
        {
            var rawData = LoadAllFromCache();
            return new ParsedData(rawData);
        }

        public static Building[] LoadBuildings()
        {
            var json = File.ReadAllText(AllBuildingsPath);
            var result = JsonConvert.DeserializeObject<Building[]>(json);

            return result;
        }
        
        public static Building[] ImportBuildings()
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/building/allbuildings").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Building[]>(json);
            
            File.WriteAllText(AllBuildingsPath, json);

            return result;
        }        
        
        public static Material[] LoadMaterials()
        {
            var json = File.ReadAllText(AllMaterialsPath);
            var result = JsonConvert.DeserializeObject<Material[]>(json);

            return result;
        }

        public static Material[] ImportMaterials()
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/material/allmaterials").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Material[]>(json);
            
            File.WriteAllText(AllMaterialsPath, json);

            return result;
        }
        
        public static PlanetIdentifier[] LoadPlanetIdentifiers()
        {
            var json = File.ReadAllText(AllPlanetIdentifiersPath);
            var result = JsonConvert.DeserializeObject<PlanetIdentifier[]>(json);

            return result;
        }
        
        public static PlanetIdentifier[] ImportPlanetIdentifiers()
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/allplanets").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<PlanetIdentifier[]>(json);

            File.WriteAllText(AllPlanetIdentifiersPath, json);
            
            return result;
        }

        public static Planet[] LoadAllPlanetData()
        {
            var files = Directory.GetFiles(PlanetFolder);
            return files.Select(LoadPlanetData).ToArray();
        }
        
        public static Planet LoadPlanetData(string fileName)
        {
            var json = File.ReadAllText(fileName);
            var result = JsonConvert.DeserializeObject<Planet>(json);

            return result;
        }
        
        public static Planet ImportPlanetData(PlanetIdentifier planetIdentifier)
        {
            return ImportPlanetData(planetIdentifier.PlanetNaturalId);
        }

        public static Planet ImportPlanetData(string planetId)
        {
            var json = Client.GetStringAsync("https://rest.fnar.net/planet/" + planetId).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Planet>(json);

            File.WriteAllText($"{PlanetFolder}{planetId}.json", json);

            return result;
        }
    }
}