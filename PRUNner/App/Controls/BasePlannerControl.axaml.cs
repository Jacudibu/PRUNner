using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Controls
{
    public class BasePlannerControl : UserControl
    {
        public BasePlannerControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}