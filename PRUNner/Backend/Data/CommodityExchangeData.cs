using FIOImport.Pocos;
using PRUNner.Backend.Data.BaseClasses;

namespace PRUNner.Backend.Data
{
    public class CommodityExchangeData : GameData<CommodityExchangeData, FioCommodityExchangeStation>
    {
        public string Name { get; private set; }
        public string NaturalId { get; private set; }
        public SystemData System { get; private set; }
        
        internal override string GetFioIdFromPoco(FioCommodityExchangeStation poco) => poco.ComexId;
        internal override string GetIdFromPoco(FioCommodityExchangeStation poco) => poco.ComexCode;

        internal override void PostProcessData(FioCommodityExchangeStation poco)
        {
            Name = poco.Name;
            NaturalId = poco.NaturalId;
            System = SystemData.GetOrThrow(poco.SystemNaturalId);
        }
    }
}