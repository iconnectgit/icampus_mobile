using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class Week: INotifyPropertyChanged
{
    public int DisplayDate { get; set; }
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public string DisplayDayOfWeek { get; set; }
    public bool IsToday { get; set; }
    private bool _isSelected;
    public bool IsSelected {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            OnPropertyChanged("IsSelected");
        }
    }
    private ObservableCollection<string> _colors;
    public ObservableCollection<string> Colors
    {
        get { return _colors; }
        set
        {
            _colors = value;
            OnPropertyChanged("Colors");
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