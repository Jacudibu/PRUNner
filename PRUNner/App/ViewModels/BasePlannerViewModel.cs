using PRUNner.App.Models;
using PRUNner.Backend.BasePlanner;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class BasePlannerViewModel : ViewModelBase
    {
        private PlanetaryBase _activeBase;
        public PlanetaryBase ActiveBase {
            get => _activeBase;
            private set
            {
                _activeBase = value;
                ShoppingCartViewModel.SetActiveBase(value);
            }
        }

        public BuildingTextBox AddBuildingTextBox { get; } = new();

        public ShoppingCartViewModel ShoppingCartViewModel { get; } = new();

        public void SetActiveBase(PlanetaryBase planetaryBase)
        {
            ActiveBase = planetaryBase;
            this.RaisePropertyChanged(nameof(ActiveBase));
        }

        public void AddBuilding()
        {
            if (AddBuildingTextBox.Building == null)
            {
                return;
            }

            ActiveBase.AddBuilding(AddBuildingTextBox.Building);
        }
    }
}