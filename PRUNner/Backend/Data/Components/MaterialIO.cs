using FIOImport.POCOs.Buildings;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialIO
    {
        public MaterialData Material { get; }
        public int Amount { get; }
        public double DailyAmount { get; }
        
        public MaterialIO(FioOutput poco, long durationInMs) : this(poco.CommodityTicker, poco.Amount, durationInMs)
        { }
        
        public MaterialIO(FioInput poco, long durationInMs) : this(poco.CommodityTicker, poco.Amount, durationInMs)
        { }
     
        public MaterialIO(FioBuildingCost poco) : this(poco.CommodityTicker, poco.Amount, Constants.MsPerDay * Constants.DaysUntilAllBuildingMaterialsAreLost) 
        { }

        private MaterialIO(string commodityTicker, int amount, double durationInMs)
            : this(MaterialData.GetOrThrow(commodityTicker), amount, durationInMs)
        { }

        public MaterialIO(MaterialData material, int amount, double durationInMs)
        {
            Material = material;
            Amount = amount;
            DailyAmount = amount / durationInMs * Constants.MsPerDay;
        }
    }
}