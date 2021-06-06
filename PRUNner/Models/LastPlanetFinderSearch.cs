using PRUNner.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Models
{
    public class LastPlanetFinderSearch : ReactiveObject
    {
        [Reactive] public string Item1Name { get; private set; } = "";
        [Reactive] public string Item2Name { get; private set; } = "";
        [Reactive] public string Item3Name { get; private set; } = "";
        [Reactive] public string Item4Name { get; private set; } = "";

        [Reactive] public bool DisplayItem1 { get; private set;}
        [Reactive] public bool DisplayItem2 { get; private set;}
        [Reactive] public bool DisplayItem3 { get; private set;}
        [Reactive] public bool DisplayItem4 { get; private set;}
        
        public void Update(PlanetFinderViewModel viewModel)
        {
            Item1Name = viewModel.Item1.Material?.Ticker ?? "";
            Item2Name = viewModel.Item2.Material?.Ticker ?? "";
            Item3Name = viewModel.Item3.Material?.Ticker ?? "";
            Item4Name = viewModel.Item4.Material?.Ticker ?? "";

            DisplayItem1 = !string.IsNullOrWhiteSpace(Item1Name);
            DisplayItem2 = !string.IsNullOrWhiteSpace(Item2Name);
            DisplayItem3 = !string.IsNullOrWhiteSpace(Item3Name);
            DisplayItem4 = !string.IsNullOrWhiteSpace(Item4Name);
        }
    }
}