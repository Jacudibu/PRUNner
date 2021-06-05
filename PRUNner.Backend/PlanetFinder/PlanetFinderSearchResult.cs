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

        public int DistanceMoria { get; }
        public int DistanceBenten { get; }
        public int DistanceHortus { get; }
        public int DistanceAntares { get; }
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
            
            DistanceMoria = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["OT-580"]).Count;
            DistanceBenten = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["UV-351"]).Count;
            DistanceHortus = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["VH-331"]).Count;
            DistanceAntares = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["ZV-307"]).Count;
            DistancePyrgos = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["CH-771"]).Count;
            
            if (resources.Length > 0)
            {
                Resource1 = planet.GetResource(resources[0])!.CalculateDailyProduction(1).ToString("F2");
            }
            if (resources.Length > 1)
            {
                Resource2 = planet.GetResource(resources[1])!.CalculateDailyProduction(1).ToString("F2");
            }
            if (resources.Length > 2)
            {
                Resource3 = planet.GetResource(resources[2])!.CalculateDailyProduction(1).ToString("F2");
            }
            if (resources.Length > 3)
            {
                Resource4 = planet.GetResource(resources[3])!.CalculateDailyProduction(1).ToString("F2");
            }
        }
    }
}