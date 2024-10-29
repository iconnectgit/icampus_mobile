using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableAgendaClassView : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public BindableAgendaClassView()
    {
    }
    public short ClassId
    {
        get;
        set;
    }

    public short GradeId
    {
        get;
        set;
    }

    public string ClassName
    {
        get;
        set;
    }

    public short CurriculumId
    {
        get;
        set;
    }

    public string CurriculumName
    {
        get;
        set;
    }

    public int CurriculumSequence
    {
        get;
        set;
    }

    public int DisplayOrder
    {
        get;
        set;
    }

    public bool IsElective
    {
        get;
        set;
    }
    Color _teamIconcolor = Colors.Black;
    public Color TeamIconcolor
    {
        get
        {
            return _teamIconcolor;
        }
        set
        {
            _teamIconcolor = value;
            OnPropertyChanged("TeamIconcolor");
        }
    }
    bool _isChecked;
    public bool IsChecked
    {
        get
        {
            return _isChecked;
        }
        set
        {
            _isChecked = value;
            OnPropertyChanged("IsChecked");
        }
    }

    bool _isAgendaPerStudent;
    public bool IsAgendaPerStudent
    {
        get
        {
            return _isAgendaPerStudent;
        }
        set
        {
            _isAgendaPerStudent = value;
            OnPropertyChanged("IsAgendaPerStudent");
        }
    }
}