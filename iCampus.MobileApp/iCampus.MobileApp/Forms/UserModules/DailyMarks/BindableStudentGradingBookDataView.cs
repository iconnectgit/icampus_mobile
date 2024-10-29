using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.DailyMarks;

public class BindableStudentGradingBookDataView : INotifyPropertyChanged
    {
        public short CurriculumId { get; set; }
        public int ElementId { get; set; }
        public short TermId { get; set; }
        public int ParentElementId { get; set; }
        public string CurriculumName { get; set; }
        public string ElementName { get; set; }
        public short LevelNo { get; set; }
        public int HeaderRowNum { get; set; }
        public bool EnableResultsEntry { get; set; }
        public bool HasAverageColumn { get; set; }
        public bool IsFixedColumn { get; set; }
        public string ElementPropertiesJson { get; set; }

        private ResultJsonModel _resultModel =new ResultJsonModel();
        public ResultJsonModel ResultModel
        {
            get { return _resultModel; }
            set
            {
                _resultModel = value;
                OnPropertyChanged("ResultModel");
            }
        }

        private EffortsResultJsonModel _effortsResultModel = new EffortsResultJsonModel();
        public EffortsResultJsonModel EffortsResultModel
        {
            get { return _effortsResultModel; }
            set
            {
                _effortsResultModel = value;
                OnPropertyChanged("EffortsResultModel");
            }
        }
        private ElementPropertiesJsonModel _elementPropertiesModel = new ElementPropertiesJsonModel();
        public ElementPropertiesJsonModel ElementPropertiesModel
        {
            get { return _elementPropertiesModel; }
            set
            {
                _elementPropertiesModel = value;
                OnPropertyChanged("ElementPropertiesModel");
            }
        }
        private MarksEntryStructureJsonModel _marksEntryModel = new MarksEntryStructureJsonModel();
        public MarksEntryStructureJsonModel MarksEntryModel
        {
            get { return _marksEntryModel; }
            set
            {
                _marksEntryModel = value;
                OnPropertyChanged("MarksEntryModel");
            }
        }
        public double PassingMark { get; set; }
        public string MarksEntryStructureJson { get; set; }
        public string EffortsEntryStructureJson { get; set; }
        public string MarkType { get; set; }
        public string ResultsJson { get; set; }
        public string PreAverageResultsJson { get; set; }
        public string EffortResultsJson { get; set; }
        public short EvalResultId { get; set; }
        public string EvaluationResultText { get; set; }
        public bool HasCourseComments { get; set; }

        private bool _hasChild;
        public bool HasChild
        {
            get { return ParentElementId==CurriculumId; }
            set
            {
                _hasChild = value;
                OnPropertyChanged("HasChild");
            }
        }

        private bool _isExpandMode;
        public bool IsExpandMode
        {
            get { return _isExpandMode; }
            set
            {
                _isExpandMode = value;
                ButtonText = value ? "+" : "-";
                OnPropertyChanged("IsExpandMode");
            }
        }

        private string _buttonText="-";
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ResultJsonModel : INotifyPropertyChanged
    {
        public double? Result { get; set; }
        public string GroupCode { get; set; }
        public string Letter { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class EffortsResultJsonModel : INotifyPropertyChanged
    {
        public double? Result { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MarksEntryStructureJsonModel : INotifyPropertyChanged
    {
        public bool UseTextBox { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class ElementPropertiesJsonModel : INotifyPropertyChanged
    {
        public double? PassingMark { get; set; }
        public int GradingSchemaSettingId { get; set; }
        public string DataType { get; set; }
        public double LowerBound { get; set; }
        public double HigherBound { get; set; }
        public double CustomLowerBound { get; set; }
        public double CustomHigherBound { get; set; }
       
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }