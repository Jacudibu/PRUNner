using Avalonia.Markup.Xaml;

namespace PRUNner.App.Popups
{
    public class BuildingConfigurationPopup : PopupWindowBase
    {
        public BuildingConfigurationPopup()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}