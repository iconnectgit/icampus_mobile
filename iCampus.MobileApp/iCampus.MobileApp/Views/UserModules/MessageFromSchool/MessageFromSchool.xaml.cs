using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.MessageFromSchool;

public partial class MessageFromSchool : ContentPage
{
    public MessageFromSchool()
    {
        InitializeComponent();
        if (AppSettings.Current.IsAlertsFromPushNotifications)
        {
                
            //beamMessageFromSchoolPage.SelectNext();
            AppSettings.Current.IsAlertsFromPushNotifications = false;
        }
    }
    protected override bool OnBackButtonPressed()
    {
        var currentViewModel = BindingContext as ViewModelBase;

        if (currentViewModel != null)
        {
            currentViewModel.HandleMenuSelectionOnBack();
        }
        return true;
    }
}