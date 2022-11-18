using PRUNner.App.Models;
using PRUNner.Backend.BasePlanner;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        public PlanetaryBase ActiveBase { get; private set; }

        public BuildingTextBox AddBuildingTextBox { get; } = new();
        
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

            ActiveBase.ShoppingCart.AddBuilding(AddBuildingTextBox.Building);
        }
    }
}