using iCampus.MobileApp.Forms.UserModules.Resources;

namespace iCampus.MobileApp.Views.UserModules.Resources;

public partial class AddNewResourcePage : ContentPage
{
    public AddNewResourcePage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>("", "UpdateAttachmentListView", (arg) =>
        {
            //ForceNativeTableUpdate(listView);
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
    private void SelectAll_CheckChanged(object? sender, EventArgs eventArgs)
    {
        if (sender != null)
        {
            InputKit.Shared.Controls.CheckBox checkBox = (InputKit.Shared.Controls.CheckBox)sender;
            MessagingCenter.Send<string>(checkBox.IsChecked.ToString(), "AllClassSelection");
        }
    }

    private void Class_CheckChanged(object? sender, EventArgs eventArgs)
    {
        if (sender != null)
        {
            InputKit.Shared.Controls.CheckBox checkBox = (InputKit.Shared.Controls.CheckBox)sender;
            BindableResourcesPickListItem bindableResourcesPickListItem = new BindableResourcesPickListItem();
            bindableResourcesPickListItem.ItemName = checkBox.Text;
            bindableResourcesPickListItem.IsSelected = checkBox.IsChecked;
            MessagingCenter.Send<BindableResourcesPickListItem>(bindableResourcesPickListItem, "UpdateClassSelection");
        }
    }

    private void Published_CheckChanged(object? sender, EventArgs eventArgs)
    {
        if (sender != null)
        {
            InputKit.Shared.Controls.CheckBox checkBox = (InputKit.Shared.Controls.CheckBox)sender;
            MessagingCenter.Send<string>(checkBox.IsChecked.ToString(), "PublishSelection");
        }
    }
}