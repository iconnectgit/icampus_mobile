using System.ComponentModel;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.Portal.Enums;

namespace iCampus.MobileApp.Forms.UserModules.Exam;

public class BindableExamScheduleView : INotifyPropertyChanged
    {
		public int ExamId
		{
			get;
			set;
		}

		public Guid ExamUId
		{
			get;
			set;
		}

		public DateTime? ExamDate
		{
			get;
			set;
		}

		public string ExamTime
		{
			get;
			set;
		}

		public string CurriculumName
		{
			get;
			set;
		}

		public string CurriculumShortName
		{
			get;
			set;
		}

		public bool IsElective
		{
			get;
			set;
		}

		public DateTime WeekEndDate
		{
			get;
			set;
		}

		public DateTime WeekStartDate
		{
			get;
			set;
		}

		public int WeekDayId
		{
			get;
			set;
		}

		public bool IsPostedByMe
		{
			get;
			set;
		}

		public string ExamRequirements
		{
			get;
			set;
		}

		public string ExamName
		{
			get;
			set;
		}

		public string ExamsDescription
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public IEnumerable<BindableAttachmentFileView>ExamFiles
		{
			get;
			set;
		}

		public IEnumerable<PickListItem> ExamCoursesList
		{
			get;
			set;
		}

		public int TeacherId
		{
			get;
			set;
		}

		public string TeacherName
		{
			get;
			set;
		}

		public bool HasWriteAccess
		{
			get;
			set;
		}

		public string ReviewStatus
		{
			get;
			set;
		}

		public string RejectionNotes
		{
			get;
			set;
		}

		public bool IsExamApprovalPermissionEnabled
		{
			get;
			set;
		}

		public bool HasValidAttachments
		{
			get;
			set;
		}

		public short GradeId
		{
			get;
			set;
		}

		public ExamScheduleViewModes ViewMode
		{
			get;
			set;
		}

		public string ExistingAttachmentIds
		{
			get;
			set;
		}

		public string[] AttachmentsArray
		{
			get;
			set;
		}

		public string[] DeletedAttachmentsArray
		{
			get;
			set;
		}

		public BindableExamScheduleView()
        {

        }
		public bool HasEditPermission(int? userRefId, bool isTeacher)
		{
			return isTeacher && (TeacherId == userRefId || HasWriteAccess);
		}

		public bool IsEmpty()
		{
			return ExamId == 0;
		}
		public bool IsApproved()
		{
			return ReviewStatus.Equals(StringEnum.GetStringValue(ExamApprovalReviewStatus.Approved), StringComparison.OrdinalIgnoreCase);
		}
		public bool IsRejected()
		{
			return ReviewStatus.Equals(StringEnum.GetStringValue(ExamApprovalReviewStatus.Rejected), StringComparison.OrdinalIgnoreCase);
		}
		public bool IsPending()
		{
			return ReviewStatus.Equals(StringEnum.GetStringValue(ExamApprovalReviewStatus.Pending), StringComparison.OrdinalIgnoreCase);
		}
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


		private int _listViewHeight;
		public int ListViewHeight
		{
			get
			{
				_listViewHeight = ExamFiles.Count() * 50;
				return _listViewHeight;
			}
			set
			{
				_listViewHeight = value;
				OnPropertyChanged("ListViewHeight");
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

	    bool _arrowImageSourceVisibility = true;
		public bool ArrowImageSourceVisibility
		{
			get
			{
				    _arrowImageSourceVisibility = string.IsNullOrEmpty(ExamRequirements) && ExamFiles.Count() == 0 ? false : true;
					return _arrowImageSourceVisibility;
			}
			set
			{
				_arrowImageSourceVisibility = value;
				OnPropertyChanged("ArrowImageSourceVisibility");
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