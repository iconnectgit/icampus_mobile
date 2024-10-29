using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class AgendaCancellationTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string cancellationText = string.Empty;
        if (value != null)
        {
            // if (value is BindableAgendaView agenda)
            // {
            //     if (agenda.DeletedBy != AppSettings.Current.UserRefId)
            //         cancellationText = string.Format(TextResource.AgendaCancellationText, agenda.DeletedByTeacherName, agenda.DeletedDate?.ToString(TextResource.DateFormatKey3));
            //     else
            //         cancellationText = string.Format(TextResource.AgendaCancellationText, "You", agenda.DeletedDate?.ToString(TextResource.DateFormatKey3));
            // }
            // else if (value is BindableAgendaWeeklyGroupView weeklyGroup)
            // {
            //     if (weeklyGroup.DeletedBy != AppSettings.Current.UserRefId)
            //         cancellationText = string.Format(TextResource.AgendaCancellationText, weeklyGroup.DeletedByTeacherName, weeklyGroup.DeletedDate?.ToString(TextResource.DateFormatKey3));
            //     else
            //         cancellationText = string.Format(TextResource.AgendaCancellationText, "You", weeklyGroup.DeletedDate?.ToString(TextResource.DateFormatKey3));
            // }
            return cancellationText;
        }
        else
            return cancellationText;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}