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
   
}