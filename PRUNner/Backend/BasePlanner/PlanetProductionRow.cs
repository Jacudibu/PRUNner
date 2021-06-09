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
                Balance -= value;
                this.RaiseAndSetIfChanged(ref _inputs, value);
            }
        }

        private double _outputs;

        public double Outputs
        {
            get => _outputs;
            set
            {
                Balance += value;
                this.RaiseAndSetIfChanged(ref _outputs, value);
            }
        }

        private double _balance;
        public double Balance
        {
            get => _balance;
            private set
            {
                this.RaiseAndSetIfChanged(ref _balance, value);
                Value = Balance * Material.PriceData.NC1.Average;
            }
        }

        [Reactive] public double Value { get; private set; }
    }
}