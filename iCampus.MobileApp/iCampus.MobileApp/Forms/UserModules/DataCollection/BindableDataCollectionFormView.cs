using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.DataCollection;

public class BindableDataCollectionFormView : INotifyPropertyChanged
{
    public int FormId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public bool IsActive { get; set; }

    private int _studentId;
    public int StudentId
    {
        get { return _studentId; }
        set
        {
            _studentId = value;
            OnPropertyChanged("StudentId");
        }
    }

    private string _studentName;
    public string StudentName
    {
        get { return _studentName; }
        set
        {
            _studentName = value;
            OnPropertyChanged("StudentName");
        }

    }

    private string _formTitle;
    public string FormTitle
    {
        get { return _formTitle; }
        set
        {
            _formTitle = value;
            OnPropertyChanged("FormTitle");
        }
    }

    private bool _isRequired;
    public bool IsRequired
    {
        get { return _isRequired; }
        set
        {
            _isRequired = value;
            OnPropertyChanged("IsRequired");
        }
    }
    private bool _fillDataPerFamily;
    public bool FillDataPerFamily
    {
        get { return _fillDataPerFamily; }
        set
        {
            _fillDataPerFamily = value;
            OnPropertyChanged("FillDataPerFamily");
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