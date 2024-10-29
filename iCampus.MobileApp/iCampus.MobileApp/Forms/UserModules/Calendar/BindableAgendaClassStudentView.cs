using System.ComponentModel;
using AutoMapper;
using iCampus.Portal.ViewModels;
using Splat;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableAgendaClassStudentView : INotifyPropertyChanged
    {
	    private IMapper _mapper;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public BindableAgendaClassStudentView()
        {
	        _mapper = Locator.Current.GetService<IMapper>();
        }
		public int AgendaId
		{
			get;
			set;
		}

		public Guid AgendaUId
		{
			get;
			set;
		}

		public int StudentId
		{
			get;
			set;
		}

		public string StudentName
		{
			get;
			set;
		}

		public short ClassId
		{
			get;
			set;
		}

		public string ClassName
		{
			get;
			set;
		}

		public bool IsElective
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public string SubmissionDate
		{
			get;
			set;
		}

		public string AttachmentFile
		{
			get;
			set;
		}

		public string AlternateId
		{
			get;
			set;
		}

		public string CurriculumName
		{
			get;
			set;
		}

		public string AgendaTypeTitle
		{
			get;
			set;
		}

		public string DueDateFormatted
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public string LastSubmissionDate
		{
			get;
			set;
		}

		public short AgendaTypeId
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

		public string DeletedAttachmentFiles
		{
			get;
			set;
		}

		public string StudentComments
		{
			get;
			set;
		}

		public bool IsStudentSubmissionAllowed
		{
			get;
			set;
		}

        List<StudentSubmissionAttachmentView> _studentSubmittedFilesList;
        public List<StudentSubmissionAttachmentView> StudentSubmittedFilesList
		{
            get
            {
				return _studentSubmittedFilesList;
            }
            set
            {
                _studentSubmittedFilesList = value;
                if (_studentSubmittedFilesList != null)
                    BindableStudentSubmittedFilesList = _mapper.Map<IList<BindableAttachmentFileView>>(_studentSubmittedFilesList);
                OnPropertyChanged("StudentSubmittedFilesList");

            }
		}

        IList<BindableAttachmentFileView> _bindableStudentSubmittedFilesList;
        public IList<BindableAttachmentFileView> BindableStudentSubmittedFilesList
        {
            get
            {
                return _bindableStudentSubmittedFilesList;
            }
            set
            {
                _bindableStudentSubmittedFilesList = value;
                OnPropertyChanged("BindableStudentSubmittedFilesList");
            }
        }

        bool _isAttachmentsVisible;
        public bool IsAttachmentsVisible
        {
            get
            {
				_isAttachmentsVisible = (BindableStudentSubmittedFilesList.Count > 2) || (BindableStudentSubmittedFilesList.Count == 1 && BindableStudentSubmittedFilesList.FirstOrDefault().FilePath != null);
                return _isAttachmentsVisible;
            }
            set
            {
                _isAttachmentsVisible = value;
                OnPropertyChanged("IsAttachmentsVisible");
            }
        }
        

        int _attachmentListViewHeight;
        public int AttachmentListViewHeight
        {
            get
            {
				_attachmentListViewHeight = BindableStudentSubmittedFilesList.Count * 50;
                return _attachmentListViewHeight;
            }
            set
            {
                _attachmentListViewHeight = value;
                OnPropertyChanged("AttachmentListViewHeight");
            }
        }

        public string StudentZipFileName => (AgendaTypeId == 3) ? $"{AlternateId}-{StudentName}-{AgendaTypeTitle}-{DueDateFormatted}.zip" : $"{AlternateId}-{StudentName}-{AgendaTypeTitle}-{CurriculumName}-{DueDateFormatted}.zip";
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
    }