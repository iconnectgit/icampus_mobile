using Microsoft.Maui.Handlers;

namespace iCampus.MobileApp.Controls;

public class AndroidWebViewHandler : WebViewHandler
{
    protected override void ConnectHandler(global::Android.Webkit.WebView platformView)
    {
        // Enable JavaScript and disable multiple windows
        platformView.Settings.SetSupportMultipleWindows(false);
        platformView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
        platformView.Settings.JavaScriptEnabled = true;

        base.ConnectHandler(platformView);
    }
}