using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class ProcessingIndicatorPopup : Popup
{
    public ProcessingIndicatorPopup()
    {
        InitializeComponent();
    }
    public void PopupClosedMethod()
    {
        this.CloseAsync();
    }
}