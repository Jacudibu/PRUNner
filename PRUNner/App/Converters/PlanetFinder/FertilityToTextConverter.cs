using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace PRUNner.App.Converters.PlanetFinder
{
    public class FertilityToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double fertility)
            {
                if (fertility <= -1)
                {
                    return "–";
                }

                return fertility.ToString("P0");
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}