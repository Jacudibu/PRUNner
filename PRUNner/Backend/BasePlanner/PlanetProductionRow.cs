using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetProductionRow : ReactiveObject
    {
        private readonly PlanetaryBase _planetaryBase;
        
        public MaterialData Material { get; }

        private double _inputs;
        public double Inputs
        {
            get => _inputs;
            set
            {
                this.RaiseAndSetIfChanged(ref _inputs, value);
                UpdateComputedValues();
            }
        }

        private double _outputs;

        public double Outputs
        {
            get => _outputs;
            set
            {
                this.RaiseAndSetIfChanged(ref _outputs, value);
                UpdateComputedValues();
            }
        }
        [Reactive] public double Balance { get; private set; }

        [Reactive] public double Value { get; private set; }
        
        [Reactive] public double Volume { get; private set; }
        [Reactive] public double Weigth { get; private set; }

        public PlanetProductionRow(PlanetProductionTable table, MaterialData material)
        {
            _planetaryBase = table.PlanetaryBase;
            Material = material;
        }
        
        private void UpdateComputedValues()
        {
            Balance = Outputs - Inputs;
            Volume = Balance * Material.Volume;
            Weigth = Balance * Material.Weight;
            UpdatePriceData();
        }

        public void UpdatePriceData()
        {
            Value = Balance * Material.PriceData.GetPrice(_planetaryBase);
        }
    }
}