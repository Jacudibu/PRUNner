using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json.Linq;
using PRUNner.Backend.BasePlanner;

namespace PRUNner.Backend.UserDataParser
{
    public static class UserDataWriter
    {
        public static void Save(PlanetaryBase planetaryBase)
        {
            Directory.CreateDirectory(UserDataPaths.UserDataFolder);
            File.WriteAllText(UserDataPaths.UserDataFolder + UserDataPaths.PlanetFile, WritePlanetaryBase(planetaryBase).ToString());
        }
        
        public static JToken WritePlanetaryBase(PlanetaryBase planetaryBase)
        {
            JObject result = new();
            
            result.Add(nameof(PlanetaryBase.Planet), planetaryBase.Planet.Id);
            
            result.Add(nameof(PlanetaryBase.InfrastructureBuildings), WriteInfrastructureBuildings(planetaryBase.InfrastructureBuildings));
            result.Add(nameof(PlanetaryBase.ProductionBuildings), WriteProductionBuildings(planetaryBase.ProductionBuildings));
            result.Add(nameof(PlanetaryBase.ExpertAllocation), WriteExpertAllocation(planetaryBase.ExpertAllocation));
            result.Add(nameof(PlanetaryBase.ProvidedConsumables), WriteProvidedConsumables(planetaryBase.ProvidedConsumables));

            return result;
        }

        private static JToken WriteProductionBuildings(ObservableCollection<PlanetBuilding> planetProductionBuildings)
        {
            var result = new JArray();

            foreach (var building in planetProductionBuildings)
            {
                var buildingObject = new JObject();
                buildingObject.Add(nameof(PlanetBuilding.Building), building.Building.Ticker);
                buildingObject.Add(nameof(PlanetBuilding.Amount), building.Amount);

                var productionArray = new JArray();
                foreach (var production in building.Production)
                {
                    var productionObject = new JObject();
                    
                    productionObject.Add(nameof(PlanetBuildingProductionQueueElement.Percentage), production.Percentage);
                    productionObject.Add(nameof(PlanetBuildingProductionQueueElement.ActiveRecipe), production.ActiveRecipe?.RecipeName);
                    
                    productionArray.Add(productionObject);
                }
                buildingObject.Add(nameof(PlanetBuilding.Production), productionArray);

                result.Add(buildingObject);
            }

            return result;
        }

        private static JToken WriteProvidedConsumables(ProvidedConsumables planetProvidedConsumables)
        {
            var result = new JObject();

            result.Add(nameof(ProvidedConsumables.DW), planetProvidedConsumables.DW);
            result.Add(nameof(ProvidedConsumables.RAT), planetProvidedConsumables.RAT);
            result.Add(nameof(ProvidedConsumables.OVE), planetProvidedConsumables.OVE);
            result.Add(nameof(ProvidedConsumables.EXO), planetProvidedConsumables.EXO);
            result.Add(nameof(ProvidedConsumables.PT), planetProvidedConsumables.PT);
            result.Add(nameof(ProvidedConsumables.MED), planetProvidedConsumables.MED);
            result.Add(nameof(ProvidedConsumables.HMS), planetProvidedConsumables.HMS);
            result.Add(nameof(ProvidedConsumables.SCN), planetProvidedConsumables.SCN);
            result.Add(nameof(ProvidedConsumables.FIM), planetProvidedConsumables.FIM);
            result.Add(nameof(ProvidedConsumables.HSS), planetProvidedConsumables.HSS);
            result.Add(nameof(ProvidedConsumables.PDA), planetProvidedConsumables.PDA);
            result.Add(nameof(ProvidedConsumables.MEA), planetProvidedConsumables.MEA);
            result.Add(nameof(ProvidedConsumables.LC), planetProvidedConsumables.LC);
            result.Add(nameof(ProvidedConsumables.WS), planetProvidedConsumables.WS);
            result.Add(nameof(ProvidedConsumables.COF), planetProvidedConsumables.COF);
            result.Add(nameof(ProvidedConsumables.PWO), planetProvidedConsumables.PWO);
            result.Add(nameof(ProvidedConsumables.KOM), planetProvidedConsumables.KOM);
            result.Add(nameof(ProvidedConsumables.REP), planetProvidedConsumables.REP);
            result.Add(nameof(ProvidedConsumables.ALE), planetProvidedConsumables.ALE);
            result.Add(nameof(ProvidedConsumables.SC), planetProvidedConsumables.SC);
            result.Add(nameof(ProvidedConsumables.GIN), planetProvidedConsumables.GIN);
            result.Add(nameof(ProvidedConsumables.VG), planetProvidedConsumables.VG);
            result.Add(nameof(ProvidedConsumables.WIN), planetProvidedConsumables.WIN);
            result.Add(nameof(ProvidedConsumables.NST), planetProvidedConsumables.NST);

            return result;
        }


        public static JToken WriteInfrastructureBuildings(PlanetaryBaseInfrastructure infrastructure)
        {
            var obj = new JObject();
            
            obj.Add(nameof(PlanetaryBaseInfrastructure.HB1), infrastructure.HB1.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HB2), infrastructure.HB2.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HB3), infrastructure.HB3.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HB4), infrastructure.HB4.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HB5), infrastructure.HB5.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HBB), infrastructure.HBB.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HBC), infrastructure.HBC.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HBM), infrastructure.HBM.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.HBL), infrastructure.HBL.Amount);
            obj.Add(nameof(PlanetaryBaseInfrastructure.STO), infrastructure.STO.Amount);

            return obj;
        }
        
        public static JToken WriteExpertAllocation(ExpertAllocation expertAllocation) 
        {
            var obj = new JObject();
        
            obj.Add(nameof(ExpertAllocation.Agriculture), expertAllocation.Agriculture.Count);
            obj.Add(nameof(ExpertAllocation.Chemistry), expertAllocation.Chemistry.Count);
            obj.Add(nameof(ExpertAllocation.Construction), expertAllocation.Construction.Count);
            obj.Add(nameof(ExpertAllocation.Electronics), expertAllocation.Electronics.Count);
            obj.Add(nameof(ExpertAllocation.FoodIndustries), expertAllocation.FoodIndustries.Count);
            obj.Add(nameof(ExpertAllocation.FuelRefining), expertAllocation.FuelRefining.Count);
            obj.Add(nameof(ExpertAllocation.Manufacturing), expertAllocation.Manufacturing.Count);
            obj.Add(nameof(ExpertAllocation.Metallurgy), expertAllocation.Metallurgy.Count);
            obj.Add(nameof(ExpertAllocation.ResourceExtraction), expertAllocation.ResourceExtraction.Count);
        
            return obj;
        }
    }
}