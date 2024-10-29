using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.DailyMarks;

public class BindableDailyMarksModel: INotifyPropertyChanged
{
    BindableStudentGradingBookDataView _parentMarksData = new BindableStudentGradingBookDataView();
    public BindableStudentGradingBookDataView ParentMarksData
    {
        get { return _parentMarksData; }
        set
        {
            _parentMarksData = value;
            OnPropertyChanged("ParentMarksData");
        }
    }

    IList<BindableStudentGradingBookDataView> _subCourseList = new List<BindableStudentGradingBookDataView>();
    public IList<BindableStudentGradingBookDataView> SubCourseList
    {
        get { return _subCourseList; }
        set
        {
            _subCourseList = value;
            OnPropertyChanged("SubCourseList");
        }
    }
    
    int _listViewHeight;
    public int ListViewHeight
    {
        get => _listViewHeight;
        set
        {
            _listViewHeight = value;
            OnPropertyChanged(nameof(ListViewHeight));
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