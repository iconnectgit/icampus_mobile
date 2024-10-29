using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Library;

public class BindableLibraryView : INotifyPropertyChanged
{

    public BindableLibraryView()
    {

    }

    public string GroupName { get; set; }

    public string Media { get; set; }

    public string CopyBarcode { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Classification { get; set; }

    public string DueDate { get; set; }

    public string ReturnDate { get; set; }

    private bool _detailsVisibility;
    public bool DetailsVisibility
    {
        get
        {
            return _detailsVisibility;
        }
        set
        {
            _detailsVisibility = value;
            OnPropertyChanged("DetailsVisibility");
        }
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
    private bool _isOverDue = false;
    public bool IsOverDue
    {
        get
        {
            return _isOverDue;
        }
        set
        {
            _isOverDue = value;
            OnPropertyChanged("DetailsVisibility");
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