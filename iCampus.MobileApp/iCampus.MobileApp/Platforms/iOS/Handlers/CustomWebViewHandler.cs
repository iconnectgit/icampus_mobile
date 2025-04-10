using System;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using iCampus.MobileApp.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Handlers;
using WebKit;
using UIKit;

namespace iCampus.MobileApp.Handlers;

public class CustomWebViewHandler : ViewHandler<CustomWebView, WKWebView>
{
    public static IPropertyMapper<CustomWebView, CustomWebViewHandler> Mapper = new PropertyMapper<CustomWebView, CustomWebViewHandler>(ViewMapper)
    {
        [nameof(CustomWebView.HtmlContent)] = MapHtmlContent
    };

    private WKUserContentController _userContentController;
    private const string HeightScriptHandler = "heightHandler";
    private WeakReference<CustomWebView> _weakVirtualView;

    public CustomWebViewHandler() : base(Mapper)
    {
    }

    protected override WKWebView CreatePlatformView()
    {
        _userContentController = new WKUserContentController();
        var config = new WKWebViewConfiguration
        {
            UserContentController = _userContentController,
            Preferences = new WKPreferences
            {
            JavaScriptEnabled = true 
        }
        };

        var webView = new WKWebView(CGRect.Empty, config)
        {
            NavigationDelegate = new CustomWebViewDelegate(this),
            ScrollView =
            {
                ScrollEnabled = true, 
                Bounces = false,
                AlwaysBounceVertical = false,
                AlwaysBounceHorizontal = true, 
                ShowsHorizontalScrollIndicator = true 
            }
        };

        _userContentController.AddScriptMessageHandler(new ScriptMessageHandler(this), HeightScriptHandler);

        return webView;
    }

    protected override void ConnectHandler(WKWebView platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView != null)
        {
            _weakVirtualView = new WeakReference<CustomWebView>(VirtualView);
            //InjectHeightListener(platformView);
            //LoadHtml(platformView, VirtualView.HtmlContent);
        }
    }

    protected override void DisconnectHandler(WKWebView platformView)
    {
        base.DisconnectHandler(platformView);
        _weakVirtualView = null;
        _userContentController.RemoveScriptMessageHandler(HeightScriptHandler);
    }

    private static void MapHtmlContent(CustomWebViewHandler handler, CustomWebView view)
    {
        handler.LoadHtml(handler.PlatformView, view.HtmlContent);
    }

    private void LoadHtml(WKWebView webView, string html)
    {
        var currentPage = Application.Current.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
        bool isExamPage = currentPage?.GetType().Name.Equals("ExamPage", StringComparison.CurrentCultureIgnoreCase) == true;
        var isExamPageJsValue = isExamPage ? "true" : "false";

        
        if (webView == null || string.IsNullOrEmpty(html))
            return;

        string fullHtml = $@"
        <html>
        <head>
            <meta name='viewport' content='width=device-width, initial-scale=1.0, user-scalable=no'>
            <style>
                body {{ margin: 0; padding-top: 10px;; overflow-x: hidden; }}
            </style>
            <script>
                var isExamPage = {isExamPageJsValue};


                function reportHeight() {{
                    setTimeout(() => {{
                        let body = document.body;
                        let html = document.documentElement;

                        let height = Math.max(
                            body.scrollHeight, body.offsetHeight,
                            html.clientHeight, html.scrollHeight, html.offsetHeight
                        );

                        let deviceScale = window.devicePixelRatio || 1;
                        let adjustedHeight = height / deviceScale;
                        
                        let finalHeight = isExamPage ? adjustedHeight : height;
                        
                        window.webkit.messageHandlers.heightHandler.postMessage(finalHeight);
                    }}, 100);
                }}
                window.onload = reportHeight;
                document.addEventListener('DOMContentLoaded', reportHeight);
            </script>
        </head>
        <body onload='reportHeight()'>
            {html}
        </body>
        </html>";
        
        webView.LoadHtmlString(fullHtml, null);
    }

    private class CustomWebViewDelegate : WKNavigationDelegate
    {
        private readonly WeakReference<CustomWebViewHandler> _handlerRef;

        public CustomWebViewDelegate(CustomWebViewHandler handler)
        {
            _handlerRef = new WeakReference<CustomWebViewHandler>(handler);
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (navigationAction.NavigationType == WKNavigationType.LinkActivated && navigationAction.Request.Url != null)
            {
                var url = navigationAction.Request.Url;
                if (UIApplication.SharedApplication.CanOpenUrl(url))
                {
                    UIApplication.SharedApplication.OpenUrl(url, new UIApplicationOpenUrlOptions(), null);
                }
                decisionHandler(WKNavigationActionPolicy.Cancel);
            }
            else
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
        }

    }

    private class ScriptMessageHandler : NSObject, IWKScriptMessageHandler
    {
        private readonly WeakReference<CustomWebViewHandler> _handlerRef;

        public ScriptMessageHandler(CustomWebViewHandler handler)
        {
            _handlerRef = new WeakReference<CustomWebViewHandler>(handler);
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (_handlerRef.TryGetTarget(out var handler) && handler._weakVirtualView.TryGetTarget(out var view))
            {
                if (message.Body is NSNumber height && height.FloatValue > 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (view.Handler == handler && Math.Abs(view.HeightRequest - height.FloatValue) > 5) // Prevent frequent updates
                        {
                            view.HeightRequest = height.FloatValue;
                        }
                    });
                }
            }
        }
    }
}
