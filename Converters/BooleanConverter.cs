using System.Globalization;

namespace TrainiumNeon.Converters
{
    public class BooleanConverter : IValueConverter
    {
        // Convierte un valor booleano a su opuesto
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }

        // Convierte de vuelta el valor booleano a su opuesto
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }
    }
}
