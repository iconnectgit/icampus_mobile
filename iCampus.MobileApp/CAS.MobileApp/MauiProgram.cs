using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using iCampus.MobileApp.DependencyService;
using Splat;
#if ANDROID
using AndroidNativeServices = CAS.MobileApp.DependencyService.AndroidNativeServices;
using AndroidPrintService = CAS.MobileApp.DependencyService.AndroidPrintService;
#elif IOS   
using iOSNativeServices = CAS.MobileApp.DependencyService.iOSNativeServices;
using iOSPrintService = CAS.MobileApp.DependencyService.iOSPrintService;
#endif

namespace CAS.MobileApp;
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