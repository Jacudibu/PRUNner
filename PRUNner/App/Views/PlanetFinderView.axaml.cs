using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Views
{
    public class PlanetFinderView : UserControl
    {
        public PlanetFinderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}