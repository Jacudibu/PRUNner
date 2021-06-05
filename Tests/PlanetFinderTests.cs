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

            public DistanceSearchResult(PlanetData planet, string targetSystem)
            {
                PlanetData = planet;
                Path = SystemPathFinder.FindShortestPath(planet.System, SystemData.AllItems[targetSystem]);
            }
        }
        
        [Fact]
        public void FindingPlanetFilteredByDistance()
        {
            var result = PlanetFinder.Find(FilterCriteria.T1Criteria, "LST")
                .Select(x => new DistanceSearchResult(x, "CH-771"))                
                .OrderBy(x => x.Path.Count);
            
            _testOutputHelper.WriteLine("Displaying all T1 planets with LST, sorted by distance to CH-771:");
   
            foreach (var searchResult in result)
            {
                _testOutputHelper.WriteLine($"{searchResult.Path.Count} – {searchResult.PlanetData.Id} ({searchResult.PlanetData.Name}) – {searchResult.PlanetData.GetResource("LST")?.Factor}");
            }
        }
    }
}