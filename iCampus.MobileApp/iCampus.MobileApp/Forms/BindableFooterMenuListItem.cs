using System.ComponentModel;

namespace iCampus.MobileApp.Forms;

public class BindableFooterMenuListItem : INotifyPropertyChanged
{
    public string MenuTitle { get; set; }
    public string MenuSelectedImage { get; set; }
    public string MenuUnSelectedImage { get; set; }
    public string AvatarColor { get; set; }
    public bool IsSelected { get; set; }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler PropertyChanged;
}