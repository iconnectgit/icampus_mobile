using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Handlers;
using Microsoft.Maui.Handlers;

#if ANDROID
using Android.Content;
using Android.Content.PM;
using Android.Net;
#endif


namespace iCampus.MobileApp.Views.UserModules.OnlinePayment;

public partial class PaymentWebView : ContentPage
{
    public PaymentWebView()
    {
        InitializeComponent();
        PaymentGatewayWebView.Navigating += OnWebViewNavigating;

    }
    private bool isSamsungPayHandling = false;

    private async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
    {
#if ANDROID
        if (isSamsungPayHandling)
            return;

        var url = e.Url;

        if (!string.IsNullOrEmpty(url) && (url.StartsWith("samsungpay://") || url.StartsWith("intent://")))
        {
            isSamsungPayHandling = true;
            e.Cancel = true; // Stop WebView from trying to load it

            string launchUrl = url;

            // Extract from intent://something#Intent;
            if (url.StartsWith("intent://"))
            {
                var parts = url.Split("#Intent");
                if (parts.Length > 0)
                    launchUrl = parts[0]; // Use plain URL without replacing to https
            }

            try
            {
                // Try to launch Samsung Pay
                var context = Android.App.Application.Context;
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(launchUrl));
                intent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                await DisplayAlert("Alert!", "Samsung Pay app is not installed.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to launch Samsung Pay.\n" + ex.Message, "OK");
            }
            finally
            {
                isSamsungPayHandling = false; // Reset the flag so it can be triggered again later
            }
        }
#endif
    }
}