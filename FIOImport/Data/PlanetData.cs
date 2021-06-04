using System.Linq;
using FIOImport.Data.BaseClasses;
using FIOImport.Data.Components;
using FIOImport.POCOs.Planets;

namespace FIOImport.Data
{
    public class PlanetData : GameData<PlanetData, FioPlanet>
    {
        public ResourceData[] Resources { get; private set; }
        public BuildRequirementData[] ColonizationMaterials { get; private set; } 
        public string Name { get; private set; }
        public double Fertility { get; private set; }
        public double Gravity { get; private set; }
        public double Pressure { get; private set; }
        public double Radiation { get; private set; }

        internal override string GetIdFromPoco(FioPlanet poco) => poco.PlanetId;
        internal override string GetFioIdFromPoco(FioPlanet poco) => poco.PlanetNaturalId;

        internal override void PostProcessData(FioPlanet poco)
        {
            Name = poco.PlanetName;
            
            Fertility = poco.Fertility;
            Gravity = poco.Gravity;
            Pressure = poco.Pressure;
            Radiation = poco.Radiation;

            Resources = poco.Resources.Select(x => new ResourceData(x)).ToArray();
            ColonizationMaterials = poco.BuildRequirements.Select(x => new BuildRequirementData(x)).ToArray();
        }
    }
}