using System;
using System.Collections.Generic;

// ReSharper disable All

namespace FIOImport.POCOs.Planets
{
    public class FioPlanet
    {
        public List<FioResource> Resources { get; set; }
        public List<FioBuildRequirement> BuildRequirements { get; set; }
        public List<FioProductionFee> ProductionFees { get; set; }
        public List<object> COGCPrograms { get; set; }
        public List<object> COGCVotes { get; set; }
        public List<object> COGCUpkeep { get; set; }
        public string PlanetId { get; set; }
        public string PlanetNaturalId { get; set; }
        public string PlanetName { get; set; }
        public object Namer { get; set; }
        public long NamingDataEpochMs { get; set; }
        public bool Nameable { get; set; }
        public string SystemId { get; set; }
        public double Gravity { get; set; }
        public double MagneticField { get; set; }
        public double Mass { get; set; }
        public double MassEarth { get; set; }
        public double OrbitSemiMajorAxis { get; set; }
        public double OrbitEccentricity { get; set; }
        public double OrbitInclination { get; set; }
        public double OrbitRightAscension { get; set; }
        public double OrbitPeriapsis { get; set; }
        public int OrbitIndex { get; set; }
        public double Pressure { get; set; }
        public double Radiation { get; set; }
        public double Radius { get; set; }
        public double Sunlight { get; set; }
        public bool Surface { get; set; }
        public double Temperature { get; set; }
        public double Fertility { get; set; }
        public bool HasLocalMarket { get; set; }
        public bool HasChamberOfCommerce { get; set; }
        public bool HasWarehouse { get; set; }
        public bool HasAdministrationCenter { get; set; }
        public bool HasShipyard { get; set; }
        public string FactionCode { get; set; }
        public string FactionName { get; set; }
        public string GovernorId { get; set; }
        public string GovernorUserName { get; set; }
        public string GovernorCorporationId { get; set; }
        public string GovernorCorporationName { get; set; }
        public string GovernorCorporationCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string CollectorCode { get; set; }
        public double BaseLocalMarketFee { get; set; }
        public double LocalMarketFeeFactor { get; set; }
        public double WarehouseFee { get; set; }
        public string PopulationId { get; set; }
        public object COGCProgramStatus { get; set; }
        public int PlanetTier { get; set; }
        public string UserNameSubmitted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}