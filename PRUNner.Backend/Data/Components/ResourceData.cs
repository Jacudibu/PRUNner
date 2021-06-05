using System;
using FIOImport.POCOs.Planets;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Backend.Data.Components
{
    public class ResourceData
    {
        public readonly MaterialData Material;
        public readonly ResourceType ResourceType;
        public readonly double Factor;

        internal ResourceData(FioResource poco)
        {
            Material = MaterialData.AllItemsByPocoId[poco.MaterialId];
            Factor = poco.Factor;
            ResourceType = Enum.Parse<ResourceType>(poco.ResourceType, true);
        }
    }
}