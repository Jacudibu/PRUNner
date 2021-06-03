using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;

namespace FIOImport
{
    public class RawData
    {
        public readonly Building[] AllBuildings;
        public readonly Material[] AllMaterials;
        public readonly PlanetIdentifier[] AllPlanets;
        public readonly Planet[] AllPlanetsDetailed;
        
        internal RawData(Building[] allBuildings, Material[] allMaterials, PlanetIdentifier[] allPlanets, Planet[] allPlanetsDetailed)
        {
            AllBuildings = allBuildings;
            AllMaterials = allMaterials;
            AllPlanets = allPlanets;
            AllPlanetsDetailed = allPlanetsDetailed;
        }
    }
}