using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class LevelNoToIntendConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Thickness leftMargin = 0;
        if (value != null)
        {
            switch (value.ToString())
            {
                case "1":
                    leftMargin = new Thickness(0,0,0,0);
                    break;
                case "2":
                    leftMargin = new Thickness(20, 0, 0, 0);
                    break;
                case "3":
                    leftMargin = new Thickness(40, 0, 0, 0);
                    break;
            }
        }
        return leftMargin;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 0;
    }
}