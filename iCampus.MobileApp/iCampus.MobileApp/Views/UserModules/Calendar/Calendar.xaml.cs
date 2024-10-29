using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.Calendar;

public partial class Calendar : ContentPage
{
    public Calendar()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "ExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamGroupedgendaList);
        });
        MessagingCenter.Subscribe<string>("", "WeeklyExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamWeeklyPlanList);
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
}