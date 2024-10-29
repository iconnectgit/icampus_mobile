using System.Globalization;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Behaviours;

public class ModuleNameToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var iconSource = AppSettings.Current.MenuStructureList.ToList().Where(i => i.ModuleCode.ToLower().Equals(value.ToString().ToLower()));
        if (iconSource != null)
        {
            return iconSource.FirstOrDefault().ModuleImageUrl;
        }
        else
            return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}