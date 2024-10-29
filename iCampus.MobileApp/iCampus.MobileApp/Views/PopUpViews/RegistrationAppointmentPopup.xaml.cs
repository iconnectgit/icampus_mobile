using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class RegistrationAppointmentPopup : Popup
{
    public RegistrationAppointmentPopup()
    {
        InitializeComponent();
    }
    private void MenuClosedClick(object? sender, EventArgs eventArgs)
    {
        Close();
    }
}