using System.Globalization;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Behaviours;

public class AttachmentFileStatusToSourceConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int  fileStatus = (int)value;
        return HelperMethods.GetAttachmentFileStatusImage(fileStatus);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}