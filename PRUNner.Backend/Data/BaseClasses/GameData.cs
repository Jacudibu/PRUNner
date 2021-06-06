using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace PRUNner.Backend.Data.BaseClasses
{
    public abstract class GameData<TData, TPoco> where TData : GameData<TData, TPoco>, new()
    {
        protected static ImmutableArray<TData> AllItemsWithoutAliases; 
        protected static readonly Dictionary<string, TData> AllItems = new();
        internal static readonly Dictionary<string, TData> AllItemsByPocoId = new();
        
        internal string FioId { get; private set; }
        public string Id { get; private set; }

        internal abstract string GetFioIdFromPoco(TPoco poco);
        internal abstract string GetIdFromPoco(TPoco poco);
        
        internal abstract void PostProcessData(TPoco poco);
        
        internal static void PostProcessData(TPoco[] pocos)
        {
            AllItemsWithoutAliases = AllItems.Values.ToImmutableArray();
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

        public static TData? Get(string id)
        {
            return AllItems.TryGetValue(id, out var result) ? result : null;
        }

        protected void AddAlias(TData obj, string alias)
        {
            AllItems[alias] = obj;
        }
        
        public static TData GetOrThrow(string id)
        {
            return AllItems[id];
        }

        public static ImmutableArray<TData> GetAll()
        {
            return AllItemsWithoutAliases;
        }
        
        public override string ToString()
        {
            return Id;
        }
    }
}