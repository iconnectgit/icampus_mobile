using System.ComponentModel;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class BindableAttendanceEntryView : INotifyPropertyChanged
{
    public int StudentAttendanceDetailId { get; set; }

    public int StudentAttendanceMasterId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public short? TakenByTeacherId { get; set; }

    public string TakenByTeacherName { get; set; }
    public string ModifiedTeacherName { get; set; }

    public short? ModifiedTeacherId { get; set; }

    public short? PeriodId { get; set; }

    public short? ClassId { get; set; }
    public short? CurriculumId { get; set; }

    public short? ElectiveClassId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentShortName { get; set; }

    private TimeSpan _selectedTime;

    public TimeSpan SelectedTime
    {
        get
        {
            _selectedTime = GetSelectedTime();
            return _selectedTime;
        }
        set
        {
            _selectedTime = value;
            OnPropertyChanged("SelectedTime");
            SetSelectedTime(value);
        }
    }

    private BindablePickListItem _selectedType;

    public BindablePickListItem SelectedType
    {
        get
        {
            _selectedType = GetSelectedType();
            return _selectedType;
        }
        set
        {
            SetSelectedType(value);
            _selectedType = value;
            OnPropertyChanged("SelectedType");
        }
    }

    private bool _isTimeVisible;

    public bool IsTimeVisible
    {
        get => _isTimeVisible;
        set
        {
            _isTimeVisible = value;
            OnPropertyChanged("IsTimeVisible");
        }
    }

    private bool _isOtherComment;

    public bool IsOtherComment
    {
        get => _isOtherComment;
        set
        {
            _isOtherComment = value;
            OnPropertyChanged("IsOtherComment");
        }
    }

    private IList<ExtPickListItem> _attendanceCommentList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> AttendanceCommentList
    {
        get => _attendanceCommentList;
        set
        {
            _attendanceCommentList = value;
            OnPropertyChanged("AttendanceCommentList");
        }
    }

    public bool IsAbsent { get; set; }
    public bool IsLate { get; set; }
    public bool IsLeaveEarly { get; set; }
    public bool IsExcused { get; set; }
    public string LeaveEarlyTime { get; set; }
    public string LateTime { get; set; }
    public string AttendanceCommentTitle { get; set; }

    public short? AttendanceCommentId { get; set; }

    private string _otherComments;

    public string OtherComments
    {
        get => _otherComments;
        set
        {
            _otherComments = value;
            OnPropertyChanged("OtherComments");
        }
    }

    private ExtPickListItem _selectedComment;

    public ExtPickListItem SelectedComment
    {
        get { return _selectedComment = GetSelectedComment(); }
        set
        {
            _selectedComment = value;
            OnPropertyChanged("SelectedComment");
            SetSelectedComment(value);
        }
    }

    public int AbsentCount { get; set; }
    public int LateCount { get; set; }
    public int LeftEarlyCount { get; set; }
    public bool AttendancePerPeriod { get; set; }
    public string AttendanceMode { get; set; }
    public bool IsAbsentWholeDay { get; set; }
    public bool IsLateWholeDay { get; set; }
    public bool HasParentNotificationData { get; set; }
    public string HighlightCssClass { get; set; }

    public string AttendanceAbsentHistotry { get; set; }
    public List<RecentAttendanceHistoryView> RecentAttendanceAbsentList { get; set; }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public BindablePickListItem GetSelectedType()
    {
        var TypeList = HelperMethods.GetAttendanceType();
        if (IsAbsent)
        {
            IsTimeVisible = false;
            return TypeList.Where(x => x.ItemId == "A").FirstOrDefault();
        }
        else if (IsLate)
        {
            IsTimeVisible = true;
            return TypeList.Where(x => x.ItemId == "L").FirstOrDefault();
        }
        else if (IsLeaveEarly)
        {
            IsTimeVisible = true;
            return TypeList.Where(x => x.ItemId == "E").FirstOrDefault();
        }
        else
        {
            IsTimeVisible = false;
            return TypeList.Where(x => x.ItemId == "P").FirstOrDefault();
        }
    }

    public void SetSelectedType(BindablePickListItem selectedItem)
    {
        switch (selectedItem.ItemId)
        {
            case "P":
                IsAbsent = false;
                IsLate = false;
                IsLeaveEarly = false;
                IsTimeVisible = false;
                break;
            case "A":
                IsAbsent = true;
                IsLate = false;
                IsLeaveEarly = false;
                IsTimeVisible = false;
                break;
            case "L":
                IsAbsent = false;
                IsLate = true;
                IsTimeVisible = true;
                break;
            case "E":
                IsAbsent = false;
                IsLeaveEarly = true;
                IsTimeVisible = true;
                break;
            default:
                IsAbsent = false;
                IsLate = false;
                IsLeaveEarly = false;
                IsTimeVisible = false;
                break;
        }
    }

    public TimeSpan GetSelectedTime()
    {
        TimeSpan res;
        if (IsLate && LateTime != null)
        {
            var val = Convert.ToDateTime(LateTime);
            res = val.TimeOfDay;
        }
        else if (IsLeaveEarly && LeaveEarlyTime != null)
        {
            var val = Convert.ToDateTime(LeaveEarlyTime);
            res = val.TimeOfDay;
        }
        else
        {
            res = TimeSpan.Zero;
        }

        return res;
    }

    public void SetSelectedTime(TimeSpan timeSpan)
    {
        try
        {
            if (timeSpan != null && timeSpan != TimeSpan.MinValue)
            {
                if (IsLate)
                    LateTime = timeSpan.ToDateTime().ToString(TextResource.TodaysDateTimeFormatKey);
                if (IsLeaveEarly)
                    LeaveEarlyTime = timeSpan.ToDateTime().ToString(TextResource.TodaysDateTimeFormatKey);
            }
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    public ExtPickListItem GetSelectedComment()
    {
        return AttendanceCommentList.Where(x => x.ItemId == AttendanceCommentId.ToString()).FirstOrDefault();
    }

    public void SetSelectedComment(ExtPickListItem selectedComment)
    {
        if (selectedComment != null)
        {
            AttendanceCommentId = selectedComment.ItemId.ToShort();
            AttendanceCommentTitle = selectedComment.ItemName;
            if (selectedComment.ItemName.ToLower().Equals("other"))
                IsOtherComment = true;
            else
                IsOtherComment = false;
        }
    }
}