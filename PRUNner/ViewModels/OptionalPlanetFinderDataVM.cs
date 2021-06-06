using Avalonia.Data;
using PRUNner.Backend.Data;
using ReactiveUI;

namespace PRUNner.ViewModels
{
    public class OptionalPlanetFinderDataVM : ViewModelBase
    {
        public OptionalPlanetDistance ExtraSystem { get; } = new();


        public class OptionalPlanetDistance : ViewModelBase
        {
            private string _systemName = "";

            public string SystemName
            {
                get => _systemName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        this.RaiseAndSetIfChanged(ref _systemName, "");
                        return;
                    }
                    
                    System = SystemData.Get(value);
                    if (System == null)
                    {
                        throw new DataValidationException("System does not exist");
                    }
                        
                    this.RaiseAndSetIfChanged(ref _systemName, value);
                }
            }

            public SystemData? System { get; private set; }
        }
    }
}