using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.DailyMarks;

public class BindableGradingBookView
{
    public IList<ColumnHeaderView> ColumnHeaderData
    {
        get;
        set;
    }

    public int MaxHeaderRows
    {
        get;
        set;
    }

    public IList<GradingBookDataView> GradingBookData
    {
        get;
        set;
    }

    public BindablePermissions Permissions
    {
        get;
        set;
    }

    public IList<StudentGradingBookDataView> GradingBookDataPerStudent
    {
        get;
        set;
    }

    public IList<MakeupGradingBookDataView> MakeupGradingBookData
    {
        get;
        set;
    }

    public List<ParentTree> ColumnHeaderTreeData
    {
        get;
        set;
    }

    public IList<ColumnHeaderView> FilteredColumnHeaderData
    {
        get;
        set;
    }

    public int FilteredMaxHeaderRows
    {
        get;
        set;
    }

    public bool IsAtleastOneCourseHasEffort
    {
        get;
        set;
    }

    public int MaxLetterLength
    {
        get;
        set;
    }

    public bool UseTextBox
    {
        get;
        set;
    }

    public PanelHeaderInfo PanelHearInformation
    {
        get;
        set;
    }

    public bool HasChildren
    {
        get;
        set;
    }

    public IList<ExtPickListItem> TermList
    {
        get;
        set;
    }

    public IList<ExtPickListItem> StudentCourses
    {
        get;
        set;
    }
}
public class BindablePermissions
{
    public bool CanEditFinalAverage { get; set; }
    public bool ViewAverageColumns { get; set; }
    public bool ViewPreAverageColumn { get; set; }
    public string NoModuleAccessMessage { get; set; }
    public string DisableBlockedStudentMessage { get; set; }
}