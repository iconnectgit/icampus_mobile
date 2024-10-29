using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class AttendanceTypeToColorConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value.ToString().ToLower())
        {
            case "p":
                return "#22B14C";
            case "a":
                return "#ED1C24";
            case "e":
                return "#99D9EA";
            case "l":
                return "#FFC90E";
            default:
                return "#22B14C";
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}