using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PRUNner.App.ViewModels;

namespace PRUNner.App.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "PRUNner v" + GetType().Assembly.GetName().Version?.ToString(3) ?? "?";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyModifiers == KeyModifiers.Control)
            {
                switch (e.Key)
                {
                    case Key.S:
                    {
                        MainWindowViewModel.Instance.SaveToDisk();
                        break;   
                    }
                    case Key.OemPlus:
                    {
                        var res = Avalonia.Application.Current.Resources["ControlContentThemeFontSize"] as double?;
                        Avalonia.Application.Current.Resources["ControlContentThemeFontSize"] = res + 1;
                        break;
                    }
                    case Key.OemMinus:
                    {
                        var res = Avalonia.Application.Current.Resources["ControlContentThemeFontSize"] as double?;
                        Avalonia.Application.Current.Resources["ControlContentThemeFontSize"] = res - 1;
                        break;
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            UpdateNotificationViewModel.CheckForUpdate();
        }
    }
}