using iCampus.MobileApp.DependencyService;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Splat;
#if ANDROID
using AndroidNativeServices = CS.MobileApp.DependencyService.AndroidNativeServices;
using AndroidPrintService = CS.MobileApp.DependencyService.AndroidPrintService;
#elif IOS   
using iOSNativeServices = CS.MobileApp.DependencyService.iOSNativeServices;
using iOSPrintService = CS.MobileApp.DependencyService.iOSPrintService;
#endif

namespace CS.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Create the base MauiApp from iCampus.MobileApp
        var mauiApp = iCampus.MobileApp.MauiProgram.CreateMauiApp();

        // Override services specific to CS.MobileApp
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