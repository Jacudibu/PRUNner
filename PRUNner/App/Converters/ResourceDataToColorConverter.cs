using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using PRUNner.Backend.Data.Components;

namespace PRUNner.App.Converters
{
    public class ResourceDataToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ResourceData resourceData)
            {
                var factor = resourceData.Factor * 1.75; // increases how quickly the color shifts from red to yellow / green
                
                var r = Math.Min(255, 2 * 255 * (1 - factor));
                var g = Math.Min(255, 2 * 255 * factor);
                    
                var color = new Color(255, (byte) r, (byte) g, 0);
                var brush = new SolidColorBrush(color);
                return brush;
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}