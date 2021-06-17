using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Controls
{
    public class PriceOverridesControl : UserControl
    {
        public PriceOverridesControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}