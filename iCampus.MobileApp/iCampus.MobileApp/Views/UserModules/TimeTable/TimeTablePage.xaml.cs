using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.TimeTable;

public partial class TimeTablePage : ContentPage
{
    public TimeTablePage()
    {
        InitializeComponent();
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