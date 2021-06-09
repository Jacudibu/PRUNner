using FIOImport.POCOs.Buildings;

namespace PRUNner.Backend.Data.Components
{
    public class MaterialIO
    {
        public MaterialData Material { get; }
        public int Amount { get; }

        public MaterialIO(FioOutput poco)
        {
            Material = MaterialData.GetOrThrow(poco.CommodityTicker);
            Amount = poco.Amount;
        }
        
        public MaterialIO(FioInput poco)
        {
            Material = MaterialData.GetOrThrow(poco.CommodityTicker);
            Amount = poco.Amount;
        }
     
        public MaterialIO(FioBuildingCost poco)
        {
            Material = MaterialData.GetOrThrow(poco.CommodityTicker);
            Amount = poco.Amount;
        }

        public MaterialIO(MaterialData material, int amount)
        {
            Material = material;
            Amount = amount;
        }
    }
}