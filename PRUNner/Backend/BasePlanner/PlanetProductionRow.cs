using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetProductionRow : ReactiveObject
    {
        public PlanetProductionRow(MaterialData material)
        {
            Material = material;
        }

        public MaterialData Material { get; }

        private double _inputs;
        public double Inputs
        {
            get => _inputs;
            set
            {
                this.RaiseAndSetIfChanged(ref _inputs, value);
                Balance = _outputs - _inputs;
            }
        }

        private double _outputs;

        public double Outputs
        {
            get => _outputs;
            set
            {
                this.RaiseAndSetIfChanged(ref _outputs, value);
                Balance = _outputs - _inputs;
            }
        }

        private double _balance;
        public double Balance
        {
            get => _balance;
            private set
            {
                this.RaiseAndSetIfChanged(ref _balance, value);
                Value = _balance * Material.PriceData.NC1.Average;
            }
        }

        [Reactive] public double Value { get; private set; }
    }
}