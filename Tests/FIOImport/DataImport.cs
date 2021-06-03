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

        [Fact]
        public void LoadFromCache()
        {
            FioImporter.LoadAllFromCache();
        }
    }
}