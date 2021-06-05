using System.Collections.Generic;
using System.Linq;
using FIOImport;
using FIOImport.Data;
using PRUNner.Backend;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PlanetFinderTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        static PlanetFinderTests()
        {
            FioImporter.LoadAndParseFromCache();
        }

        public PlanetFinderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void FindingPlanet()
        {
            var result = PlanetFinder.Find(FilterCriteria.T1Criteria, MaterialData.AllItems["FEO"])
                .OrderByDescending(x => x.GetResource("FEO")!.Factor);
            
            _testOutputHelper.WriteLine("Displaying all T1 planets with FEO, sorted by concentration:");
            foreach (var planetData in result)
            {
                _testOutputHelper.WriteLine($"{planetData.Id} ({planetData.Name}) – {planetData.GetResource("FEO")?.Factor}");
            }
        }

        public class DistanceSearchResult
        { 
            public readonly PlanetData PlanetData;
            public readonly List<SystemData> Path;

            public readonly List<SystemData> PathToMoria;
            public readonly List<SystemData> PathToBenten;

            public DistanceSearchResult(PlanetData planet, string targetSystem)
            {
                PlanetData = planet;
                Path = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems[targetSystem]);
                PathToMoria = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["OT-580"]);
                PathToBenten = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems["UV-351"]);
            }
        }
        
        [Fact]
        public void FindingPlanetFilteredByDistance()
        {
            // using this more like a command line tool right now... :D
            
            const string origin = "CH-771";
            const string resource = "H";
            var result = PlanetFinder.Find(FilterCriteria.None, resource)
                .Select(x => new DistanceSearchResult(x, origin))                
                .OrderBy(x => x.Path.Count);
            
            _testOutputHelper.WriteLine($"Displaying all planets with {resource}, sorted by distance to {origin}:");

            foreach (var searchResult in result)
            {
                _testOutputHelper.WriteLine($"{searchResult.Path.Count} – {searchResult.PlanetData.Id} ({searchResult.PlanetData.Name}) – {searchResult.PlanetData.GetResource(resource)?.Factor} – Distance Moria: {searchResult.PathToMoria.Count}, Distance Benten: {searchResult.PathToBenten.Count}" );
            }
        }
    }
}