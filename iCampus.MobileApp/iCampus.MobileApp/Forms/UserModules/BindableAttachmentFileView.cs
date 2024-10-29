using System.ComponentModel;
using iCampus.Common.Enums;

namespace iCampus.MobileApp.Forms.UserModules;

public class BindableAttachmentFileView : INotifyPropertyChanged
{
    public BindableAttachmentFileView() { }
    public BindableAttachmentFileView(string _fileName, string _filePath)
    {
        FileName = _fileName;
        FilePath = _filePath;
    }
    public string FileName { get; set; }
    public bool IsNewFile { get; set; }
    public string DisplayName { get; set; }
    public string FilePath { get; set; }
    public byte[] FileData { get; set; }
    public FileTypes FileType { get; set; }
    public int ParentFieldId { get; set; }

    int _fileStatus;
    public int FileStatus
    {
        get { return _fileStatus; }
        set
        {
            _fileStatus = value;
            OnPropertyChanged("FileStatus");
        }
    }
    string _fileDevicePath;
    public string FileDevicePath {
        get { return _fileDevicePath; }
        set
        {
            _fileDevicePath = value;
            OnPropertyChanged("FileDevicePath");
        }
    }
    private int _attachmentMessageId { get; set; }
    public int AttachmentMessageId
    {
        get
        {
            return _attachmentMessageId;
        }
        set
        {
            _attachmentMessageId = value;
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