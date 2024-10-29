using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms.PopupForms;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class AttachmentListPopup : Popup
{
    public AttachmentListPopup()
    {
        InitializeComponent();
    }
    
    private void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        Close();
    }
}