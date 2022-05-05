using Avalonia.Markup.Xaml;

namespace PRUNner.App.Popups
{
    public class UpdateNotificationWindow : PopupWindowBase
    {
        public UpdateNotificationWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}