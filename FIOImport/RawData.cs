using FIOImport.Pocos;
using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;

namespace FIOImport
{
    public class RawData
    {
        public readonly FioBuilding[] AllBuildings;
        public readonly FioMaterial[] AllMaterials;
        public readonly FioPlanet[] AllPlanets;
        public readonly FioSystem[] AllSystems;
        
        internal RawData(FioBuilding[] allBuildings, FioMaterial[] allMaterials, FioPlanet[] allPlanets, FioSystem[] allSystems)
        {
            AllBuildings = allBuildings;
            AllMaterials = allMaterials;
            AllPlanets = allPlanets;
            AllSystems = allSystems;
        }
    }
}