using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class ExceptionAlertPopup : Popup
{
    public ExceptionAlertPopup(string message = "")
    {
        InitializeComponent();
        exceptionLabel.Text = !string.IsNullOrEmpty(message) ? message : TextResource.ExceptionMessage;
    }
    private void OKButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}