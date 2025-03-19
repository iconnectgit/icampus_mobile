using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Certificates;

public class BindableCertificateView : INotifyPropertyChanged
{
    public int CertificateId { get; set; }

    public string CertificateName { get; set; }

    public string FileNameFormat { get; set; }

    public string AlternateId { get; set; }

    public bool FileExist { get; set; }

    public string CertificateFilePath { get; set; }

    public string ErrorMessage { get; set; }
    
    
    private bool _isSelected;
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(IsSelected)));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        });
    }

}