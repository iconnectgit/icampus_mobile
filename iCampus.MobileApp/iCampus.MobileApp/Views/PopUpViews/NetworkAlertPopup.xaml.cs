using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class NetworkAlertPopup : Popup
{
    public NetworkAlertPopup()
    {
        InitializeComponent();
    }
    private void OKButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}