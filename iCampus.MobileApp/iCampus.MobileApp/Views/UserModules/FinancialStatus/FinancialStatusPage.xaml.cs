using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Views.UserModules.FinancialStatus;

public partial class FinancialStatusPage : ContentPage
{
    public FinancialStatusPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "FinancialDetailsExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(FinancialListView);
            ForceNativeTableUpdate(HistoryListView);
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
    protected override bool OnBackButtonPressed()
    {
        var currentViewModel = BindingContext as ViewModelBase;

        if (currentViewModel != null)
        {
            currentViewModel.HandleMenuSelectionOnBack();
        }
        return true;
    }
}