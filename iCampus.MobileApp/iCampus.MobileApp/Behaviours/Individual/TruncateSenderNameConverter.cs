using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class TruncateSenderNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (Application.Current.MainPage.Width <= System.Convert.ToInt32(TextResource.DeviceMaxPortraitSize))
        {
            return value.ToString().ToCharArray().Count() > System.Convert.ToInt32(TextResource.SenderValueMaxLength) ? String.Concat(value.ToString().Substring(0, System.Convert.ToInt32(TextResource.SenderValueMaxLength)), "...") : value.ToString();
        }
        else
            return value.ToString();
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}