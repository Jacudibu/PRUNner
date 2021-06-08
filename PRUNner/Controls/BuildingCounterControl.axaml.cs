using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.Controls
{
    public class BuildingCounterControl : UserControl
    {
        public BuildingCounterControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}