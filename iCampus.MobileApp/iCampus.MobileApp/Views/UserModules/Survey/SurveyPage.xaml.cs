using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.Survey;

public partial class SurveyPage : ContentPage
{
    public SurveyPage()
    {
        InitializeComponent();
    }
    protected override bool OnBackButtonPressed()
    {
        if (App.IsMandateSurvey || App.IsSurveyFromLogin)
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