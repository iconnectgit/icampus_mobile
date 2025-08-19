using Android.Content;
using Android.Views;
using Android.Webkit;
using iCampus.MobileApp.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Handlers;

namespace iCampus.MobileApp.Handlers;

public class CustomWebViewHandler : ViewHandler<CustomWebView, Android.Webkit.WebView>
{
    public static IPropertyMapper<CustomWebView, CustomWebViewHandler> Mapper = new PropertyMapper<CustomWebView, CustomWebViewHandler>(ViewMapper)
    {
        [nameof(CustomWebView.HtmlContent)] = MapHtmlContent
    };

    public CustomWebViewHandler() : base(Mapper)
    {
    }

    protected override Android.Webkit.WebView CreatePlatformView()
    {
        var webView = new Android.Webkit.WebView(Context)
        {
            LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent) // Ensures WebView wraps content height
        };

        webView.Settings.JavaScriptEnabled = true;
        webView.SetWebViewClient(new CustomWebViewClient());
    
        return webView;
    }


    protected override void ConnectHandler(Android.Webkit.WebView platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView != null)
            platformView.LoadDataWithBaseURL(null, VirtualView.HtmlContent, "text/html", "utf-8", null);
    }

    private static void MapHtmlContent(CustomWebViewHandler handler, CustomWebView view)
    {
        if (handler.PlatformView == null || string.IsNullOrEmpty(view.HtmlContent))
            return;

        // Example CSS that matches DescriptionLabelStyle
        string css = @"
        body {
            font-size: 14px;
            color: #707070;
            margin: 0;
            padding-top: 5px;
            overflow-x: hidden;
        }
    ";

        // Inject the CSS into the HTML
        string fullHtml = $@"
        <html>
        <head>
            <meta name='viewport' content='width=device-width, initial-scale=1.0, user-scalable=no'>
            <style>{css}</style>
        </head>
        <body>
            {view.HtmlContent}
        </body>
        </html>
    ";
        handler.PlatformView?.LoadDataWithBaseURL(null, fullHtml, "text/html", "utf-8", null);
    }

    private class CustomWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        {
            var url = request.Url.ToString();
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            Platform.CurrentActivity.StartActivity(intent);
            return true;
        }
    }
}