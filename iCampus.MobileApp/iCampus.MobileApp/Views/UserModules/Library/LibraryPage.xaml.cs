using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.Library;

public partial class LibraryPage : ContentPage
{
    public LibraryPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "CurrentExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamCurrentListview);
        });
        MessagingCenter.Subscribe<string>("", "HistoryExpandCollapse", (arg) =>
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