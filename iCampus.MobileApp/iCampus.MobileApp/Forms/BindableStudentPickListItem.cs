using System.ComponentModel;

namespace iCampus.MobileApp.Forms;

public class BindableStudentPickListItem : INotifyPropertyChanged
{
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string AlternateId { get; set; }
    public string AvatarColor { get; set; }
    public string AvatarImagePath { get; set; }
    public int GenderId { get; set; }
    public string StudentName { get; set; }
    public string BirthdayNotficationMessage { get; set; }
    public string ClassName { get; set; }
    public bool HasTransport { get; set; }


    private byte[] _imageData;
    public byte[] ImageData
    {
        get
        {
            return _imageData;
        }
        set
        {
            _imageData = value;
            OnPropertyChanged("ImageData");
        }
    }
    int _serialNumber;
    public int SerialNumber
    {
        get { return _serialNumber; }
        set
        {
            _serialNumber = value;
            OnPropertyChanged("SerialNumber");
        }
    }
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
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