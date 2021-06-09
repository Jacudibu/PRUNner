using System;
using FIOImport.POCOs.Planets;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Backend.Data.Components
{
    public class ResourceData
    {
        public MaterialData Material { get; }
        public ResourceType ResourceType { get; }
        public double Factor { get; }

        internal ResourceData(FioResource poco)
        {
            Material = MaterialData.AllItemsByPocoId[poco.MaterialId];
            Factor = poco.Factor;
            ResourceType = Enum.Parse<ResourceType>(poco.ResourceType, true);
        }

        public double CalculateDailyProduction(double workforceEfficiency)
        {
            double extractionRate;
            switch (ResourceType)
            {
                case ResourceType.Gaseous:
                    extractionRate = 60;
                    break;
                case ResourceType.Liquid:
                case ResourceType.Mineral:
                    extractionRate = 70;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ResourceType), ResourceType, null);
            }

            return extractionRate * workforceEfficiency * Factor;
        }
    }
}