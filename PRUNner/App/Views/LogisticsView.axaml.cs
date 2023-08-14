using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Views
{
    public class LogisticsView : UserControl
    {
        public LogisticsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
