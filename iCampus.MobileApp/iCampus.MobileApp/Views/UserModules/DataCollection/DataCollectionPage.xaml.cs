using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.DataCollection;

public partial class DataCollectionPage : ContentPage
{
    public DataCollectionPage()
    {
        InitializeComponent();
    }
    protected override bool OnBackButtonPressed()
    {
        if (App.IsMandateDataCollection || App.IsDataCollectionOn)
            return true;
        else
        {
            var currentViewModel = BindingContext as ViewModelBase;

            if (currentViewModel != null)
            {
                currentViewModel.HandleMenuSelectionOnBack();
            }
            return true;
        }
    }
}