using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetBuildingProductionElement : ReactiveObject
    {
        public readonly string RecipeName;
        public List<MaterialIO> Inputs { get; }
        public List<MaterialIO> Outputs { get; }
        [Reactive] public string DurationString { get; private set; } = null!;
        [Reactive] public double DurationInMilliseconds { get; private set; }

        private readonly long _baseDurationMs;
        

        public PlanetBuildingProductionElement(ProductionData productionData)
        {
            RecipeName = productionData.RecipeName;
            Inputs = productionData.Inputs.ToList();
            Outputs = productionData.Outputs.ToList();
            _baseDurationMs = productionData.DurationInMilliseconds;
            UpdateProductionEfficiency(1);
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
            RecipeName = resourceData.Material.Ticker;
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
            UpdateProductionEfficiency(1);
        }

        private void ParseDurationString()
        {
            var timespan = TimeSpan.FromMilliseconds(DurationInMilliseconds);

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

        public void UpdateProductionEfficiency(double efficiencyFactor)
        {
            if (efficiencyFactor == 0)
            {
                DurationInMilliseconds = double.MaxValue;
                DurationString = "∞";
            }
            else
            {
                DurationInMilliseconds = _baseDurationMs * (2 - efficiencyFactor);
                ParseDurationString();
            }
        }
    }
}