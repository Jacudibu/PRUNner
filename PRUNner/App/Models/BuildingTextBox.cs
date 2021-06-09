using Avalonia.Data;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Enums;
using ReactiveUI;

namespace PRUNner.App.Models
{
    public class BuildingTextBox : ReactiveObject
    {
        public BuildingData? Building { get; private set; }

        private string _buildingName = "";
        public string BuildingName
        {
            get => _buildingName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Building = null;
                    this.RaiseAndSetIfChanged(ref _buildingName, "");
                    return;
                }
                    
                Building = BuildingData.Get(value);
                if (Building == null || Building.Category < 0)
                {
                    Building = null;
                    throw new DataValidationException("Building " + value + " does not exist. :(");
                }

                if (Building.Category == BuildingCategory.Infrastructure)
                {
                    Building = null;
                    throw new DataValidationException("Please don't add Infrastructure Buildings here.");
                }
                        
                this.RaiseAndSetIfChanged(ref _buildingName, value);
            }
        }

    }
}