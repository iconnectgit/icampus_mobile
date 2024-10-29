using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.CampusKey;

public partial class PortalTopupHistoryPage : ContentPage
{
    public PortalTopupHistoryPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "OnlinePaymentExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamTopupHistoryListview);
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