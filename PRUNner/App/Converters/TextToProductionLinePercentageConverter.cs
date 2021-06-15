using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace PRUNner.App.Converters
{
    public class TextToProductionLinePercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value.ToString();
            if (result == null)
            {
                return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string stringValue)
            {
                return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
            }
            
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return 0;
            }

            stringValue = stringValue.Trim().Replace(',', '.');
            
            if (double.TryParse(stringValue, out var result))
            {
                return result;
            }

            var split = stringValue.Split('/');
            if (split.Length == 2 && split[0].Length > 0)
            {
                if (split[1].Length > 0)
                {
                    return double.Parse(split[0]) / double.Parse(split[1]) * 100;
                }

                return double.Parse(split[0]);
            }

            split = stringValue.Split('*');
            if (split.Length == 2 && split[0].Length > 0)
            {
                if (split[1].Length > 0)
                {
                    return double.Parse(split[0]) * double.Parse(split[1]) * 100;
                }

                return double.Parse(split[0]);
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}