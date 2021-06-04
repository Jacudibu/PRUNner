using System.Collections.Generic;
using System.Linq;
using FIOImport.Data;
using FIOImport.Data.Enums;

namespace PRUNner.Backend
{
    public static class PlanetFinder
    {
        public static IEnumerable<PlanetData> Find(IEnumerable<MaterialData> resourceFilter, bool excludeInfertile,
            bool excludeRocky, bool excludeGaseous, 
            bool excludeLowGravity, bool excludeHighGravity,
            bool excludeLowPressure, bool excludeHighPressure,
            bool excludeLowTemperature, bool excludeHighTemperature)
        {
            var result = PlanetData.AllItems.Values
                .Where(x => DoesPlanetHaveAllResources(x, resourceFilter));

            if (excludeRocky)
            {
                result = result.Where(x => x.Type != PlanetType.Rocky);
            }

            if (excludeGaseous)
            {
                result = result.Where(x => x.Type != PlanetType.Gaseous);
            }

            if (excludeLowGravity)
            {
                result = result.Where(x => !x.IsLowGravity());
            }
            
            if (excludeHighGravity)
            {
                result = result.Where(x => !x.IsHighGravity());
            }
            
            if (excludeLowPressure)
            {
                result = result.Where(x => !x.IsLowPressure());
            }
            
            if (excludeHighPressure)
            {
                result = result.Where(x => !x.IsHighPressure());
            }
            
            if (excludeLowTemperature)
            {
                result = result.Where(x => !x.IsLowTemperature());
            }
            
            if (excludeHighTemperature)
            {
                result = result.Where(x => !x.IsHighTemperature());
            }
            
            if (excludeInfertile)
            {
                result = result.Where(x => x.Fertility > -1);
            }

            return result;
        }

        private static bool DoesPlanetHaveAllResources(PlanetData planet, IEnumerable<MaterialData> resources)
        {
            return resources.All(planet.HasResource);
        }
    }
}