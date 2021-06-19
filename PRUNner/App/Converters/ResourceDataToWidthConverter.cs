using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using PRUNner.Backend.Data.Components;

namespace PRUNner.App.Converters
{
    public class ResourceDataToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ResourceData resourceData)
            {
                return 50 * resourceData.Factor;
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}