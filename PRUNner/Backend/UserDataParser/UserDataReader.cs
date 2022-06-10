using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.BasePlanner.ShoppingCart;
using PRUNner.Backend.Data;
using PRUNner.Backend.Enums;

namespace PRUNner.Backend.UserDataParser
{
    public static class UserDataReader
    {
        private static JArray? _v03GlobalPricePreferenceSettings; // For backwards compatibility with v0.3.14 and lower. Can be removed at some point.
        
        public static Empire Load()
        {
            if (!File.Exists(UserDataPaths.UserDataFolder + UserDataPaths.EmpireFile))
            {
                return new Empire();
            }
            
            var jObject = JObject.Parse(File.ReadAllText(UserDataPaths.UserDataFolder + UserDataPaths.EmpireFile));
            var result = ReadEmpire((JObject) jObject[nameof(Empire)]!);
            ReadGlobalSettings((JObject?) jObject[nameof(GlobalSettings)]);

            return result;
        }

        public static void ReadGlobalSettings(JObject? jObject)
        {
            if (jObject == null)
            {
                return;
            }

            GlobalSettings.IgnoreUpdateTag = jObject[nameof(GlobalSettings.IgnoreUpdateTag)]?.ToString();
            _v03GlobalPricePreferenceSettings = (JArray?) jObject["PriceDataPreferenceOrder"];
        }

        public static Empire ReadEmpire(JObject jObject)
        {
            var result = new Empire();

            ReadHeadquarters((JObject?) jObject[nameof(Empire.Headquarters)], result.Headquarters);
            ReadPlanetaryBases((JArray?) jObject[nameof(Empire.PlanetaryBases)], result);
            ReadPriceOverrides((JArray?) jObject[nameof(Empire.PriceOverrides)], result.PriceOverrides);

            if (_v03GlobalPricePreferenceSettings != null)
            {
                result.PriceDataPreferences.ParseJson(_v03GlobalPricePreferenceSettings);
            }
            else
            {
                result.PriceDataPreferences.ParseJson(jObject[nameof(Empire.PriceDataPreferences)]);
            }
            
            return result;
        }

        private static void ReadPriceOverrides(JArray? jArray, PriceOverrides priceOverrides)
        {
            priceOverrides.Clear();
            
            if (jArray == null)
            {
                return;
            }
            
            foreach (var jObject in jArray.Cast<JObject>())
            {
                var priceOverride = new PriceOverride();
                priceOverride.Ticker = jObject[nameof(PriceOverride.Ticker)]?.ToObject<string>() ?? "";
                priceOverride.Price = jObject[nameof(PriceOverride.Price)]?.ToObject<double>() ?? 0;
                priceOverrides.AddOverride(priceOverride);
            }
        }

        private static void ReadHeadquarters(JObject? jObject, Headquarters headquarters)
        {
            if (jObject == null)
            {
                return;
            }
            
            headquarters.Faction = Enum.Parse<Faction>(jObject[nameof(Headquarters.Faction)].Value<string>(), true);
            headquarters.UsedHQSlots = jObject[nameof(Headquarters.UsedHQSlots)]?.ToObject<int>() ?? 1;
            headquarters.TotalHQSlots = jObject[nameof(Headquarters.TotalHQSlots)]?.ToObject<int>() ?? 2;
        }

        private static void ReadPlanetaryBases(JArray? jArray, Empire empire)
        {
            empire.PlanetaryBases.Clear();

            if (jArray == null)
            {
                return;
            }
            
            foreach (var jObject in jArray.Cast<JObject>())
            {
                ReadPlanet(empire, jObject);
            }
        }

        public static PlanetaryBase ReadPlanet(Empire empire, JObject obj)
        {
            var result = empire.AddPlanetaryBase(PlanetData.Get(obj.GetValue(nameof(PlanetaryBase.Planet))!.ToObject<string>()!)!);
            result.BeginLoading();

            var cogcString = obj.GetValue(nameof(PlanetaryBase.CoGCBonus))?.ToObject<string>() ?? CoGCBonusType.None.ToString();
            result.AreaPermitIncreaseCount = obj.GetValue(nameof(PlanetaryBase.AreaPermitIncreaseCount))?.ToObject<int>() ?? 0;
            result.CoGCBonus = Enum.Parse<CoGCBonusType>(cogcString, true);
            result.CorpHQBonus = obj.GetValue(nameof(PlanetaryBase.CorpHQBonus))?.ToObject<bool>() ?? false;
            result.IncludeCoreModuleInColonyCosts = obj.GetValue(nameof(PlanetaryBase.IncludeCoreModuleInColonyCosts))?.ToObject<bool>() ?? false;

            ReadInfrastructureBuildings((JObject) obj[nameof(PlanetaryBase.InfrastructureBuildings)]!, result.InfrastructureBuildings);
            ReadProductionBuildings((JArray) obj[nameof(PlanetaryBase.ProductionBuildings)]!, result);
            ReadExpertAllocation((JObject) obj[nameof(PlanetaryBase.ExpertAllocation)]!, result.ExpertAllocation);
            ReadConsumableData((JObject) obj[nameof(PlanetaryBase.ProvidedConsumables)]!, result.ProvidedConsumables);
            ReadPriceOverrides((JArray?) obj[nameof(PlanetaryBase.PriceOverrides)], result.PriceOverrides);
            
            result.PriceDataPreferences.ParseJson(obj[nameof(PlanetaryBase.PriceDataPreferences)]);

            result.FinishLoading();

            ReadShoppingCart((JObject?) obj[nameof(PlanetaryBase.ShoppingCart)], result.ShoppingCart);

            return result;
        }

