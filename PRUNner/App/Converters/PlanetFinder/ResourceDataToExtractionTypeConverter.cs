using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using PRUNner.Backend;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.App.Converters.PlanetFinder
{
    public class ResourceDataToExtractionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ResourceData resourceData)
            {
                return resourceData.ResourceType switch
                {
                    ResourceType.Gaseous => Names.Buildings.COL,
                    ResourceType.Liquid => Names.Buildings.RIG,
                    ResourceType.Mineral => Names.Buildings.EXT,
                    _ => new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError)
                };
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}