using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace PRUNner.App.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}