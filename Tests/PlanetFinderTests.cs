using System;
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
            var materialFilter = new List<MaterialData>()
            {
                MaterialData.AllItems["FEO"]
            };

            var result = PlanetFinder.Find(materialFilter, false, false, false,
                    true, true, true, true, true, true)
                .OrderByDescending(x => x.Resources.Single(res => res.Material.Ticker.Equals("FEO")).Factor);
            
            _testOutputHelper.WriteLine("Displaying all T1 planets with FEO, sorted by concentration:");
            foreach (var planetData in result)
            {
                _testOutputHelper.WriteLine($"{planetData.Id} ({planetData.Name}) – {planetData.GetResource("FEO")?.Factor}");
            }
        }
    }
}