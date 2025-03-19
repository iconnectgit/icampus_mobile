namespace iCampus.MobileApp.Views.UserModules.Certificates;

public partial class CertificatesPage : ContentPage
{
    public CertificatesPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "UpdateCertificateList", (arg) =>
        {
            ForceNativeTableUpdate(CertificateListview);
        });
    }
    public async void ForceNativeTableUpdate(ListView listView)
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