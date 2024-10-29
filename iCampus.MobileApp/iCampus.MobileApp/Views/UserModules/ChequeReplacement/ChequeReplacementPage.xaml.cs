namespace iCampus.MobileApp.Views.UserModules.ChequeReplacement;

public partial class ChequeReplacementPage : ContentPage
{
    public ChequeReplacementPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "OnlinePaymentExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(beamHistoryListview);
            ForceNativeTableUpdate(beamPendingListView);
            ForceNativeTableUpdate(beamBouncedListView);
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
