using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.Calendar;

public partial class QuickPostPage : ContentPage
{
    public QuickPostPage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "UpdateAttachmentListView", (arg) =>
        {
            ForceNativeTableUpdate(listview);
        });
    }
    public void ForceNativeTableUpdate(CollectionView listView)
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

    private void SelectAll_CheckChanged(object? sender, EventArgs e)
    {
        if (sender != null)
        {
            InputKit.Shared.Controls.CheckBox checkBox = (InputKit.Shared.Controls.CheckBox)sender;
            MessagingCenter.Send<string>(checkBox.IsChecked.ToString(), "AllClassSelection");
        }
    }
}