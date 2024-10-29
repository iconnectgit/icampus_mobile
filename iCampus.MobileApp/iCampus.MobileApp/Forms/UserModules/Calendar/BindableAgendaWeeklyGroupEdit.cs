using System.ComponentModel;
using iCampus.Portal.EditModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableAgendaWeeklyGroupEdit : AgendaWeeklyGroupEdit, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public BindableAgendaWeeklyGroupEdit() : base()
    {
    }
}