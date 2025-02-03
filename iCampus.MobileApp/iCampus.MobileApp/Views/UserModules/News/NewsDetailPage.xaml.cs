using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("http") || e.Url.StartsWith("https"))
        {
            e.Cancel = true; 
            await Launcher.OpenAsync(e.Url); 
        }
    }
   
}