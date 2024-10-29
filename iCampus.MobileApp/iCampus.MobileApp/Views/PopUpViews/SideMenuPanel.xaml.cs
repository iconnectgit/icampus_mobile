using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class SideMenuPanel : Popup
{
    public SideMenuPanel()
    {
        InitializeComponent();
    }
    

    async void SwipeGestureListview_SwipeLeft(System.Object sender, System.EventArgs e)
    {
        Close(); // Close the popup directly
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

    private void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        Close();
    }
}
