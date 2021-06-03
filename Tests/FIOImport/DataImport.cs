using System.Diagnostics;
using FIOImport;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Tests.FIOImport
{
    public class DataImport
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DataImport(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ImportBuildings()
        {
            var result = FioImporter.ImportBuildings();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact]
        public void ImportMaterials()
        {
            var result = FioImporter.ImportMaterials();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact]
        public void ImportPlanetIdentifiers()
        {
            var result = FioImporter.ImportPlanetIdentifiers();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact]
        public void ImportSpecificPlanet()
        {
            var result = FioImporter.ImportPlanetData("Montem");
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = "You really don't want to import planets by accident...")]
        public void ImportAll()
        {
            var result = FioImporter.ImportAll();
            Debugger.Break();
        }
        
        [Fact]
        public void RawData()
        {
            var result = FioImporter.LoadAllFromCache();
            Debugger.Break();
        }

        [Fact]
        public void ParsedData()
        {
            var result = FioImporter.LoadAndParseFromCache();
            Debugger.Break();
        }
    }
}