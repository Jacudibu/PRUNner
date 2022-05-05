using Octokit;

namespace PRUNner.Backend
{
    public class UpdateData
    {
        public readonly bool UpdateAvailable;
        public readonly Release? LatestRelease;

        public static readonly UpdateData NoUpdate = new();

        private UpdateData()
        {
            UpdateAvailable = false;
        }

        public UpdateData(Release latestRelease)
        {
            UpdateAvailable = true;
            LatestRelease = latestRelease;
        }
    }
}