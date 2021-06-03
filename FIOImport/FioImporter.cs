using System.Net.Http;
using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;
using Newtonsoft.Json;

namespace FIOImport
{
    public class FioImporter
    {
        private static readonly HttpClient _client = new HttpClient();
        
        public static Building[] ImportBuildings()
        {
            var json = _client.GetStringAsync("https://rest.fnar.net/building/allbuildings").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Building[]>(json);

            return result;
        }        
        
        public static Material[] ImportMaterials()
        {
            var json = _client.GetStringAsync("https://rest.fnar.net/material/allmaterials").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Material[]>(json);

            return result;
        }
        
        public static PlanetIdentifier[] ImportPlanetIdentifiers()
        {
            var json = _client.GetStringAsync("https://rest.fnar.net/planet/allplanets").GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<PlanetIdentifier[]>(json);

            return result;
        }

        public static Planet ImportPlanetData(PlanetIdentifier planetIdentifier)
        {
            return ImportPlanetData(planetIdentifier.PlanetNaturalId);
        }

        public static Planet ImportPlanetData(string planetNameOrId)
        {
            var json = _client.GetStringAsync("https://rest.fnar.net/planet/" + planetNameOrId).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Planet>(json);

            return result;
        }
    }
}