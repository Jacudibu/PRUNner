using System;
using System.Collections.Generic;

namespace FIOImport.Pocos
{
    public class FioConnection
    {
        public string SystemConnectionId { get; set; }
        public string ConnectingId { get; set; }
    }
    
    public class FioSystem
    {
        public List<FioConnection> Connections { get; set; }
        public string SystemId { get; set; }
        public string Name { get; set; }
        public string NaturalId { get; set; }
        public string Type { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }
        public string SectorId { get; set; }
        public string SubSectorId { get; set; }
        public object UserNameSubmitted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}