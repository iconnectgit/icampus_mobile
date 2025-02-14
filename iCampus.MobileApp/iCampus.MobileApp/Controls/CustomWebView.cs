using Microsoft.Maui.Controls;

namespace iCampus.MobileApp.Controls;

public class CustomWebView : WebView
{
    public static readonly BindableProperty HtmlContentProperty = 
        BindableProperty.Create(nameof(HtmlContent), typeof(string), typeof(CustomWebView), default(string), propertyChanged: OnHtmlContentChanged);

    public string HtmlContent
    {
        get => (string)GetValue(HtmlContentProperty);
        set => SetValue(HtmlContentProperty, value);
    }

    private static void OnHtmlContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomWebView webView && newValue is string htmlContent)
        {
            webView.Source = new HtmlWebViewSource { Html = htmlContent };
        }
    }
}