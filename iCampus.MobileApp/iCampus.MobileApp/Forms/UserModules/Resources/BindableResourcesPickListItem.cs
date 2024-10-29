using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class BindableResourcesPickListItem : INotifyPropertyChanged
{
    public string ItemId
    {
        get;
        set;
    }

    public string ItemName
    {
        get;
        set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            OnPropertyChanged("IsSelected");
        }
    }
}