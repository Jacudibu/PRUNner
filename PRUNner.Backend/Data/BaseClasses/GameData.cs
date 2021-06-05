using System.Collections.Generic;
using System.Linq;

namespace PRUNner.Backend.Data.BaseClasses
{
    public abstract class GameData<TData, TPoco> where TData : GameData<TData, TPoco>, new()
    {
        public static readonly Dictionary<string, TData> AllItems = new Dictionary<string, TData>();
        internal static readonly Dictionary<string, TData> AllItemsByPocoId = new Dictionary<string, TData>();
        
        internal string FioId { get; private set; }
        public string Id { get; private set; }

        internal abstract string GetFioIdFromPoco(TPoco poco);
        internal abstract string GetIdFromPoco(TPoco poco);
        
        internal abstract void PostProcessData(TPoco poco);
        
        internal static void PostProcessData(TPoco[] pocos)
        {
            foreach (var value in AllItemsByPocoId.Values)
            {
                value.PostProcessData(pocos.Single(x => value.GetFioIdFromPoco(x).Equals(value.FioId)));
            }
        }
        
        public static void CreateFrom(TPoco poco)
        {
            var result = new TData();
            result.Id = result.GetIdFromPoco(poco);
            result.FioId = result.GetFioIdFromPoco(poco);
            AllItems[result.Id] = result;
            AllItemsByPocoId[result.FioId] = result;
        }

        public override string ToString()
        {
            return Id;
        }
    }
}