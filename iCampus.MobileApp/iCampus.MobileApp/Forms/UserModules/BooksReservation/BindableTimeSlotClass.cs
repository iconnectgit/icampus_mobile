using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.BooksReservation;

public class BindableTimeSlotClass : INotifyPropertyChanged
{
    public string TimeSlotString { get; set; }

    Color _selectedTimeSlotBackgroundColor;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public Color SelectedTimeSlotBackgroundColor
    {
        get
        {
            return _selectedTimeSlotBackgroundColor;
        }
        set
        {
            _selectedTimeSlotBackgroundColor = value;
            OnPropertyChanged("SelectedTimeSlotBackgroundColor");
        }
    }

    string _date;
    public string Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
            OnPropertyChanged("Date");
        }
    }
}