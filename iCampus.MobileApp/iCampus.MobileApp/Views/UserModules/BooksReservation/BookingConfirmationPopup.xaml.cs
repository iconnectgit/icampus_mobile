using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.UserModules.BooksReservation;

public partial class BookingConfirmationPopup : Popup
{
    public BookingConfirmationPopup()
    {
        InitializeComponent();
    }

    private void MenuClosedClick(object? sender, EventArgs eventArgs)
    {
        Close();
    }
}