using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views;

public partial class HomePage : ContentPage
{
    private readonly IMapper _mapper;
    private readonly INativeServices _nativeServices;
    public HomePage()
    {
        InitializeComponent();
        //BindingContext = new HomeForm(_mapper, Navigation, _nativeServices);
        Disappearing += HomePage_Disappearing;
    }
    private void HomePage_Disappearing(object sender, EventArgs e)
    {
        if (BindingContext is HomeForm homeForm)
        {
            homeForm.OnDisappearing();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        _nativeServices.KillProcess();
        return true;
    }

    void SwipeGestureListview_SwipeRight(System.Object sender, System.EventArgs e)
    {
        MessagingCenter.Send("", "ListViewRightSwipeNews");
    }
}