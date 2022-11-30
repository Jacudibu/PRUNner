using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend
{
    public class PriceOverride : ReactiveObject
    {
        [Reactive] public string Ticker { get; set; } 
        [Reactive] public double Price { get; set; }
        [Reactive] public bool IsEnabled { get; set; } = true;
    }
    
    public class PriceOverrides
    {
        public ObservableCollection<PriceOverride> OverrideList { get; } = new();

        private readonly Dictionary<string, double> _dictionary = new();

        public PriceOverrides()
        {
            AddNewOverride();
        }
        
        public void AddNewOverride()
        {
            var priceOverride = new PriceOverride();
            AddOverride(priceOverride);
        }

        public void AddOverride(PriceOverride priceOverride)
        {
            priceOverride.Changed.Subscribe(_ => UpdateDictionary());
            OverrideList.Add(priceOverride);
            UpdateDictionary();
        }

        public void RemoveOverride(PriceOverride priceOverride)
        {
            OverrideList.Remove(priceOverride);
            UpdateDictionary();
        }

        public void UpdateDictionary()
        {
            _dictionary.Clear();
            foreach (var priceOverride in OverrideList)
            {
                if (!priceOverride.IsEnabled)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(priceOverride.Ticker))
                {
                    continue;
                }
                
                _dictionary[priceOverride.Ticker.ToUpper()] = priceOverride.Price;
            }
            
            OnPriceUpdate?.Invoke();
        }
        
        public double? GetOverrideForTicker(string ticker)
        {
            if (_dictionary.TryGetValue(ticker, out var result))
            {
                return result;
            }

            return null;
        }

        public delegate void OnPriceUpdateDelegate();
        public event OnPriceUpdateDelegate? OnPriceUpdate;

        public void Clear()
        {
            OverrideList.Clear();
        }
    }
}