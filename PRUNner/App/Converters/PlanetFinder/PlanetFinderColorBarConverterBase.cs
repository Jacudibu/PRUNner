using System;
using Avalonia.Media;

namespace PRUNner.App.Converters.PlanetFinder
{
    public abstract class PlanetFinderColorBarConverterBase
    {
        protected static readonly SolidColorBrush TransparentBrush = new (0);

        public SolidColorBrush GetBrush(double greenFactor)
        {
            var r = Math.Min(255, 2 * 255 * (1 - greenFactor));
            var g = Math.Min(255, 2 * 255 * greenFactor);
                    
            var color = new Color(255, (byte) r, (byte) g, 0);
            var brush = new SolidColorBrush(color);
            return brush;
        }
    }
}