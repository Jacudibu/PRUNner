using System.Collections.Generic;
using System.Linq;
using FIOImport.POCOs.Planets;

namespace FIOImport.Data
{
    public class PlanetData
    {
        public static readonly Dictionary<string, PlanetData> AllPlanets = new Dictionary<string, PlanetData>();
        private static readonly Dictionary<string, PlanetData> AllPlanetsById = new Dictionary<string, PlanetData>();
        
        public readonly ResourceData[] Resources;
        public readonly BuildRequirementData[] ColonizationMaterials; 

        private readonly string _id;
        public readonly string NaturalId;
        public readonly string Name;
        
        public readonly double Fertility;
        public readonly double Gravity;
        public readonly double Pressure;
        public readonly double Radiation;
        
        private PlanetData(Planet planet)
        {
            _id = planet.PlanetId;
            NaturalId = planet.PlanetNaturalId;
            Name = planet.PlanetName;
            
            Fertility = planet.Fertility;
            Gravity = planet.Gravity;
            Pressure = planet.Pressure;
            Radiation = planet.Radiation;

            Resources = planet.Resources.Select(x => new ResourceData(x)).ToArray();
            ColonizationMaterials = planet.BuildRequirements.Select(x => new BuildRequirementData(x)).ToArray();
        }

        public static void CreateFrom(Planet planet)
        {
            var result = new PlanetData(planet);
            AllPlanets[result.NaturalId] = result;
        }

        internal static PlanetData GetById(string id)
        {
            return AllPlanetsById[id];
        }
    }
}