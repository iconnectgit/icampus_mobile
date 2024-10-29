using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.DataCollection;

public class BindableDataCollectionFieldsView : INotifyPropertyChanged
    {

        public int DataCollectionFieldId { get; set; }
        public string FieldTitle { get; set; }
        public int FieldTypeId { get; set; }
        public string FieldOption { get; set; }
        public int Sequence { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int? StudentId { get; set; }
        //public string FieldValue { get; set; }


        private bool _isTrue;
        public bool IsTrue
        {
            get { return _isTrue; }
            set
            {
                _isTrue = value;
                OnPropertyChanged("IsTrue");
            }
        }

        private bool _isFalse;
        public bool IsFalse
        {
            get { return _isFalse; }
            set
            {
                _isFalse = value;
                OnPropertyChanged("IsFalse");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private string _fieldValue;
        public string FieldValue
        {
            get { return _fieldValue; }
            set
            {
                _fieldValue = value;
                OnPropertyChanged("FieldValue");
            }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        private List<string> _selectAnswerList;
        public List<string> SelectAnswerList
        {
            get { return _selectAnswerList; }
            set
            {
                _selectAnswerList = value;
                OnPropertyChanged("SelectAnswerList");
            }
        }

        private int _attachmentListHeight;
        public int AttachmentListHeight
        {
            get {
                _attachmentListHeight = AttachmentList.Count() * 50;
                return _attachmentListHeight;
            }
            set
            {
                _attachmentListHeight = value;
                OnPropertyChanged("AttachmentListHeight");
            }
        }

        public int AttachmentCount
        {
            get;
            set;
        }

        private ObservableCollection<BindableAttachmentFileView> _attachmentList;
        public ObservableCollection<BindableAttachmentFileView> AttachmentList
        {
            get
            {
                return _attachmentList;
            }
            set {
                _attachmentList = value;
                AttachmentListHeight = _attachmentList.Count() * 50;
                OnPropertyChanged("AttachmentList");
            }
        }

        //public string AnswerText { get; set; }

        public string SelecedDataAnswer { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }