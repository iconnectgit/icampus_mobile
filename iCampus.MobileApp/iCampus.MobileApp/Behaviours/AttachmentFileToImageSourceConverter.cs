using System.Globalization;
using iCampus.Common.Enums;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Behaviours;

public class AttachmentFileToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var fileName = System.Convert.ToString(value);
        if (!string.IsNullOrEmpty(fileName))
        {
            string extension = Path.GetExtension(value.ToString()).Replace(".", "");
            var fileType = extension.ToEnum<FileTypes>();
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