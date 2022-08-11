using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NLog;

namespace PRUNner.Backend.Data.BaseClasses
{
    public abstract class GameData<TData, TPoco> where TData : GameData<TData, TPoco>, new()
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
            AllItemsWithoutAliases = AllItems.Values.OrderBy(x => x.Id).ToImmutableArray();
            foreach (var value in AllItemsByPocoId.Values)
            {
                value.PostProcessData(pocos.Single(x => value.GetFioIdFromPoco(x).Equals(value.FioId)));
            }
        }
        
        public static void CreateFrom(TPoco poco)
        {
            var result = new TData();
            result.Id = result.GetIdFromPoco(poco).ToUpper();
            result.FioId = result.GetFioIdFromPoco(poco);
            
            // Very duct-tapey solution in hopes of finding the cause for parsing issues on OSX. TODO: remove the if/else and error logging once the stuff is fixed
            if (result.Id != null)
            {
                AllItems[result.Id] = result;
            }
            else
            {
                Logger.Warn("Something went wrong when loading an item by it's Id; Associated poco.Id is " + result.FioId + ". If you see this, please tell me the Id (and pray it's not null) so I can fix it. :D");
            }
            if (result.FioId != null)
            {
                AllItemsByPocoId[result.FioId] = result;
            }
            else
            {
                Logger.Warn("Something went wrong when loading an item by it's poco.Id; Associated Id is " + result.Id + ". If you see this, please tell me the Id (and pray it's not null) so I can fix it. :D");
            }
            
        }

        public static TData? Get(string id)
        {
            return AllItems.TryGetValue(id.ToUpper(), out var result) ? result : null;
        }

        protected void AddAlias(TData obj, string alias)
        {
            AllItems[alias.ToUpper()] = obj;
        }
        
        public static TData GetOrThrow(string id)
        {
            return AllItems[id.ToUpper()];
        }

        public static ImmutableArray<TData> GetAll()
        {
            return AllItemsWithoutAliases;
        }

        public static ImmutableArray<TData> GetAllProperty => GetAll();
        
        public override string ToString()
        {
            return Id;
        }
    }
}