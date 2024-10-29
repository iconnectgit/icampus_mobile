using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class LevelNoToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string colorString = string.Empty;
        if (value != null)
        {
            switch (value.ToString())
            {
                case "1":
                    colorString = "#006699";
                    break;
                case "2":
                    colorString = "#993366";
                    break;
                case "3":
                    colorString = "#006600";
                    break;
            }
        }
        return colorString;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}