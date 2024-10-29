using System.ComponentModel;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableAgendaWeeklyGroupView : AgendaWeeklyGroupView, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public BindableAgendaWeeklyGroupView()
    {
    }

    private string _arrowImageSource = "dropdown_gray.png";
    public string ArrowImageSource
    {
        get
        {
            return _arrowImageSource;
        }
        set
        {
            _arrowImageSource = value;
            OnPropertyChanged("ArrowImageSource");
        }
    }

    private bool _weeklyPostDetailsVisibility;
    public bool WeeklyPostDetailsVisibility
    {
        get
        {
            return _weeklyPostDetailsVisibility;
        }
        set
        {
            _weeklyPostDetailsVisibility = value;
            OnPropertyChanged("WeeklyPostDetailsVisibility");
        }
    }
}