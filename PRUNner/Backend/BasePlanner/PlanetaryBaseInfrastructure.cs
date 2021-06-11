using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Backend.BasePlanner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PlanetaryBaseInfrastructure : ReactiveObject
    {
        public PlanetBuilding CM { get; } 

        public PlanetBuilding HB1 { get; } 
        public PlanetBuilding HB2 { get; } 
        public PlanetBuilding HB3 { get; } 
        public PlanetBuilding HB4 { get; } 
        public PlanetBuilding HB5 { get; } 

        public PlanetBuilding HBB { get; } 
        public PlanetBuilding HBC { get; } 
        public PlanetBuilding HBM { get; } 
        public PlanetBuilding HBL { get; } 
        public PlanetBuilding STO { get; } 

        public PlanetaryBaseInfrastructure(PlanetaryBase planetaryBase)
        {
            CM = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.CM));
            HB1 = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HB1));
            HB2 = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HB2));
            HB3 = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HB3));
            HB4 = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HB4));
            HB5 = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HB5));
            HBB = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HBB));
            HBC = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HBC));
            HBM = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HBM));
            HBL = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.HBL)); 
            STO = PlanetBuilding.FromInfrastructureBuilding(planetaryBase, BuildingData.GetOrThrow(Names.Buildings.STO));
            
            All = new[]
            {
                CM,
                HB1, HB2, HB3, HB4, HB5,
                HBB, HBC, HBM, HBL,
                STO
            }.ToImmutableList();

            CM.Amount = 1;
        }

        public readonly ImmutableList<PlanetBuilding> All;
    }
}