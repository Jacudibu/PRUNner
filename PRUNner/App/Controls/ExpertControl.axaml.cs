using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Controls
{
    public class ExpertControl : UserControl
    {
        public ExpertControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}