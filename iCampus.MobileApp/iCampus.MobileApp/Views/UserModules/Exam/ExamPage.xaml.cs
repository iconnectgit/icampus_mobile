using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace iCampus.MobileApp.Views.UserModules.Exam;

public partial class ExamPage : ContentPage
{
    public ExamPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "ExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(listView);
        });
    }

    protected override bool OnBackButtonPressed()
    {
        var currentViewModel = BindingContext as ViewModelBase;

        if (currentViewModel != null)
        {
            currentViewModel.HandleMenuSelectionOnBack();
        }
        return true;
    }
    public void ForceNativeTableUpdate(ListView listView)
    {
        if (listView.Handler != null)
        {
#if ANDROID
            var nativeListView = listView.Handler.PlatformView as AndroidX.RecyclerView.Widget.RecyclerView;
            nativeListView?.GetAdapter()?.NotifyDataSetChanged();
#elif IOS || MACCATALYST
            var nativeListView = listView.Handler.PlatformView as UIKit.UITableView;
            nativeListView?.ReloadData();
#endif
        }
    }
    private async void WebView_Navigating(object? sender, WebNavigatingEventArgs e)
    {
        if (sender is WebView webView) // Get the WebView instance from the sender
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
                        webView.HeightRequest = height; // Set the height for the specific WebView
                    });
                }
            }
        }
    }
}