using System;
using System.ComponentModel;
using Avalonia.Controls;

namespace PRUNner.App.Popups
{
    public class PopupWindowBase : Window
    {
        private static bool _mainWindowStillActive = true;

        protected PopupWindowBase()
        {
            ShowInTaskbar = false;
            SystemDecorations = SystemDecorations.None;

            Deactivated += OnDeactivated;
            Closing += OnClosing;

            App.MainWindow.Closed += OnMainWindowClosed;
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            if (_mainWindowStillActive)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void OnDeactivated(object? sender, EventArgs e)
        {
            Hide();
        }

        public void ShowAndFocus(object dataContext)
        {
            DataContext = dataContext;
            Show();
            Focus();
            Activate();
        }

        private void OnMainWindowClosed(object? sender, EventArgs e)
        {
            _mainWindowStillActive = false;
            Close();
        }
    }
}