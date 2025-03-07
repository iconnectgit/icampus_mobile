using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.Calendar;

public partial class AddNewPostPage : ContentPage
{
    public AddNewPostPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "UpdateAttachmentListView", (arg) =>
        {
            //ForceNativeTableUpdate(attachmentlistview);
        });
    }
    public void ForceNativeTableUpdate(ListView listView)
    {
        if (listView.Handler != null)
        {
#if ANDROID
            var nativeListView = listView.Handler.PlatformView as AndroidX.RecyclerView.Widget.RecyclerView;
            nativeListView?.GetAdapter()?.NotifyDataSetChanged();
#elif IOS || MACCATALYST
            var nativeListView = listView.Handler.PlatformView as UIKit.UITableView;
            nativeListView?.ReloadData();
#endif
        }
    }
    
    private async void CustomDatePicker_DateSelected(object? sender, DateChangedEventArgs e)
    {
        try
        {

            DateTime dateTime;
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = e.NewDate.Date.Date.AddDays(-1 * (int)cal.GetDayOfWeek(e.NewDate.Date));
            var d2 = DateTime.Now.Date.AddDays(-1 * (int)cal.GetDayOfWeek(DateTime.Now.Date));


            if (AppSettings.Current.IsWeekendsDisabled && AppSettings.Current.WeekendDays.Contains(((int)e.NewDate.DayOfWeek).ToString()))
            {
                dateTime = e.OldDate;
                dueDatePicker.Date = dateTime;
                await App.Current.MainPage.DisplayAlert("", TextResource.WeekendDateError, TextResource.OkText);
            }
            else if (AppSettings.Current.IsCurrentWeekDisabled && d1 == d2)
            {
                dateTime = GetNextWeekday(e.NewDate.Date, DayOfWeek.Monday); 
                dueDatePicker.Date = dateTime;
                dueDatePicker.MinimumDate = dateTime;
                await App.Current.MainPage.DisplayAlert("", TextResource.SameWeekError, TextResource.OkText);
            }
            else
            {
                dateTime = e.NewDate;
                dueDatePicker.Date = dateTime;
            }
            MessagingCenter.Send<string>(e.NewDate.ToString(), "SetReminderMaxDate");
        }
        catch (Exception ex)
        {
                
        }
    }

    void Reminder_DateSelected(object? sender, DateChangedEventArgs dateChangedEventArgs)
    {
        MessagingCenter.Send<string>(dateChangedEventArgs.NewDate.ToString(), "ReminderDateSelected");
    }
    public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
    {
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }
}