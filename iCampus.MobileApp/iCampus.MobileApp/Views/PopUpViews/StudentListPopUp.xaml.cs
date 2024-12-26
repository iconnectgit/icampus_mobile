using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms;
using Mopups.Services;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class StudentListPopUp
{
    public StudentListPopUp()
    {
        InitializeComponent();
    }

    private async void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        if (MopupService.Instance.PopupStack.Any())
        {
            await MopupService.Instance.PopAsync();
        }
    }
}