using Android.Runtime;
using Foundation;
using UIKit;

namespace iCampus.MobileApp;

[Foundation.Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // Add compatibility flags here if necessary
        return base.FinishedLaunching(app, options);
    }
}