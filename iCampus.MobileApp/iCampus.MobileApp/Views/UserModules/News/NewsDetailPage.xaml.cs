using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace iCampus.MobileApp.Views.UserModules.News;

public partial class NewsDetailPage : ContentPage
{
    public NewsDetailPage()
    {
        InitializeComponent();
    }
    void Details_SwipeRight(System.Object sender, System.EventArgs e)
    {
        MessagingCenter.Send("", "ListViewRightSwipeNewsDetails");
    }
    private async void WebView_Navigating(object? sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("http") || e.Url.StartsWith("https"))
        {
            e.Cancel = true; 
            await Launcher.OpenAsync(e.Url); 
            return;
        }

        if (e.Url.StartsWith("webview://height="))
        {
            e.Cancel = true;
            var heightStr = e.Url.Replace("webview://height=", "");
            if (int.TryParse(heightStr, out int height))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    beamDetails.HeightRequest = height;
                });
            }
        }
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        beamDetails.Navigating += WebView_Navigating;
    }

   
}