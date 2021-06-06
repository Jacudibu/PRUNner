using Avalonia.Data;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.Models
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
                    
                value = value.ToUpper();
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