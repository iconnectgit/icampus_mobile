using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.UserModules.BooksReservation;

public partial class TimeSlotListPopup : Popup
{
    public TimeSlotListPopup()
    {
        InitializeComponent();
    }
    private void MenuClosedClick(object? sender, EventArgs eventArgs)
    {
        MessagingCenter.Send<string>("", "TimeSlotPopupClosed");
        Close();
    }

    private void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
    {
        if (e != null)
        {
            var eventArgs = (TappedEventArgs)e;
            string selectedTimeSlot = eventArgs.Parameter.ToString();
            MessagingCenter.Send<string>(selectedTimeSlot, "GetSelectedTimeSlot");
            Close();
        }    
    }
}