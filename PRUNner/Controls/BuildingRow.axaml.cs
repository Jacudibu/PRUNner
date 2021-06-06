using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.Controls
{
    public class BuildingRow : UserControl
    {
        public BuildingRow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}