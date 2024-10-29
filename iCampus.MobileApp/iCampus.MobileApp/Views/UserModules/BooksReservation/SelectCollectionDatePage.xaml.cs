using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.BooksReservation;

public partial class SelectCollectionDatePage : ContentPage
{
    public SelectCollectionDatePage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "ExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(listView);
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