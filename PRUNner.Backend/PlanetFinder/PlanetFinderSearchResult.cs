using System.Linq;
using PRUNner.Backend.Data;

namespace PRUNner.Backend.PlanetFinder
{
    public class PlanetFinderSearchResult
    {
        public PlanetData Planet { get; }
        
        public string Resource1 { get; }
        public string Resource2 { get; }
        public string Resource3 { get; }
        public string Resource4 { get; }

        public readonly double Resource1Yield; // needed for sorting
        public readonly double Resource2Yield;
        public readonly double Resource3Yield;
        public readonly double Resource4Yield;

        public int DistanceToExtraSystem { get; }
        public bool DisplayDistanceToExtraSystem { get; }

        public bool DisplayResource1 => Resource1Yield > 0;
        public bool DisplayResource2 => Resource2Yield > 0;
        public bool DisplayResource3 => Resource3Yield > 0;
        public bool DisplayResource4 => Resource4Yield > 0;
        
        public PlanetFinderSearchResult(PlanetData planet, MaterialData[] resources, OptionalPlanetFinderData optionalData) : this(planet, resources.Select(x => x.Ticker).ToArray(), optionalData)
        { }
            
        public PlanetFinderSearchResult(PlanetData planet, string[] resources, OptionalPlanetFinderData optionalData)
        {
            Planet = planet;

            Resource1Yield = GetResourceYield(1, resources);
            Resource2Yield = GetResourceYield(2, resources);
            Resource3Yield = GetResourceYield(3, resources);
            Resource4Yield = GetResourceYield(4, resources);
            Resource1 = Resource1Yield.ToString("F2");
            Resource2 = Resource2Yield.ToString("F2");
            Resource3 = Resource3Yield.ToString("F2");
            Resource4 = Resource4Yield.ToString("F2");
            
            if (optionalData.AdditionalSystem != null)
            {
                DistanceToExtraSystem = SystemPathFinder.FindShortestPath(planet.System, optionalData.AdditionalSystem).Count;
                DisplayDistanceToExtraSystem = true;
            }
            else
            {
                DistanceToExtraSystem = -1;
                DisplayDistanceToExtraSystem = false;
            }
        }

        private double GetResourceYield(int resourceId, string[] resources)
        {
            return resources.Length > resourceId - 1 
                ? Planet.GetResource(resources[resourceId - 1])!.CalculateDailyProduction(1) 
                : -1;
        }

        public void OpenBasePlanner()
        {
            // TODO: later on we can invoke an event here that the editor can subscribe to.
        }
    }
}