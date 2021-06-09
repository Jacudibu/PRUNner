using ReactiveUI;

namespace PRUNner.App.Models
{
    public class OptionalPlanetFinderDataObject : ReactiveObject
    {
        public SystemTextBox ExtraSystem { get; } = new();
        
    }
}