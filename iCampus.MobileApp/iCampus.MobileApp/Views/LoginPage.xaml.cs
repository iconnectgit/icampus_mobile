using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using Microsoft.Maui.Controls;
using Splat;

namespace iCampus.MobileApp.Views;

public partial class LoginPage : ContentPage
{
    
    public LoginPage(IMapper mapper, INativeServices nativeServices)
    {
        InitializeComponent();
        BindingContext = new LoginForm(Navigation, mapper, nativeServices);
        InitializePage();
    }
    private void InitializePage()
    {
        // Set the BindingContext to the ViewModel
        //ViewModel = new LoginForm();
        // ViewModel = Locator.Current.GetService<LoginForm>();
        // BindingContext = ViewModel;

        // Handle the visibility of specific views based on ClientGroupCode
        if (App.ClientGroupCode != null && App.ClientGroupCode.Equals("Beam"))
        {
            OtherAppView.Children.Clear();
        }
        else
        {
            BeamAppView.Children.Clear();
        }

        // Change the status bar color using the native services
        //_nativeServices.ChangeStatusBarColor(254, 254, 254);
    }
}