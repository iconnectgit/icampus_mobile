using iCampus.MobileApp.DependencyService;
using Microsoft.Extensions.Logging;
using Splat;
#if ANDROID
using AndroidNativeServices = Gipa.MobileApp.DependencyService.AndroidNativeServices;
using AndroidPrintService = Gipa.MobileApp.DependencyService.AndroidPrintService;
#elif IOS   
using iOSNativeServices = Gipa.MobileApp.DependencyService.iOSNativeServices;
using iOSPrintService = Gipa.MobileApp.DependencyService.iOSPrintService;
#endif
namespace Gipa.MobileApp;

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