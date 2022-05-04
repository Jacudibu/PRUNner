using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NLog;
using PRUNner.App.Popups;

namespace PRUNner.App.Controls
{
    public class ProductionLineControl : UserControl
    {
        private static BuildingConfigurationPopup? _configurationPopup;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProductionLineControl()
        {
            InitializeComponent();
            
            var openConfigurationButton = this.FindControl<Button>("OpenConfigurationPopupButton");
            openConfigurationButton.Click += OpenConfigurationPopup;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void OpenConfigurationPopup(object? sender, RoutedEventArgs routedEventArgs)
        {
            if (DataContext == null)
            {
                Logger.Error("DataContext was null when opening building configuration button was pressed!");
                return;
            }
            
            _configurationPopup ??= new BuildingConfigurationPopup();
            _configurationPopup.ShowAndFocus(DataContext);
        }
    }
}