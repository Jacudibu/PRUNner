using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Controls
{
    public class ShoppingCartControl : UserControl
    {
        public ShoppingCartControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}