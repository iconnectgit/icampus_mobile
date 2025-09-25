using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.Calendar;

public partial class WeeklyPlanSearchPage : ContentPage
{
    public WeeklyPlanSearchPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "WeeklyExpandCollapse", (arg) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ForceNativeTableUpdate(beamWeeklyPlanList);
            });
        });
        
        MessagingCenter.Subscribe<string>("", "WeeklyGroupedAgendaListExpandCollapse", (arg) =>
        {
            //ForceNativeTableUpdate(beamWeeklyGroupedAgendaList);
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
        if (nativeListView != null)
        {
            nativeListView.ReloadData();
            nativeListView.BeginUpdates();
            nativeListView.EndUpdates();
        }
#endif
    }
}
}