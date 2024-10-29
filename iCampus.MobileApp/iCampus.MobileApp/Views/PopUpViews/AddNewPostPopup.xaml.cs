using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class AddNewPostPopup : Popup
{
    public AddNewPostPopup()
    {
        InitializeComponent();
    }
    private void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        this.Close();
    }
}