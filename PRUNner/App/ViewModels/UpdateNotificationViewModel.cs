using Octokit;
using PRUNner.App.Popups;
using PRUNner.Backend;
using ReactiveUI;

namespace PRUNner.App.ViewModels
{
    public class UpdateNotificationViewModel : ReactiveObject
    {
        public Release Release { get; private set; }
        private UpdateNotification? _updatePopup;

        public UpdateNotificationViewModel()
        {
            Release = null!;
        }

        public void Open()
        {
            _updatePopup = new UpdateNotification();
            _updatePopup.DataContext = this;
            _updatePopup.ShowDialog(App.MainWindow);
        }

        public static void CheckForUpdate()
        {
            var updateData = UpdateChecker.CheckForUpdates(MainWindowViewModel.Instance.GetType().Assembly.GetName().Version);
            if (updateData.UpdateAvailable)
            {
                var viewModel = new UpdateNotificationViewModel();
                viewModel.Release = updateData.LatestRelease!;
                viewModel.Open();
            }
        }
        
        public void OpenReleasePage()
        {
            var uri = Release.HtmlUrl;
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = uri
            };
            System.Diagnostics.Process.Start(psi);
        }

        public void IgnoreAndClose()
        {
            CloseWindow();
        }
        
        public void CloseWindow()
        {
            _updatePopup?.Close();
        }
    }
}