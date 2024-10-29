using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.PopUpViews;

public partial class StudentListPopUp : Popup
{
    public StudentListPopUp()
    {
        InitializeComponent();
    }

    private void MenuClosedClick(object? sender, TappedEventArgs e)
    {
        this.Close();
    }
}