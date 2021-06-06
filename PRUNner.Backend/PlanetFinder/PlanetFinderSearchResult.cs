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

        public int DistancePyrgos { get; }

        public bool DisplayResource1 => !string.IsNullOrEmpty(Resource1);
        public bool DisplayResource2 => !string.IsNullOrEmpty(Resource2);
        public bool DisplayResource3 => !string.IsNullOrEmpty(Resource3);
        public bool DisplayResource4 => !string.IsNullOrEmpty(Resource4);
        
        public PlanetFinderSearchResult(PlanetData planet, MaterialData[] resources) : this(planet, resources.Select(x => x.Ticker).ToArray())
        { }
            
        public PlanetFinderSearchResult(PlanetData planet, string[] resources)
        {
            Planet = planet;
            DistancePyrgos = SystemPathFinder.FindShortestPath(planet.System, SystemData.Get("CH-771")).Count;

            Resource1 = ParseResource(1, resources);
            Resource2 = ParseResource(2, resources);
            Resource3 = ParseResource(3, resources);
            Resource4 = ParseResource(4, resources);
        }

        private string ParseResource(int resourceId, string[] resources)
        {
            return resources.Length > resourceId - 1 
                ? Planet.GetResource(resources[resourceId - 1])!.CalculateDailyProduction(1).ToString("F2") 
                : "";
        }

        public void OpenBasePlanner()
        {
            // TODO: later on we can invoke an event here that the editor can subscribe to.
        }
    }
}