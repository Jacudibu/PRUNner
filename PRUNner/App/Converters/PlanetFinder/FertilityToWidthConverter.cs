using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace PRUNner.App.Converters.PlanetFinder
{
    public class FertilityToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double fertility)
            {
                return 50 * FertilityConverterHelper.GetFertilityFactor(fertility);
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}