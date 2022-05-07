using System;
using NLog;
using Octokit;

namespace PRUNner.Backend
{
    public static class UpdateChecker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static UpdateData CheckForUpdates(Version? currentVersion)
        {
            Logger.Info("Checking for Updates...");
            var client = new GitHubClient(new ProductHeaderValue("PRUNner"));
            Release latestRelease;
            try
            {
                var response = client.Repository.Release.GetAll("jacudibu", "prunner").Result;
                latestRelease = response[0];
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error whilst checking for updates: " + e);
                return UpdateData.NoUpdate;
            }
            
            if (latestRelease.TagName == null)
            {
                Logger.Error("Error whilst checking for updates: Version String was null.");
                return UpdateData.NoUpdate;
            }

            var versionString = latestRelease.TagName.Trim('v');
            if (!Version.TryParse(versionString, out var availableVersion))
            {
                Logger.Error("Error whilst checking for updates: Unable to parse git version string.");
                return UpdateData.NoUpdate;
            }
            
            if (currentVersion == null)
            {
                Logger.Error("Error whilst checking for updates: PRUNner version was null..");
                return UpdateData.NoUpdate;
            }

            if (currentVersion < availableVersion)
            {
                Logger.Info("A new PRUNner version is available: " + versionString);
                return new UpdateData(latestRelease);
            }
            
            Logger.Info("PRUNner is up to date!");
            return UpdateData.NoUpdate;
        }
    }
}