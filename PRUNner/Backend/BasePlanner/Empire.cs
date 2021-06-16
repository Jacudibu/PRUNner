using System.Collections.ObjectModel;
using PRUNner.Backend.Data;

namespace PRUNner.Backend.BasePlanner
{
    public class Empire
    {
        public Headquarters Headquarters { get; } = new();
        public PriceOverrides PriceOverrides { get; } = new();
        
        public ObservableCollection<PlanetaryBase> PlanetaryBases { get; } = new();

        public Empire()
        {
            PriceOverrides.OnPriceUpdate += OnPriceDataUpdate;
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

        public void OnPriceDataUpdate()
        {
            foreach (var planetaryBase in PlanetaryBases)
            {
                planetaryBase.OnPriceDataUpdate();
            }
        }
    }
}