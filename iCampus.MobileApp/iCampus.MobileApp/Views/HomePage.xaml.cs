using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
#if ANDROID
using AndroidX.Core.View;
#endif

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
#if ANDROID
        try
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BackButton Kill Error] {ex}");
        }
#endif

        return true;
    }

    void SwipeGestureListview_SwipeRight(System.Object sender, System.EventArgs e)
    {
        MessagingCenter.Send("", "ListViewRightSwipeNews");
    }
//     protected override void OnAppearing()
//     {
//         base.OnAppearing();
//
// #if ANDROID
//         if ((int)Android.OS.Build.VERSION.SdkInt >= 35) 
//         {
//             var insets = WindowInsetsCompat.ToWindowInsetsCompat(Platform.CurrentActivity?.Window?.DecorView?.RootWindowInsets);
//             if (insets != null)
//             {
//                 var statusBarHeight = 35;  
//                 var bottomPadding = 10;    
//
//                 Padding = new Thickness(0, statusBarHeight, 0, bottomPadding);
//             }
//         }
// #endif
//     }
}