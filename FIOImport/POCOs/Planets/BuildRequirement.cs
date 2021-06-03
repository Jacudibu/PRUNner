// ReSharper disable All
namespace FIOImport.POCOs.Planets
{
    public class BuildRequirement
    {
        public string MaterialName { get; set; }
        public string MaterialId { get; set; }
        public string MaterialTicker { get; set; }
        public string MaterialCategory { get; set; }
        public int MaterialAmount { get; set; }
        public double MaterialWeight { get; set; }
        public double MaterialVolume { get; set; }
    }
}