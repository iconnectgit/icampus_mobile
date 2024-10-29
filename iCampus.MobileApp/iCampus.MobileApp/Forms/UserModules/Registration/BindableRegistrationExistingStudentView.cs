using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class BindableRegistrationExistingStudentView : RegistrationExistingStudentView, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool _detailsVisibility;

    public bool DetailsVisibility
    {
        get => _detailsVisibility;
        set
        {
            _detailsVisibility = value;
            OnPropertyChanged("DetailsVisibility");
        }
    }

    private string _arrowImageSource = "dropdown_gray.png";

    public string ArrowImageSource
    {
        get => _arrowImageSource;
        set
        {
            _arrowImageSource = value;
            OnPropertyChanged("ArrowImageSource");
        }
    }

    private bool _showTransportationDeposit;

    public bool ShowTransportationDeposit
    {
        get => _showTransportationDeposit;
        set
        {
            _showTransportationDeposit = value;
            OnPropertyChanged("ShowTransportationDeposit");
        }
    }

    private bool _transportationFeeCheckBoxChecked;

    public bool TransportationFeeCheckBoxChecked
    {
        get => _transportationFeeCheckBoxChecked;
        set
        {
            _transportationFeeCheckBoxChecked = value;
            OnPropertyChanged("TransportationFeeCheckBoxChecked");
        }
    }

    private bool _reRegistrationCheckBoxChecked;

    public bool ReRegistrationCheckBoxChecked
    {
        get => _reRegistrationCheckBoxChecked;
        set
        {
            _reRegistrationCheckBoxChecked = value;
            OnPropertyChanged("ReRegistrationCheckBoxChecked");
        }
    }

    public BindableRegistrationExistingStudentView()
    {
    }
}