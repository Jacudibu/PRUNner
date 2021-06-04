using FIOImport.POCOs.Planets;

namespace FIOImport.Data.Components
{
    public class BuildRequirementData
    {
        public readonly MaterialData Material;
        public readonly int Amount;

        internal BuildRequirementData(FioBuildRequirement poco)
        {
            Material = MaterialData.AllItemsByPocoId[poco.MaterialId];
            Amount = poco.MaterialAmount;
        }
    }
}