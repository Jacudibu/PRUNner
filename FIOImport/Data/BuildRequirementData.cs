using FIOImport.POCOs.Planets;

namespace FIOImport.Data
{
    public class BuildRequirementData
    {
        public readonly MaterialData Material;
        public readonly int Amount;

        internal BuildRequirementData(BuildRequirement buildRequirement)
        {
            Material = MaterialData.GetById(buildRequirement.MaterialId);
            Amount = buildRequirement.MaterialAmount;
        }
    }
}