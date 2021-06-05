using System;
using System.Collections.Generic;
using System.Linq;
using FIOImport.Data.BaseClasses;
using FIOImport.Data.Enums;
using FIOImport.Pocos;

namespace FIOImport.Data
{
    public class SystemData : GameData<SystemData, FioSystem>
    {
        public string NaturalId { get; private set; }
        public string Name { get; private set; }
        public StarType StarType { get; private set; }
        public List<SystemData> Connections { get; private set; } = new List<SystemData>();
        public List<PlanetData> Planets { get; private set; } = new List<PlanetData>();
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }
        public double PositionZ { get; private set; }
        public string SectorId { get; private set; }
        public string SubSectorId { get; private set; }
        
        internal override string GetIdFromPoco(FioSystem poco) => poco.NaturalId;
        internal override string GetFioIdFromPoco(FioSystem poco) => poco.SystemId;

        internal override void PostProcessData(FioSystem poco)
        {
            foreach (var connection in poco.Connections)
            {
                if (Connections.Any(x => x.FioId.Equals(connection.Connection)))
                {
                    continue;
                }
                
                var connectedSystem = AllItemsByPocoId[connection.Connection];
                connectedSystem.Connections.Add(this);
                Connections.Add(connectedSystem);
            }

            NaturalId = poco.NaturalId;
            Name = poco.Name;
            StarType = Enum.Parse<StarType>(poco.Type, true);
            PositionX = poco.PositionX;
            PositionY = poco.PositionY;
            PositionZ = poco.PositionZ;
            SectorId = poco.SectorId;
            SubSectorId = poco.SubSectorId;
        }

        internal void AddPlanet(PlanetData planet)
        {
            Planets.Add(planet);
        }
    }
}