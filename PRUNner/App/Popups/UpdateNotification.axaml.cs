using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PRUNner.App.Popups
{
    public class UpdateNotification : Window
    {
        public UpdateNotification()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}