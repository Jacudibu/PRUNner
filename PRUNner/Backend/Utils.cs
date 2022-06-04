namespace PRUNner.Backend
{
    public static class Utils
    {
        public static void OpenWebsite(string uri)
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = uri
            };
            System.Diagnostics.Process.Start(psi);
        }
    }
}