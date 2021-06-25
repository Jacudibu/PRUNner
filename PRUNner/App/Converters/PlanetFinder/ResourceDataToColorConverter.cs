using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using PRUNner.Backend.Data.Components;

namespace PRUNner.App.Converters.PlanetFinder
{
    public class ResourceDataToColorConverter : PlanetFinderColorBarConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ResourceData resourceData)
            {
                var factor = resourceData.Factor * 1.75; // increases how quickly the color shifts from red to yellow / green
                return GetBrush(factor);
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}