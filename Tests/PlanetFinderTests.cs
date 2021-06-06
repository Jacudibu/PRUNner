using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PlanetFinderTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        static PlanetFinderTests()
        {
            DataParser.LoadAndParseFromCache();
        }

        public PlanetFinderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void FindingPlanet()
        {
            var result = PlanetFinder.Find(FilterCriteria.T1Criteria, MaterialData.GetOrThrow(Names.Materials.FEO))
                .OrderByDescending(x => x.Planet.GetResource(Names.Materials.FEO)!.Factor);
            
            _testOutputHelper.WriteLine("Displaying all T1 planets with FEO, sorted by concentration:");
            foreach (var searchResult in result)
            {
                _testOutputHelper.WriteLine($"{searchResult.Planet.Id} ({searchResult.Planet.Name}) – {searchResult.Planet.GetResource(Names.Materials.FEO)?.Factor}");
            }
        }

        public class DistanceSearchResult
        {
            public readonly PlanetData Planet;
            public readonly SystemData From;
            public readonly SystemData To;
            public readonly List<SystemData> Path;

            public readonly List<SystemData> PathToMoria;
            public readonly List<SystemData> PathToBenten;

            public DistanceSearchResult(PlanetFinderSearchResult searchResult, string targetSystem)
            {
                Planet = searchResult.Planet;
                From = Planet.System;
                To = SystemData.GetOrThrow(targetSystem);
                Path = SystemPathFinder.FindShortestPath(From, To);
                PathToMoria = SystemPathFinder.FindShortestPath(From, SystemData.GetOrThrow(Names.Systems.Moria));
                PathToBenten = SystemPathFinder.FindShortestPath(From, SystemData.GetOrThrow(Names.Systems.Benten));
            }

            public override string ToString()
            {
                var builder = new StringBuilder();

                builder.Append(Planet.Name);
                foreach (var system in Path)
                {
                    builder.Append(" – ");
                    builder.Append(system.Name);
                }

                return builder.ToString();
            }
        }

        [Fact]
        public void PathingSingleJump()
        {
            var result = SystemPathFinder.FindShortestPath(SystemData.GetOrThrow("SO-953"), SystemData.GetOrThrow("CH-771"));
            Assert.Single(result);
        }
        
        [Fact]
        public void FindingPlanetFilteredByDistance()
        {
            // using this more like a command line tool right now... :D
            
            const string origin = "CH-771";
            const string resource = "N";
            var result = PlanetFinder.Find(FilterCriteria.None, resource)
                .Select(x => new DistanceSearchResult(x, origin))                
                .OrderBy(x => x.Path.Count);
            
            _testOutputHelper.WriteLine($"Displaying all planets with {resource}, sorted by distance to {origin}:");

            foreach (var searchResult in result)
            {
                _testOutputHelper.WriteLine($"{searchResult.Path.Count} – {searchResult.Planet.Id} ({searchResult.Planet.Name}) – {searchResult.Planet.GetResource(resource)?.Factor} – Distance Moria: {searchResult.PathToMoria.Count}, Distance Benten: {searchResult.PathToBenten.Count}\n  | Path: {searchResult}" );
            }
        }
    }
}