        private static void ReadShoppingCart(JObject? jObject, ShoppingCart shoppingCart)
        {
            if (jObject == null)
            {
                return;
            }

            var buildings = (JArray) jObject[nameof(ShoppingCart.Buildings)]!;
            foreach (var buildingObject in buildings.Cast<JObject>())
            {
                var ticker = buildingObject.GetValue(nameof(ShoppingCartBuilding.Building))!.ToObject<string>();
                var shoppingCartBuilding = shoppingCart.Buildings.SingleOrDefault(x => x.Building.Building.Ticker.Equals(ticker));
                if (shoppingCartBuilding != null)
                {
                    shoppingCartBuilding.PlannedAmount = buildingObject.GetValue(nameof(ShoppingCartBuilding.PlannedAmount))!.ToObject<int>();
                }
            }
            
            var materials = (JArray) jObject[nameof(ShoppingCart.Materials)]!;
            foreach (var materialObject in materials.Cast<JObject>())
            {
                var ticker = materialObject.GetValue(nameof(ShoppingCartMaterial.Material))!.ToObject<string>();
                var shoppingCartMaterial = shoppingCart.Materials.SingleOrDefault(x => x.Material.Ticker.Equals(ticker));
                if (shoppingCartMaterial != null)
                {
                    shoppingCartMaterial.Inventory = materialObject.GetValue(nameof(ShoppingCartMaterial.Inventory))!.ToObject<int>();
                }
            }
        }

        private static void ReadProductionBuildings(JArray buildingArray, PlanetaryBase planetaryBase)
        {
            planetaryBase.ProductionBuildings.Clear();

            foreach (var buildingObject in buildingArray.Cast<JObject>())
            {
                var building = planetaryBase.AddBuilding(BuildingData.GetOrThrow(buildingObject.GetValue(nameof(PlanetBuilding.Building))?.ToObject<string>() ?? ""));
                building.Amount = buildingObject.GetValue(nameof(PlanetBuilding.Amount))?.ToObject<int>() ?? 0;

                ReadAdvancedBuildingConfiguration(building, buildingObject.GetValue(nameof(PlanetBuilding.AdvancedBuildingConfiguration)));
                
                building.Production.Clear();
                foreach (var productionObject in buildingObject.GetValue(nameof(PlanetBuilding.Production))!.Cast<JObject>())
                {
                    var production = building.AddProduction();
                    production.Percentage = productionObject.GetValue(nameof(PlanetBuildingProductionQueueElement.Percentage))!.ToObject<double>();
                    var recipeName = productionObject.GetValue(nameof(PlanetBuildingProductionQueueElement.ActiveRecipe))!.ToObject<string>();

                    production.ActiveRecipe = building.AvailableRecipes!.SingleOrDefault(x => x.RecipeName.Equals(recipeName));
                }
            }
        }

        private static void ReadAdvancedBuildingConfiguration(PlanetBuilding building, JToken? config)
        {
            if (config == null)
            {
                return;
            }

            building.AdvancedBuildingConfiguration.ProductionLineAge = config[nameof(AdvancedBuildingConfiguration.ProductionLineAge)]?.ToObject<int>() ?? 0;
            building.AdvancedBuildingConfiguration.UseEfficiencyOverride = config[nameof(AdvancedBuildingConfiguration.UseEfficiencyOverride)]?.ToObject<bool>() ?? false;
            building.AdvancedBuildingConfiguration.EfficiencyOverride = config[nameof(AdvancedBuildingConfiguration.EfficiencyOverride)]?.ToObject<double>() ?? 100;
        }

