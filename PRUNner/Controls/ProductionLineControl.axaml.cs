using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.Controls
{
    public class ProductionLineControl : UserControl
    {
        public ProductionLineControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}