using Avalonia.Data;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Models
{
    public class MaterialTextBox : ReactiveObject
    {
        private string _materialName = "";

        public string MaterialName
        {
            get => _materialName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Material = null;
                    this.RaiseAndSetIfChanged(ref _materialName, "");
                    return;
                }

                value = value.ToUpper();
                Material = MaterialData.Get(value);
                if (Material == null)
                {
                    throw new DataValidationException("Material " + value + " does not exist. :(");
                }
                    
                this.RaiseAndSetIfChanged(ref _materialName, value);
            }
        }

        public MaterialData? Material { get; private set; }
    }
}