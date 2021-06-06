using PRUNner.ViewModels;
using ReactiveUI;

namespace PRUNner.Models
{
    public class LastPlanetFinderSearch : ReactiveObject
    {
        public string Item1Name { get; private set; } = "";
        public string Item2Name { get; private set; } = "";
        public string Item3Name { get; private set; } = "";
        public string Item4Name { get; private set; } = "";

        public void Update(PlanetFinderViewModel viewModel)
        {
            Item1Name = viewModel.Item1.MaterialName;
            Item2Name = viewModel.Item2.MaterialName;
            Item3Name = viewModel.Item3.MaterialName;
            Item4Name = viewModel.Item4.MaterialName;

            DisplayItem1 = !string.IsNullOrWhiteSpace(Item1Name);
            DisplayItem2 = !string.IsNullOrWhiteSpace(Item2Name);
            DisplayItem3 = !string.IsNullOrWhiteSpace(Item3Name);
            DisplayItem4 = !string.IsNullOrWhiteSpace(Item4Name);
            
            this.RaisePropertyChanged(nameof(Item1Name));
            this.RaisePropertyChanged(nameof(Item2Name));
            this.RaisePropertyChanged(nameof(Item3Name));
            this.RaisePropertyChanged(nameof(Item4Name));
            this.RaisePropertyChanged(nameof(DisplayItem1));
            this.RaisePropertyChanged(nameof(DisplayItem2));
            this.RaisePropertyChanged(nameof(DisplayItem3));
            this.RaisePropertyChanged(nameof(DisplayItem4));
        }

        public bool DisplayItem1 { get; private set;}
        public bool DisplayItem2 { get; private set;}
        public bool DisplayItem3 { get; private set;}
        public bool DisplayItem4 { get; private set;}
    }
}