using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Mopups.Services;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class OnlinePaymentTermsandConditionsPopup
{
    public OnlinePaymentTermsandConditionsPopup()
    {
        InitializeComponent();
    }
    private async void MenuClosedClick(object? sender, EventArgs eventArgs)
    {
        if (MopupService.Instance.PopupStack.Any())
        {
            await MopupService.Instance.PopAsync();
        }
    }
}