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
        private const string? Skip = "No Blaming FIO today"; // Set this to null if you wanna run the tests here.
        
        public DataImport(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = Skip)]
        public void ImportBuildings()
        {
            var result = FioImporter.ImportBuildings();
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportMaterials()
        {
            var result = FioImporter.ImportMaterials();
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
            var result = FioImporter.ImportPlanetData("Montem");
            var json = JsonConvert.SerializeObject(result);
            _testOutputHelper.WriteLine(json);
        }

        [Fact(Skip = Skip)]
        public void ImportAll()
        {
            var result = FioImporter.ImportAll();
            Debugger.Break();
        }
        
        [Fact(Skip = Skip)]
        public void RawData()
        {
            var result = FioImporter.LoadAllFromCache();
            Debugger.Break();
        }

        [Fact(Skip = Skip)]
        public void ParsedData()
        {
            var result = FioImporter.LoadAndParseFromCache();
            Debugger.Break();
        }
    }
}