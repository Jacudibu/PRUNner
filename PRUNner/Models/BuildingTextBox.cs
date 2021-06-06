using Avalonia.Data;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Models
{
    public class BuildingTextBox : ReactiveObject
    {
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
                    
                value = value.ToUpper();
                Building = BuildingData.Get(value);
                if (Building == null)
                {
                    throw new DataValidationException("Building " + value + " does not exist. :(");
                }
                        
                this.RaiseAndSetIfChanged(ref _buildingName, value);
            }
        }

        public BuildingData? Building { get; private set; }
    }
}