using System.Linq;
using FIOImport.POCOs.Planets;
using PRUNner.Backend.Data.BaseClasses;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using PRUNner.Backend.PlanetFinder;

namespace PRUNner.Backend.Data
{
    public class PlanetData : GameData<PlanetData, FioPlanet>
    { 
        public ResourceData[] Resources { get; private set; } = null!;
        public BuildRequirementData[] ColonizationMaterials { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public double Fertility { get; private set; }
        public double Gravity { get; private set; }
        public double Pressure { get; private set; }
        public double Temperature { get; private set; }
        public double Radiation { get; private set; }
        public PlanetType Type { get; private set; }
        public SystemData System { get; private set; } = null!;

        private PlanetFinderCache? _planetFinderCache;
        public PlanetFinderCache PlanetFinderCache => _planetFinderCache ??= new PlanetFinderCache(this);

        internal override string GetIdFromPoco(FioPlanet poco) => poco.PlanetNaturalId;
        internal override string GetFioIdFromPoco(FioPlanet poco) => poco.PlanetId;

        internal override void PostProcessData(FioPlanet poco)
        {
            Name = poco.PlanetName;
            AddAlias(this, Name);
            
            Fertility = poco.Fertility;
            Gravity = poco.Gravity;
            Pressure = poco.Pressure;
            Temperature = poco.Temperature;
            Radiation = poco.Radiation;

            Resources = poco.Resources.Select(x => new ResourceData(x)).ToArray();
            ColonizationMaterials = poco.BuildRequirements.Select(x => new BuildRequirementData(x)).ToArray();

            Type = poco.Surface ? PlanetType.Rocky : PlanetType.Gaseous;
            System = SystemData.AllItemsByPocoId[poco.SystemId];
            System.AddPlanet(this);
        }

        public bool HasResource(string ticker)
        {
            return Resources.Any(x => x.Material.Ticker.Equals(ticker));
        }
        
        public bool HasResource(MaterialData material)
        {
            return Resources.Any(x => x.Material.Equals(material));
        }
        
        public ResourceData? GetResource(string ticker)
        {
            return Resources.SingleOrDefault(x => x.Material.Ticker.Equals(ticker));
        }       
        
        public ResourceData? GetResource(MaterialData material)
        {
            return Resources.SingleOrDefault(x => x.Material.Equals(material));
        }

        public bool IsFertile()
        {
            return Fertility > -1;
        }

        public bool IsLowGravity()
        {
            return Gravity < 0.25;
        }

        public bool IsHighGravity()
        {
            return Gravity > 2.5;
        }

        public bool IsLowPressure()
        {
            return Pressure < 0.25;
        }

        public bool IsHighPressure()
        {
            return Pressure > 2;
        }

        public bool IsLowTemperature()
        {
            return Temperature < -25;
        }

        public bool IsHighTemperature()
        {
            return Temperature > 75;
        }
    }
    
    public class PlanetFinderCache
    {
        public string BuildingMaterialString { get; }
        public int DistanceToAntares { get; }
        public int DistanceToBenten { get; }
        public int DistanceToHortus { get; }
        public int DistanceToMoria { get; }
        public int DistanceToHubur { get; }
        public int DistanceToArclight { get; }

        public PlanetFinderCache(PlanetData planet)
        {
            BuildingMaterialString = CreateBuildingMaterialString(planet);
            DistanceToAntares = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.AntaresI)).Count;
            DistanceToBenten = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.Benten)).Count;
            DistanceToHortus = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.Hortus)).Count;
            DistanceToMoria = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.Moria)).Count;
            DistanceToHubur = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.Hubur)).Count;
            DistanceToArclight = SystemPathFinder.FindShortestPath(planet.System, SystemData.GetOrThrow(Names.Systems.Arclight)).Count;
        }
        
        private static string CreateBuildingMaterialString(PlanetData planet)
        {
            var result = planet.Type == PlanetType.Rocky ? "MCG" : "AEF";
            if (planet.IsLowGravity()) result += "|MGC";
            if (planet.IsHighGravity()) result += "|BL";
            if (planet.IsLowPressure()) result += "|SEA";
            if (planet.IsHighPressure()) result += "|HSE";
            if (planet.IsLowTemperature()) result += "|INS";
            if (planet.IsHighTemperature()) result += "|TSH";
            return result;
        }

    }
}