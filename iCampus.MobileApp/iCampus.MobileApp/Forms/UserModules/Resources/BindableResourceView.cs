using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class BindableResourceView : INotifyPropertyChanged
{
    public BindableResourceView()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public int FamilyId { get; set; }

    public string FamilyName { get; set; }

    public bool IsMySelf { get; set; }

    public int Type { get; set; }

    public string FirstName { get; set; }

    public int StudentId { get; set; }

    public string StudentGrade { get; set; }

    public int ClassId { get; set; }

    public int ResourceId { get; set; }

    public string Title { get; set; }

    public string Title_1 { get; set; }

    public string Title_2 { get; set; }

    public string Title_3 { get; set; }

    public int CreatedTypeId { get; set; }

    public int ResourceGrade { get; set; }

    public bool IsParent { get; set; }

    public bool IsStudent { get; set; }

    public bool IsTeacher { get; set; }

    public string Data { get; set; }

    public int UserId { get; set; }

    public int? TermId { get; set; }

    public string Date { get; set; }

    public string AttachmentFile { get; set; }

    public short TransMode { get; set; }

    public bool IsAddMode { get; set; }

    public bool IsPublished { get; set; }

    public string SelectedClasses { get; set; }

    public List<BindableResourceAttachmentView> AttachmentList { get; set; }

    public IEnumerable<BindableResourceClassView> ResourceClassList { get; set; }

    public List<AttachmentFileView> AttachmentFileList { get; set; }

    public int? GradeId { get; set; }

    public string Status { get; set; }

    public string UserName { get; set; }

    public int UserTypeId { get; set; }

    public string[] AttachmentsArray { get; set; }

    public string[] DeletedAttachmentsArray { get; set; }

    public string ExistingAttachmentIds { get; set; }
    public int? CurriculumId { get; set; }

    public string CurriculumName { get; set; }

    private bool _descriptionVisibility;

    public bool DescriptionVisibility
    {
        get => _descriptionVisibility;
        set
        {
            _descriptionVisibility = value;
            OnPropertyChanged("DescriptionVisibility");
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

    private bool _isCourseVisible;

    public bool IsCourseVisible
    {
        get => _isCourseVisible;
        set
        {
            _isCourseVisible = value;
            OnPropertyChanged("IsCourseVisible");
        }
    }
}

public class BindableResourceAttachmentView
{
    public int AttachmentId { get; set; }

    public string AttachmentPath { get; set; }

    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public string AttachmentFile { get; set; }

    public int ResourceId { get; set; }

    public string AttachmentFiles { get; set; }

    public string DeletedAttachmentFiles { get; set; }

    public int UserRefId { get; set; }
}