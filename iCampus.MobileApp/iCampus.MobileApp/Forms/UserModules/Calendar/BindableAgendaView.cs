using System.ComponentModel;
using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableAgendaView : AgendaBase, INotifyPropertyChanged
	{
        public BindableAgendaView()
        {
        }
		public string TeacherName
		{
			get;
			set;
		}

		public string Assignment
		{
			get;
			set;
		}

		public string TypeTitle
		{
			get;
			set;
		}

		public int AgendaCount
		{
			get;
			set;
		}
        public int CurriculumId
        {
            get;
            set;
        }
        public string FormatedPostDate
		{
			get;
			set;
		}

		public DateTime ApprovedDate
		{
			get;
			set;
		}

		public string FormattedApprovedDate
		{
			get;
			set;
		}

		public string AgendaColor
		{
			get;
			set;
		}

		public string WorkTypeColor
		{
			get;
			set;
		}

		public string AgendaDateString
		{
			get;
			set;
		}

		public int? StudentId
		{
			get;
			set;
		}

		public string StudentName
		{
			get;
			set;
		}

		public bool IsAgenda
		{
			get;
			set;
		}

		public bool IsElective
		{
			get;
			set;
		}

		public string PostOwnerName
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public string ApprovalStatus
		{
			get;
			set;
		}

		string _curriculumName;
		public string CurriculumName
		{
			get
            {
				return _curriculumName;
            }
			set
            {
				_agendaDetailsVisibility = string.IsNullOrEmpty(CurriculumName) ? true : false;
				_curriculumName = value;
				OnPropertyChanged("CurriculumName");
			}

		}

		public string CurriculumNameWithDeletedAgenda
		{
			get;
			set;
		}

		public short? ClassId
		{
			get;
			set;
		}

		public string ClassName
		{
			get;
			set;
		}

		public string ElectiveClassName
		{
			get;
			set;
		}

		public string AgendaClassNames
		{
			get;
			set;
		}

		public int OwnerIndicatorFlag
		{
			get;
			set;
		}

		public string OwnerIndicator
		{
			get;
			set;
		}

		public int? AttachmentId
		{
			get;
			set;
		}

		public string Attachment
		{
			get;
			set;
		}

		public string AttachmentIds
		{
			get;
			set;
		}

		public string TooltipJsonString
		{
			get;
			set;
		}

		public string CreatedFor
		{
			get;
			set;
		}

		public int UsedBy
		{
			get;
			set;
		}

		public string Actions
		{
			get;
			set;
		}

		public int? DeletedBy
		{
			get;
			set;
		}

		public DateTime? DeletedDate
		{
			get;
			set;
		}

		public string DeletedByTeacherName
		{
			get;
			set;
		}

		public bool IsSupervisorUser
		{
			get;
			set;
		}

		public int AttachmentCount
		{
			get;
			set;
		}

		public List<AttachmentFileView> AttachmentList
		{
			get;
			set;
		}

		public string AgendaBrief
		{
			get;
			set;
		}

		public bool IsWeekly
		{
			get;
			set;
		}

		public string AttachmentDisplayName
		{
			get;
			set;
		}

		public IEnumerable<WebsiteLinkView> WebsiteLinks
		{
			get;
			set;
		}

		public int WebsiteLinksCount
		{
			get;
			set;
		}

		public bool IsGrouping
		{
			get;
			set;
		}

		public IEnumerable<StudentPickListItem> StudentData
		{
			get;
			set;
		}

		public bool IsStudentSubmissionAllowed
		{
			get;
			set;
		}

		public string SubmissionCountLevel
		{
			get;
			set;
		}

		public string LastSubmissionDate
		{
			get;
			set;
		}

		public int SubmissionCount
		{
			get;
			set;
		}

		public int AgendaStudentCount
		{
			get;
			set;
		}

		public bool IsDownloadAllowed
		{
			get;
			set;
		}

		public string StudentComments
		{
			get;
			set;
		}

		public IList<StudentSubmissionAttachmentView> StudentSubmittedFilesList
		{
			get;
			set;
		}

		public int StudentSubmissionMasterId
		{
			get;
			set;
		}

		public IList<AgendaClassStudentView> AgendaClassStudents
		{
			get;
			set;
		}

		private string _arrowImageSource= "dropdown_gray.png";
		public string ArrowImageSource
		{
			get {
				return _arrowImageSource;
			}
			set
			{
				_arrowImageSource = value;
				OnPropertyChanged("ArrowImageSource");
			}
		}

		private bool _agendaDetailsVisibility;
		public bool AgendaDetailsVisibility
		{
			get
			{
				return _agendaDetailsVisibility;
			}
			set
			{
                //if (string.IsNullOrEmpty(CurriculumName))
                //{
                //    _agendaDetailsVisibility = true;
                //}
                //else
                //{
                //    _agendaDetailsVisibility = false;

                //}
				_agendaDetailsVisibility = value;
				OnPropertyChanged("AgendaDetailsVisibility");
			}
		}
		public IEnumerable<AgendaView> AgendaAttachmentList
		{
			get;
			set;
		}
		bool _isChecked;
		public bool IsChecked
		{
			get
			{
				return _isChecked;
			}
			set
			{
				_isChecked = value;
				OnPropertyChanged("IsChecked");
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