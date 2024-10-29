using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class BindablePickListItem : INotifyPropertyChanged
{
    string _itemId;
    public string ItemId
    {
        get { return _itemId; }
        set
        {
            _itemId = value;
            OnPropertyChanged("ItemId");
        }
    }

    string _itemName;
    public string ItemName
    {
        get { return _itemName; }
        set
        {
            _itemName = value;
            OnPropertyChanged("ItemName");
        }
    }

    string _itemColor;
    public string ItemColor
    {
        get { return _itemColor; }
        set
        {
            _itemColor = value;
            OnPropertyChanged("ItemColor");
        }
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler PropertyChanged;
}