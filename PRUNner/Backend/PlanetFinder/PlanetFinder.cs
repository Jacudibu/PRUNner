using System.Collections.Generic;
using System.Linq;
using PRUNner.Backend.Data;

namespace PRUNner.Backend.PlanetFinder
{
    public static class PlanetFinder
    {
        public static IEnumerable<PlanetFinderSearchResult> Find(FilterCriteria filterCriteria, string[] resourceFilter, OptionalPlanetFinderData optionalData)
        {
            return PlanetData.GetAll()
                .Where(filterCriteria.DoesPlanetFitCriteria)
                .Where(x => DoesPlanetHaveAllResources(x, resourceFilter))
                .Select(x => new PlanetFinderSearchResult(x, resourceFilter, optionalData));            
        }
        
        public static IEnumerable<PlanetFinderSearchResult> Find(FilterCriteria filterCriteria, MaterialData[] resourceFilter, OptionalPlanetFinderData optionalData)
        {
            return PlanetData.GetAll()
                .Where(filterCriteria.DoesPlanetFitCriteria)
                .Where(x => DoesPlanetHaveAllResources(x, resourceFilter))
                .Select(x => new PlanetFinderSearchResult(x, resourceFilter, optionalData));
        }

        private static bool DoesPlanetHaveAllResources(PlanetData planet, IEnumerable<MaterialData> resources)
        {
            return resources.All(planet.HasResource);
        }        
        
        private static bool DoesPlanetHaveAllResources(PlanetData planet, IEnumerable<string> resources)
        {
            return resources.All(planet.HasResource);
        }
    }
}