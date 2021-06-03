using System;
using FIOImport.Data.Enums;
using FIOImport.POCOs.Planets;

namespace FIOImport.Data
{
    public class ResourceData
    {
        public readonly MaterialData Material;
        public readonly ResourceType ResourceType;
        public readonly double Factor;

        internal ResourceData(Resource resource)
        {
            Material = MaterialData.GetById(resource.MaterialId);
            Factor = resource.Factor;
            ResourceType = Enum.Parse<ResourceType>(resource.ResourceType, true);
        }
    }
}