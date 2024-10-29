using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class BindableSeries : INotifyPropertyChanged
{
    public BindableSeries()
    {
    }
    public string name
    {
        get;
        set;
    }

    public double y
    {
        get;
        set;
    }

    public string color
    {
        get;
        set;
    }

    public bool visible
    {
        get;
        set;
    }

    public decimal[] data
    {
        get;
        set;
    }

    double _countPercentage;
    public double CountPercentage
    {
        get
        {

            return _countPercentage;
        }
        set
        {
            _countPercentage = value;
            OnPropertyChanged("CountPercentage");
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}