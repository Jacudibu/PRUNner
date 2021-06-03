using FIOImport.POCOs;
using FIOImport.POCOs.Buildings;
using FIOImport.POCOs.Planets;

namespace FIOImport
{
    public class RawData
    {
        public readonly Building[] AllBuildings;
        public readonly Material[] AllMaterials;
        public readonly Planet[] AllPlanets;
        
        internal RawData(Building[] allBuildings, Material[] allMaterials, Planet[] allPlanets)
        {
            AllBuildings = allBuildings;
            AllMaterials = allMaterials;
            AllPlanets = allPlanets;
        }
    }
}