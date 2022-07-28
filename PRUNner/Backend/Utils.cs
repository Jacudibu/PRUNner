using System;
using System.Text;

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

        public static string GetDurationString(double milliseconds)
        {
            if (milliseconds < 0 || double.IsPositiveInfinity(milliseconds))
            {
                return "∞";
            }
            
            var timespan = TimeSpan.FromMilliseconds(milliseconds);

            var builder = new StringBuilder();
            if (timespan.Days > 0)
            {
                builder.Append(timespan.Days);
                builder.Append("d ");
            }
        
            if (timespan.Hours > 0)
            {
                builder.Append(timespan.Hours);
                builder.Append("h ");
            }
        
            if (timespan.Minutes > 0)
            {
                builder.Append(timespan.Minutes);
                builder.Append('m');
            }

            return builder.ToString();
        }
    }
}