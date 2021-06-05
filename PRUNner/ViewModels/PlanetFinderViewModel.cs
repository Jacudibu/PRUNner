namespace PRUNner.ViewModels
{
    public class PlanetFinderViewModel : ViewModelBase
    {
        public string Item1 { get; set; } = "";
        public string Item2 { get; set; } = "";
        public string Item3 { get; set; } = "";
        public string Item4 { get; set; } = "";
        
        public bool DisplayFertile { get; set; }
        public bool DisplayRocky { get; set; } = true;
        public bool DisplayGaseous { get; set; }
        public bool DisplayLowGravity { get; set; }
        public bool DisplayLowPressure { get; set; }
        public bool DisplayLowTemperature { get; set; }
        public bool DisplayHighGravity { get; set; }
        public bool DisplayHighPressure { get; set; }
        public bool DisplayHighTemperature { get; set; }
    }
}