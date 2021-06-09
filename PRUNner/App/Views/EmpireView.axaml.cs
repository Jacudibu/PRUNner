using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Views
{
    public class EmpireView : UserControl
    {
        public EmpireView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}