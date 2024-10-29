using System.Globalization;
using iCampus.Common.Enums;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Behaviours;

public class AttachmentTypeToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            var fileType = (FileTypes)value;
            return HelperMethods.GetAttachmentImageSourceFromType(fileType);
        }
        else
            return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}