        private static void ReadConsumableData(JObject jObject, ProvidedConsumables providedConsumables)
        {
            providedConsumables.DW = jObject.GetValue(nameof(ProvidedConsumables.DW))?.ToObject<bool>() ?? true;
            providedConsumables.RAT = jObject.GetValue(nameof(ProvidedConsumables.RAT))?.ToObject<bool>() ?? true;
            providedConsumables.OVE = jObject.GetValue(nameof(ProvidedConsumables.OVE))?.ToObject<bool>() ?? true;
            providedConsumables.EXO = jObject.GetValue(nameof(ProvidedConsumables.EXO))?.ToObject<bool>() ?? true;
            providedConsumables.PT = jObject.GetValue(nameof(ProvidedConsumables.PT))?.ToObject<bool>() ?? true;
            providedConsumables.MED = jObject.GetValue(nameof(ProvidedConsumables.MED))?.ToObject<bool>() ?? true;
            providedConsumables.HMS = jObject.GetValue(nameof(ProvidedConsumables.HMS))?.ToObject<bool>() ?? true;
            providedConsumables.SCN = jObject.GetValue(nameof(ProvidedConsumables.SCN))?.ToObject<bool>() ?? true;
            providedConsumables.FIM = jObject.GetValue(nameof(ProvidedConsumables.FIM))?.ToObject<bool>() ?? true;
            providedConsumables.HSS = jObject.GetValue(nameof(ProvidedConsumables.HSS))?.ToObject<bool>() ?? true;
            providedConsumables.PDA = jObject.GetValue(nameof(ProvidedConsumables.PDA))?.ToObject<bool>() ?? true;
            providedConsumables.MEA = jObject.GetValue(nameof(ProvidedConsumables.MEA))?.ToObject<bool>() ?? true;
            providedConsumables.LC = jObject.GetValue(nameof(ProvidedConsumables.LC))?.ToObject<bool>() ?? true;
            providedConsumables.WS = jObject.GetValue(nameof(ProvidedConsumables.WS))?.ToObject<bool>() ?? true;
            providedConsumables.COF = jObject.GetValue(nameof(ProvidedConsumables.COF))?.ToObject<bool>() ?? false;
            providedConsumables.PWO = jObject.GetValue(nameof(ProvidedConsumables.PWO))?.ToObject<bool>() ?? false;
            providedConsumables.KOM = jObject.GetValue(nameof(ProvidedConsumables.KOM))?.ToObject<bool>() ?? false;
            providedConsumables.REP = jObject.GetValue(nameof(ProvidedConsumables.REP))?.ToObject<bool>() ?? false;
            providedConsumables.ALE = jObject.GetValue(nameof(ProvidedConsumables.ALE))?.ToObject<bool>() ?? false;
            providedConsumables.SC = jObject.GetValue(nameof(ProvidedConsumables.SC))?.ToObject<bool>() ?? false;
            providedConsumables.GIN = jObject.GetValue(nameof(ProvidedConsumables.GIN))?.ToObject<bool>() ?? false;
            providedConsumables.VG = jObject.GetValue(nameof(ProvidedConsumables.VG))?.ToObject<bool>() ?? false;
            providedConsumables.WIN = jObject.GetValue(nameof(ProvidedConsumables.WIN))?.ToObject<bool>() ?? false;
            providedConsumables.NST = jObject.GetValue(nameof(ProvidedConsumables.NST))?.ToObject<bool>() ?? false;
        }

        public static void ReadInfrastructureBuildings(JObject obj, PlanetaryBaseInfrastructure infrastructure)
        {
            infrastructure.HB1.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB1))?.ToObject<int>() ?? 0;
            infrastructure.HB2.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB2))?.ToObject<int>() ?? 0;
            infrastructure.HB3.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB3))?.ToObject<int>() ?? 0;
            infrastructure.HB4.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB4))?.ToObject<int>() ?? 0;
            infrastructure.HB5.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HB5))?.ToObject<int>() ?? 0;
            infrastructure.HBB.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBB))?.ToObject<int>() ?? 0;
            infrastructure.HBC.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBC))?.ToObject<int>() ?? 0;
            infrastructure.HBM.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBM))?.ToObject<int>() ?? 0;
            infrastructure.HBL.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.HBL))?.ToObject<int>() ?? 0;
            infrastructure.STO.Amount = obj.GetValue(nameof(PlanetaryBaseInfrastructure.STO))?.ToObject<int>() ?? 0;
        }        
        
        public static void ReadExpertAllocation(JObject obj, ExpertAllocation expertAllocation)
        {
            expertAllocation.Agriculture.Count = obj.GetValue(nameof(ExpertAllocation.Agriculture))?.ToObject<int>() ?? 0;
            expertAllocation.Chemistry.Count = obj.GetValue(nameof(ExpertAllocation.Chemistry))?.ToObject<int>() ?? 0;
            expertAllocation.Construction.Count = obj.GetValue(nameof(ExpertAllocation.Construction))?.ToObject<int>() ?? 0;
            expertAllocation.Electronics.Count = obj.GetValue(nameof(ExpertAllocation.Electronics))?.ToObject<int>() ?? 0;
            expertAllocation.FoodIndustries.Count = obj.GetValue(nameof(ExpertAllocation.FoodIndustries))?.ToObject<int>() ?? 0;
            expertAllocation.FuelRefining.Count = obj.GetValue(nameof(ExpertAllocation.FuelRefining))?.ToObject<int>() ?? 0;
            expertAllocation.Manufacturing.Count = obj.GetValue(nameof(ExpertAllocation.Manufacturing))?.ToObject<int>() ?? 0;
            expertAllocation.Metallurgy.Count = obj.GetValue(nameof(ExpertAllocation.Metallurgy))?.ToObject<int>() ?? 0;
            expertAllocation.ResourceExtraction.Count = obj.GetValue(nameof(ExpertAllocation.ResourceExtraction))?.ToObject<int>() ?? 0;
        }
        
        
    }
}