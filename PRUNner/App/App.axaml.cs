using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PRUNner.App.ViewModels;
using PRUNner.App.Views;

namespace PRUNner.App
{
    public class App : Application
    {
        internal static Window MainWindow = null!;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }
            else
            {
                throw new Exception("Unsupported Environment!");
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}