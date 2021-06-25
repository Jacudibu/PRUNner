using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace PRUNner.App.Converters.PlanetFinder
{
    public class FertilityConverterHelper
    {
        public static double GetFertilityFactor(double fertility)
        {
            if (fertility < 0)
            {
                return (fertility + 1) / 4;
            }

            return fertility + 0.25;
        }
    }
    
    public class FertilityToColorConverter : PlanetFinderColorBarConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double fertility)
            {
                if (fertility <= -1)
                {
                    return TransparentBrush;
                }
                
                return GetBrush(FertilityConverterHelper.GetFertilityFactor(fertility));
            }
            
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Unable to Parse."), BindingErrorType.DataValidationError);
        }
    }
}