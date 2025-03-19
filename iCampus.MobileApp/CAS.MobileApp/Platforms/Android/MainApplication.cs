using System;
using Android.App;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Hosting;

namespace CAS.MobileApp;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
    }
    
    protected override MauiApp CreateMauiApp()
    {
        iCampus.MobileApp.App.ClientCode = "CAS";
        // Remove Entry control underline
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
        {
            h.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
        });

        return MauiProgram.CreateMauiApp();
    }
}