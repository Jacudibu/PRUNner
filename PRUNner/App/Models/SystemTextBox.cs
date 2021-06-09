using Avalonia.Data;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.App.Models
{
    public class SystemTextBox : ReactiveObject
    {
        private string _systemName = "";

        public string SystemName
        {
            get => _systemName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    System = null;
                    this.RaiseAndSetIfChanged(ref _systemName, "");
                    return;
                }
                    
                System = SystemData.Get(value);
                if (System == null)
                {
                    throw new DataValidationException("System " + value + " does not exist. :(");
                }
                        
                this.RaiseAndSetIfChanged(ref _systemName, value);
            }
        }

        public SystemData? System { get; private set; }
    }
}