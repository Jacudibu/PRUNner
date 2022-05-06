using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Controls
{
    public class PriceDataPreferenceControl : UserControl
    {
        public PriceDataPreferenceControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}