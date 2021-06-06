using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Models.BasePlanner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PlanetaryBaseInfrastructure : ReactiveObject
    {
        public PlanetBuilding CM { get; } = new(BuildingData.GetOrThrow(Names.Buildings.CM));

        public PlanetBuilding HB1 { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HB1));
        public PlanetBuilding HB2 { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HB2));
        public PlanetBuilding HB3 { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HB3));
        public PlanetBuilding HB4 { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HB4));
        public PlanetBuilding HB5 { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HB5));

        public PlanetBuilding HBB { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HBB));
        public PlanetBuilding HBC { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HBC));
        public PlanetBuilding HBM { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HBM));
        public PlanetBuilding HBL { get; } = new(BuildingData.GetOrThrow(Names.Buildings.HBL)); 
        public PlanetBuilding STO { get; } = new(BuildingData.GetOrThrow(Names.Buildings.STO));

        public PlanetaryBaseInfrastructure()
        {
            All = new[]
            {
                CM,
                HB1, HB2, HB3, HB4, HB5,
                HBB, HBC, HBM, HBL,
                STO
            }.ToImmutableList();
            
            CM.Add();
        }

        public readonly ImmutableList<PlanetBuilding> All;
    }
}