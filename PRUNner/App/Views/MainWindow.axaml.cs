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
            if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
            {
                MainWindowViewModel.Instance.SaveToDisk();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            MainWindowViewModel.Instance.CheckForUpdates();
        }
    }
}