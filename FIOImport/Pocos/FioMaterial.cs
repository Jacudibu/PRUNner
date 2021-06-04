using System;
// ReSharper disable All

namespace FIOImport.POCOs
{
    public class FioMaterial
    {
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string MatId { get; set; }
        public string Ticker { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public string UserNameSubmitted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}