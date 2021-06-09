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
        public PlanetBuilding CM { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.CM));

        public PlanetBuilding HB1 { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HB1));
        public PlanetBuilding HB2 { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HB2));
        public PlanetBuilding HB3 { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HB3));
        public PlanetBuilding HB4 { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HB4));
        public PlanetBuilding HB5 { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HB5));

        public PlanetBuilding HBB { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HBB));
        public PlanetBuilding HBC { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HBC));
        public PlanetBuilding HBM { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HBM));
        public PlanetBuilding HBL { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.HBL)); 
        public PlanetBuilding STO { get; } = PlanetBuilding.FromInfrastructureBuilding(BuildingData.GetOrThrow(Names.Buildings.STO));

        public PlanetaryBaseInfrastructure()
        {
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