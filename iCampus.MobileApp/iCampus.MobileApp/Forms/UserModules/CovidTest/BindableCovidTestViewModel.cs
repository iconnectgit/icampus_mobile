using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.CovidTest;

[Serializable]
    public class BindableCovidTestViewModel : INotifyPropertyChanged
    {
        public BindableCovidTestViewModel()
        {
            CovidTestList = new List<CovidTestView>();
            PersonTypes = new List<ExtPickListItem>();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public IList<CovidTestView> CovidTestList { get; set; }
        public IEnumerable<ExtPickListItem> PersonTypes { get; set; }
        public FamilyInformation FamilyInformation { get; set; }

    }

    [Serializable]
    public class CovidTestView  : INotifyPropertyChanged
    {
        public int CovidTestId { get; set; }
        public short UserTypeId { get; set; }
        public int UserRefId { get; set; }
        public string PersonType { get; set; }
        public string PersonName { get; set; }
        public string UserTypeName { get; set; }
        public int? TestedChild { get; set; }
        public bool TestStatus { get; set; }
        public string TestLocation { get; set; }
        public string DocumentName { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime TestDate { get; set; }
        public string FormattedUploadedDate { get; set; }
        public string FormattedTestDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsAddMode { get; set; }
        public short TransMode { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string AttachmentFile { get; set; }
        public AttachmentFileView AttachmentView { get; set; }
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged == null)
        //        return;

        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
        //string _formattedTestStatus;
        //public string FormattedTestStatus
        //{
        //    get {
        //        _formattedTestStatus = TestStatus ? "Positive" : "Negative";
        //        return _formattedTestStatus;
        //    }
        //    set
        //    {
        //        _formattedTestStatus = value;
        //        OnPropertyChanged("FormattedTestStatus");
        //    }
        //}
        //string _loggedInUserName;
        //public string LoggedInUserName
        //{
        //    get
        //    {
        //        return _loggedInUserName;
        //    }
        //    set
        //    {
        //        _loggedInUserName = value;
        //        OnPropertyChanged("LoggedInUserName");
        //    }
        //}

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

        public string FormattedTestStatus { get; set; }
        public string LoggedInUserName { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
    public class FamilyInformation
    {
        public int CovidTestId { get; set; }
        public int UserTypeId { get; set; }
        public int UserRefId { get; set; }
        public object PersonType { get; set; }
        public object PersonName { get; set; }
        public object UserTypeName { get; set; }
        public object TestedChild { get; set; }
        public bool TestStatus { get; set; }
        public object TestLocation { get; set; }
        public object DocumentName { get; set; }
        public object AttachmentPath { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime TestDate { get; set; }
        public object FormattedUploadedDate { get; set; }
        public object FormattedTestDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsAddMode { get; set; }
        public int TransMode { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public object AttachmentFile { get; set; }
        public object AttachmentView { get; set; }
    }