using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using iCampus.MobileApp.DependencyService;
using Splat;
#if ANDROID
using AndroidNativeServices = Madar.MobileApp.DependencyService.AndroidNativeServices;
using AndroidPrintService = Madar.MobileApp.DependencyService.AndroidPrintService;
#elif IOS   
using iOSNativeServices = Madar.MobileApp.DependencyService.iOSNativeServices;
using iOSPrintService = Madar.MobileApp.DependencyService.iOSPrintService;
#endif

namespace Madar.MobileApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Create the base MauiApp from iCampus.MobileApp
        var mauiApp = iCampus.MobileApp.MauiProgram.CreateMauiApp();

        // Override services specific to CAS.MobileApp
#if ANDROID
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new AndroidNativeServices());
        Locator.CurrentMutable.RegisterConstant<IPrintService>(new AndroidPrintService());
#elif IOS       
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new iOSNativeServices());
        Locator.CurrentMutable.RegisterConstant<IPrintService>(new iOSPrintService());
#endif
        return mauiApp;
    }
}