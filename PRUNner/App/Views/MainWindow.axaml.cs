using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "PRUNner v" + GetType().Assembly.GetName().Version?.ToString(3) ?? "?";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}