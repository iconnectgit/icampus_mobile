using System.Collections.ObjectModel;
using System.ComponentModel;
using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class BindableQuickPost : INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            //PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public BindableQuickPost()
		{
		}
        public Guid? AgendaUId { get; set; }
        public string DueDate { get; set; }
        public int AgendaId { get; set; }

        private string _agendaWeekDatesFormatted;
        public string AgendaWeekDatesFormatted
        {
            get
            {
                return _agendaWeekDatesFormatted;
            }
            set
            {
                _agendaWeekDatesFormatted = value;
                OnPropertyChanged("AgendaWeekDatesFormatted");
            }
        }

        private bool _isDeleted = false;
        public bool IsDeleted
        {
            get
            {
                return _isDeleted;
            }
            set
            {
                _isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }
        private string _learningOutcomesLabel;
        public string LearningOutcomesLabel
        {
            get
            {
                return _learningOutcomesLabel;
            }
            set
            {
                _learningOutcomesLabel = value;
                OnPropertyChanged("LearningOutcomesLabel");
            }
        }

        private string _assignmentsLabel;
        public string AssignmentsLabel
        {
            get
            {
                return _assignmentsLabel;
            }
            set
            {
                _assignmentsLabel = value;
                OnPropertyChanged("AssignmentsLabel");
            }
        }
        private string _agendaDescription;
        public string AgendaDescription
        {
            get
            {
                return _agendaDescription;
            }
            set
            {
                _agendaDescription =
                    "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>"
                    + value
                    + "</body></html>";
                OnPropertyChanged("AgendaDescription");
            }
        }
        private string _learningOutcomes;
        public string LearningOutcomes
        {
            get
            {
                
                return _learningOutcomes;
            }
            set
            {
                _learningOutcomes =
                    "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>"
                    + value
                    + "</body></html>";
                OnPropertyChanged("LearningOutcomes");
            }
        }
        ObservableCollection<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
        public ObservableCollection<AttachmentFileView> AttachmentFiles
        {
            get
            {
                return _attachmentFiles;
            }
            set
            {
                _attachmentFiles = value;
                OnPropertyChanged("AttachmentFiles");
                AttachmentListViewHeight = AttachmentFiles.Count * 40;
            }
        }
        int _attachmentListViewHeight;
        public int AttachmentListViewHeight
        {
            get
            {
                return _attachmentListViewHeight;
            }
            set
            {
                _attachmentListViewHeight = value;
                OnPropertyChanged("AttachmentListViewHeight");
            }
        }
        List<int> _deletedAttachmentFileID = new List<int>();
        public List<int> DeletedAttachmentFileID
        {
            get
            {
                return _deletedAttachmentFileID;
            }
            set
            {
                _deletedAttachmentFileID = value;
                OnPropertyChanged("DeletedAttachmentFileID");
            }
        }

        List<string> _deletedAttachmentFileName = new List<string>();
        public List<string> DeletedAttachmentFileName
        {
            get
            {
                return _deletedAttachmentFileName;
            }
            set
            {
                _deletedAttachmentFileName = value;
                OnPropertyChanged("DeletedAttachmentFileName");
            }
        }
        List<AgendaView> _existingAttachmentList = new List<AgendaView>();
        public List<AgendaView> ExistingAttachmentList
        {
            get
            {
                return _existingAttachmentList;
            }
            set
            {
                _existingAttachmentList = value;
                OnPropertyChanged("ExistingAttachmentList");
            }
        }
    }