using System.Collections.ObjectModel;
using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class BindableFormFieldsView : FormFieldsView , INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ObservableCollection<SelectionListView> _generalSelectList = new ();
        public ObservableCollection<SelectionListView> GeneralSelectList
        {
            get
            {
                return _generalSelectList;
            }
            set
            {
                _generalSelectList = value;
                OnPropertyChanged(nameof(GeneralSelectList));
            }
        }
        private ObservableCollection<SelectionListView> _masterList = new ();
        public ObservableCollection<SelectionListView> MasterList 
        {
            get
            {
                return _masterList;
            }
            set
            {
                _masterList = value;
                OnPropertyChanged(nameof(MasterList));
            }
        }
        private SelectionListView _selectedItem;
        public SelectionListView SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        private int _listViewHeightRequest;
        public int ListViewHeightRequest
        {
            get
            {
                return _listViewHeightRequest;
            }
            set
            {
                _listViewHeightRequest = value;
                OnPropertyChanged(nameof(ListViewHeightRequest));
            }
        }
        private bool _listViewVisible = false;
        public bool ListViewVisible
        {
            get
            {
                return _listViewVisible;
            }
            set
            {
                _listViewVisible = value;
                OnPropertyChanged(nameof(ListViewVisible));
            }
        }
        private bool _isVisible = true;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
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
        private WebFormImageView _imageData;
        public WebFormImageView ImageData
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
        public BindableFormFieldsView()
		{
		}
	}