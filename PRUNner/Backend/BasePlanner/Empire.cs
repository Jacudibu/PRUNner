using System;
using System.Collections.ObjectModel;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Backend.BasePlanner
{
    public class Empire : ReactiveObject
    {
        public Headquarters Headquarters { get; } = new();
        public PriceOverrides PriceOverrides { get; } = new();
        public PriceDataPreferences PriceDataPreferences { get; } = PriceDataPreferences.CreateDefault();
        
        public ObservableCollection<PlanetaryBase> PlanetaryBases { get; } = new();

        public Empire()
        {
            PriceOverrides.OnPriceUpdate += OnPriceDataUpdate;
            PriceDataPreferences.PriceDataQueryPreferences.CollectionChanged += OnPriceDataUpdate;
            PriceDataPreferences.PropertyChanged += OnPriceDataUpdate;
        }

        public PlanetaryBase AddPlanetaryBase(PlanetData planet)
        {
            var planetaryBase = new PlanetaryBase(this, planet);
            PlanetaryBases.Add(planetaryBase);
            return planetaryBase;
        }

        public void RemovePlanetaryBase(PlanetaryBase planetaryBase)
        {
            PlanetaryBases.Remove(planetaryBase);
        }

        private void OnPriceDataUpdate(object? sender, EventArgs e)
        {
            OnPriceDataUpdate();
        }
        
        public void OnPriceDataUpdate()
        {
            foreach (var planetaryBase in PlanetaryBases)
            {
                planetaryBase.OnPriceDataUpdate();
            }
        }
    }
}