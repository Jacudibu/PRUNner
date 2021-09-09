using System;
using NLog;
using Octokit;

namespace PRUNner.Backend
{
    public static class UpdateChecker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static void CheckForUpdates(Version? currentVersion)
        {
            Logger.Info("Checking for Updates...");
            var client = new GitHubClient(new ProductHeaderValue("PRUNner"));
            Release latest;
            try
            {
                var response = client.Repository.Release.GetAll("jacudibu", "prunner").Result;
                latest = response[0];
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error whilst checking for updates: " + e);
                return;
            }
            
            if (latest.TagName == null)
            {
                Logger.Error("Error whilst checking for updates: Version String was null.");
                return;
            }

            var versionString = latest.TagName.Trim('v');
            if (!Version.TryParse(versionString, out var availableVersion))
            {
                Logger.Error("Error whilst checking for updates: Unable to parse git version string.");
                return;
            }
            
            if (currentVersion == null)
            {
                Logger.Error("Error whilst checking for updates: PRUNner version was null..");
                return;
            }

            if (currentVersion < availableVersion)
            {
                Logger.Info("A new PRUNner version is available: " + availableVersion.ToString(3));
            }
            else
            {
                Logger.Info("PRUNner is up to date!");
            }
        }
    }
}