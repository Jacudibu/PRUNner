using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace PRUNner.App.Converters
{
    public class MonetaryValueToColor : IValueConverter
    {
        private static readonly SolidColorBrush Red = new(new Color(255, 200, 0, 0)); 
        private static readonly SolidColorBrush Green = new(new Color(255, 0, 200, 0)); 
        private static readonly SolidColorBrush Black = new(new Color(255, 0, 0, 0)); 
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double monetaryValue)
            {
                if (monetaryValue < 0)
                {
                    return Red;
                }

                if (monetaryValue > 0)
                {
                    return Green;
                }
                
                return Black;
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}