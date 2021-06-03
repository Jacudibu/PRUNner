using System.Collections.Generic;
using FIOImport.Data;

namespace FIOImport
{
    public class ParsedData
    {
        public Dictionary<string, MaterialData> AllMaterials = MaterialData.AllMaterials;
        public Dictionary<string, PlanetData> AllPlanets = PlanetData.AllPlanets;

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
        }
    }
}