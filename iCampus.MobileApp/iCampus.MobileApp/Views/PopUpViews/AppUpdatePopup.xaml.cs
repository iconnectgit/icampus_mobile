using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class AppUpdatePopup : Popup
{
    public AppUpdatePopup()
    {
        InitializeComponent();
    }
    private void LaterButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}