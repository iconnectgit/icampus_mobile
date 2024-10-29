using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.Registration;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "CurrentExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(existingListview);
        });
        MessagingCenter.Subscribe<string>("", "NextExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(newListview);
        });
        MessagingCenter.Subscribe<string>("", "StudentListExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(studentNameListview);
        });
        MessagingCenter.Subscribe<string>("", "NewStudentsExpandCollapse", (arg) =>
        {
            ForceNativeTableUpdate(newStudentListview);
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