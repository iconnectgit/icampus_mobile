using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Survey;

public class BindableSurveyQuestionAnswerView : INotifyPropertyChanged
    {

        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public bool IsRequired { get; set; }
        public int SurveyQuestionId { get; set; }
        public int? ParentQuestionId { get; set; }
        public bool IsDecisionMaker { get; set; }
        public bool IsRequiredQuestion { get; set; }
        public bool IsLinkedQuestion { get; set; }
        public int QuestionGroupId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionSequence { get; set; }
        public short QuestionTypeId { get; set; }
        
        public int SurveyChoiceAnswerId { get; set; }
        public int? ParentSurveyChoiceAnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool HasLinkedQuestions { get; set; }
        public ObservableCollection<BindableSurveyAnswerView> SurveyAnswerList { get; set; }
        public BindableSurveyAnswerView SelecedSurveyAnswer { get; set; }

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class BindableSurveyAnswerView : INotifyPropertyChanged
    {
        public int SurveyChoiceAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }

        private string _answerText;
        public string AnswerText
        {
            get { return _answerText; }
            set
            {
                _answerText = value;
                OnPropertyChanged("AnswerText");
            }
        }
        public bool IsSelected { get; set; }
    

        public short SurveyQuestionTypeId { get; set; }
        public bool HasLinkedQuestions { get; set; }
        public ObservableCollection<BindableSurveyQuestionAnswerView> LinkedQuestions { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }