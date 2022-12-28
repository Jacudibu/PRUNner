using System;

namespace PRUNner.Backend.Utility
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
            if (milliseconds < 0 || double.IsPositiveInfinity(milliseconds) || double.IsNaN(milliseconds))
            {
                return "∞";
            }
            
            var timespan = TimeSpan.FromMilliseconds(milliseconds);

            var builder = ObjectPools.StringBuilderPool.Get();
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

            var result = builder.ToString();
            ObjectPools.StringBuilderPool.Return(builder);
            return result;
        }
    }
}