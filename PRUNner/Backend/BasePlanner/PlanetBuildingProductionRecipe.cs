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
    public class PlanetBuildingProductionRecipe : ReactiveObject
    {
        public PlanetBuilding Source { get; }
        public string RecipeName { get; }
        public List<MaterialIO> Inputs { get; }
        public List<MaterialIO> Outputs { get; }
        [Reactive] public string DurationString { get; private set; } = null!;
        [Reactive] public double PaybackPeriod { get; private set; }

        private double _durationInMilliseconds;
        private readonly double _baseDurationMs;

        public PlanetBuildingProductionRecipe(PlanetBuilding source, ProductionData productionData)
        {
            Source = source;
            RecipeName = productionData.RecipeName;
            Inputs = productionData.Inputs.ToList();
            Outputs = productionData.Outputs.ToList();
            _baseDurationMs = productionData.DurationInMilliseconds;
            UpdateProductionEfficiency(1);
        }

        private const long ColTime = 6 * 60 * 60 * 1000;
        private const long ExtTime = 12 * 60 * 60 * 1000;
        private const long RigTime = 4 * 60 * 60 * 1000 + 48 * 60 * 1000;
        
        private const double ColFactor = ColTime / Constants.MsPerDay;
        private const double ExtFactor = ExtTime / Constants.MsPerDay;
        private const double RigFactor = RigTime / Constants.MsPerDay;
        
        public PlanetBuildingProductionRecipe(PlanetBuilding source, ResourceData resourceData)
        {
            Source = source;
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
            var timeForOne = Constants.MsPerDay / dailyProduction;
            var timeForAll = realAmount * timeForOne;
            _baseDurationMs = timeForAll;
            
            Inputs = new List<MaterialIO>();
            Outputs = new List<MaterialIO>() {new(resourceData.Material, realAmount, timeForAll) };
            UpdateProductionEfficiency(1);
        }

        public void UpdateProductionEfficiency(double efficiencyFactor)
        {
            if (efficiencyFactor <= 0)
            {
                DurationString = "∞";
                return;
            }

            _durationInMilliseconds = _baseDurationMs / efficiencyFactor;
            var timespan = TimeSpan.FromMilliseconds(_durationInMilliseconds);

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

        public void OnPriceDataUpdate()
        {
            if (!IsThereAnyWorkforce())
            {
                PaybackPeriod = double.PositiveInfinity;
                return;
            }
            
            var inputPurchasePrice = Inputs.Sum(x => x.Amount * x.Material.PriceData.GetPrice(Source.PlanetaryBase.Empire, Source.PlanetaryBase));
            var outputSalesPrice = Outputs.Sum(x => x.Amount * x.Material.PriceData.GetPrice(Source.PlanetaryBase.Empire, Source.PlanetaryBase));
            var workforceUpkeepCost = CalculateUpkeepCost();
            
            var runsPerDay = Constants.MsPerDay / _durationInMilliseconds;
            var dailyProfit = (outputSalesPrice - inputPurchasePrice) * runsPerDay - workforceUpkeepCost;
            
            if (dailyProfit <= 0)
            {
                PaybackPeriod = double.PositiveInfinity;
            }
            else
            {
                PaybackPeriod = Source.BuildingCost / (dailyProfit - Source.DailyCostForRepairs);

                if (PaybackPeriod < 0)
                {
                    PaybackPeriod = double.PositiveInfinity;
                }
            }
        }

        private bool IsThereAnyWorkforce()
        {
            return 0 <
                   Source.PlanetaryBase.FilledJobRatio.Pioneers * Source.Building.Workforce.Pioneers +
                   Source.PlanetaryBase.FilledJobRatio.Settlers * Source.Building.Workforce.Settlers +
                   Source.PlanetaryBase.FilledJobRatio.Technicians * Source.Building.Workforce.Technicians +
                   Source.PlanetaryBase.FilledJobRatio.Engineers * Source.Building.Workforce.Engineers +
                   Source.PlanetaryBase.FilledJobRatio.Scientists * Source.Building.Workforce.Scientists;
        }

        private double CalculateUpkeepCost()
        {
            var result = 0d;
            result += Source.PlanetaryBase.FilledJobRatio.Pioneers * Source.PlanetaryBase.WorkforceUpkeepCosts.Pioneers * Source.Building.Workforce.Pioneers;
            result += Source.PlanetaryBase.FilledJobRatio.Settlers * Source.PlanetaryBase.WorkforceUpkeepCosts.Settlers * Source.Building.Workforce.Settlers;
            result += Source.PlanetaryBase.FilledJobRatio.Technicians * Source.PlanetaryBase.WorkforceUpkeepCosts.Technicians * Source.Building.Workforce.Technicians;
            result += Source.PlanetaryBase.FilledJobRatio.Engineers * Source.PlanetaryBase.WorkforceUpkeepCosts.Engineers * Source.Building.Workforce.Engineers;
            result += Source.PlanetaryBase.FilledJobRatio.Scientists * Source.PlanetaryBase.WorkforceUpkeepCosts.Scientists * Source.Building.Workforce.Scientists;
            return result;
        }

        public override string ToString()
        {
            return RecipeName;
        }
    }
}