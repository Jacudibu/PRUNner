using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    public class PlanetProductionRow : ReactiveObject
    {
        private double _outputs;
        private double _inputs;

        public PlanetProductionRow(MaterialData material)
        {
            Material = material;
        }

        public MaterialData Material { get; }

        public double Inputs
        {
            get => _inputs;
            set
            {
                Balance -= value;
                this.RaiseAndSetIfChanged(ref _inputs, value);
            }
        }

        public double Outputs
        {
            get => _outputs;
            set
            {
                Balance += value;
                this.RaiseAndSetIfChanged(ref _outputs, value);
            }
        }

        [Reactive] public double Balance { get; private set; } = 0;
    }
}