using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.Communication;

namespace iCampus.MobileApp.Views.UserModules.Communication;

public partial class CommunicationPage : ContentPage
{
    private IMapper _mapper;
    public CommunicationPage()
    {
        InitializeComponent();
        //_mapper = mapper;
        //BindingContext = new CommunicationForm(_mapper);
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