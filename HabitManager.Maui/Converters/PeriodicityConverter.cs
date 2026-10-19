using HabitManager.Maui.Helpers;
using System.Globalization;

namespace HabitManager.Maui.Converters
{
    public class PeriodicityConverter : IValueConverter  // saģenerēja MI
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<DayOfWeek> enums)
            {
                return enums.ToNormalString();
            }
            return "No schedule";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
