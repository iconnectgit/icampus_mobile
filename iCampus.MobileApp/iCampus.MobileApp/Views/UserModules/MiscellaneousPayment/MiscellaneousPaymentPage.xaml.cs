using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms.UserModules.MiscellaneousPayment;

namespace iCampus.MobileApp.Views.UserModules.MiscellaneousPayment;

public partial class MiscellaneousPaymentPage : ContentPage
{
    public MiscellaneousPaymentPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "OnlinePaymentExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamHistoryListview);
        });
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
}