using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.Views
{
    public class BasePlannerView : UserControl
    {
        public BasePlannerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}