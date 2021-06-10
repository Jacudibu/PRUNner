using System.Diagnostics;
using System.Linq;
using FIOImport;
using Newtonsoft.Json;
using PRUNner.Backend;
using Xunit;
using Xunit.Abstractions;

namespace Tests.FIOImport
{
    public class DataImport
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private const string? Skip = "No Blaming FIO today"; // Set this to null if you wanna run the tests here.
        
        public DataImport(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = Skip)]
        public void ImportBuildings()
        {
            var result = FioImporter.DownloadBuildings();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportMaterials()
        {
            var result = FioImporter.DownloadMaterials();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportPlanetIdentifiers()
        {
            var result = FioImporter.ImportPlanetIdentifiers();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportSpecificPlanet()
        {
            var result = FioImporter.LoadFromCacheOrDownloadPlanetData(Names.Planets.Montem);
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportAll()
        {
            var result = FioImporter.ImportAll();
            Debugger.Break();
        }
        
        [Fact(Skip = null)]
        public void RawData()
        {
            var result = FioImporter.LoadAllFromCache();
            foreach (var item in result.AllBuildings.Where(x => x.Expertise != null).Select(x => x.Expertise).Distinct().OrderBy(x => x))
            {
                _testOutputHelper.WriteLine(item);
            }
            Debugger.Break();
        }

        [Fact(Skip = Skip)]
        public void ParsedData()
        {
            DataParser.LoadAndParseFromCache();
            Debugger.Break();
        }
    }
}