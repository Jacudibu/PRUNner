using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models.BasePlanner
{
    public class PlanetBuildingProductionElement : ReactiveObject
    {
        public List<MaterialIO> Inputs { get; }
        public List<MaterialIO> Outputs { get; }
        [Reactive] public string DurationString { get; private set; } = null!;
        [Reactive] public long Duration { get; private set; }

        private readonly long _baseDurationMs;
        

        public PlanetBuildingProductionElement(ProductionData productionData)
        {
            Inputs = productionData.Inputs.ToList();
            Outputs = productionData.Outputs.ToList();
            _baseDurationMs = productionData.DurationInMilliseconds;
            ParseDurationString();
        }

        private const long ColTime = 6 * 60 * 60 * 1000;
        private const long ExtTime = 12 * 60 * 60 * 1000;
        private const long RigTime = 4 * 60 * 60 * 1000 + 48 * 60 * 1000;
        
        private const double MsPerDay = 24 * 60 * 60 * 1000;
        private const double ColFactor = ColTime / MsPerDay;
        private const double ExtFactor = ExtTime / MsPerDay;
        private const double RigFactor = RigTime / MsPerDay;
        
        public PlanetBuildingProductionElement(ResourceData resourceData)
        {
            var factor = resourceData.ResourceType switch
            {
                ResourceType.Gaseous => ColFactor,
                ResourceType.Liquid => RigFactor,
                ResourceType.Mineral => ExtFactor,
                _ => throw new ArgumentOutOfRangeException()
            };

            var dailyProduction = resourceData.CalculateDailyProduction(1);
            var realAmount = (int) Math.Ceiling(dailyProduction * factor);
            var timeForOne = MsPerDay / dailyProduction;
            var timeForAll = realAmount * timeForOne;
            _baseDurationMs = (long) Math.Round(timeForAll);
            
            Inputs = new List<MaterialIO>();
            Outputs = new List<MaterialIO>() {new(resourceData.Material, realAmount) };
            ParseDurationString();
        }

        private void ParseDurationString()
        {
            var timespan = TimeSpan.FromMilliseconds(_baseDurationMs);

            var builder = new StringBuilder();
            if (timespan.Days > 0)
            {
                builder.Append(timespan.Days);
                builder.Append("d ");
            }
            
            if (timespan.Hours > 0)
            {
                builder.Append(timespan.Hours);
                builder.Append("h ");
            }
            
            if (timespan.Minutes > 0)
            {
                builder.Append(timespan.Minutes);
                builder.Append('m');
            }

            DurationString = builder.ToString();
        }
    }
}