using System.ComponentModel;

namespace iCampus.MobileApp.Forms;

public class PushNotificationSettings : INotifyPropertyChanged
{
    public static PushNotificationSettings Current = new PushNotificationSettings();
    public event PropertyChangedEventHandler PropertyChanged;
    private bool _isPushNotificationEnabled = new Boolean();
    public bool IsPushNotificationEnabled
    {
        get
        {
            return _isPushNotificationEnabled;
        }
        set
        {
            _isPushNotificationEnabled = value;
            OnPropertyChanged("IsPushNotificationEnabled");
        }
    }
    private int _pushNotificationCount = 0;
    public int PushNotificationCount
    {
        get
        {
            return _pushNotificationCount;
        }
        set
        {
            _pushNotificationCount = value;
            OnPropertyChanged("PushNotificationCount");
        }
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}