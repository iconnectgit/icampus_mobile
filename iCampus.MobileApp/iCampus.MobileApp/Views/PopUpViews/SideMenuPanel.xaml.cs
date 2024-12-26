using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms;
using Mopups.Services;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class SideMenuPanel
{
    public SideMenuPanel()
    {
        InitializeComponent();
    }
    

    async void SwipeGestureListview_SwipeLeft(System.Object sender, System.EventArgs e)
    {
        if (MopupService.Instance.PopupStack.Any())
        {
            await MopupService.Instance.PopAsync();
        }
    }

    protected void OnOpened()
    {
        // Custom logic when the popup is opened
        MessagingCenter.Send<string>("", "SideMenuPanelLeftSwipeSubscribe");
    }

    protected void OnClosed()
    {
        // Custom logic when the popup is closed
        MessagingCenter.Unsubscribe<string>("", "SideMenuPanelLeftSwipe");
        MessagingCenter.Unsubscribe<string>("", "SideMenuPanelLeftSwipeSubscribe");
    }

    private async void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        if (MopupService.Instance.PopupStack.Any())
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
