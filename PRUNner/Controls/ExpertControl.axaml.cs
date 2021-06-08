using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.Controls
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