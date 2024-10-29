using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.TeacherEvaluation;

public class BindableTeacherFeedBackView : INotifyPropertyChanged
	{
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public int CurriculumId { get; set; }

        public string TeacherName { get; set; }

        public int FeedBackId { get; set; }

        public string FeedBackName { get; set; }

        public string TeacherCourse { get; set; }

        public int GenderId { get; set; }

        public int StudentId { get; set; }

        private string _starImageSource1 = "gray_star.png";
        public string StarImageSource1
        {
            get
            {
                return _starImageSource1;
            }
            set
            {
                _starImageSource1 = value;
                OnPropertyChanged("StarImageSource1");
            }
        }
        private string _starImageSource2 = "gray_star.png";
        public string StarImageSource2
        {
            get
            {
                return _starImageSource2;
            }
            set
            {
                _starImageSource2 = value;
                OnPropertyChanged("StarImageSource2");
            }
        }
        private string _starImageSource3 = "gray_star.png";
        public string StarImageSource3
        {
            get
            {
                return _starImageSource3;
            }
            set
            {
                _starImageSource3 = value;
                OnPropertyChanged("StarImageSource3");
            }
        }
        private string _starImageSource4 = "gray_star.png";
        public string StarImageSource4
        {
            get
            {
                return _starImageSource4;
            }
            set
            {
                _starImageSource4 = value;
                OnPropertyChanged("StarImageSource4");
            }
        }
        private string _starImageSource5 = "gray_star.png";
        public string StarImageSource5
        {
            get
            {
                return _starImageSource5;
            }
            set
            {
                _starImageSource5 = value;
                OnPropertyChanged("StarImageSource5");
            }
        }
        bool _submitButtonVisibility;
        public bool SubmitButtonVisibility
        {
            get
            {
                return _submitButtonVisibility;
            }
            set
            {
                _submitButtonVisibility = value;
                OnPropertyChanged("SubmitButtonVisibility");
            }
        }
        string _feedBackText;
        public string FeedBackText
        {
            get
            {
                return _feedBackText;
            }
            set
            {
                _feedBackText = value;
                OnPropertyChanged("FeedBackText");
            }
        }

        public IEnumerable<BindableFeedbackStatusView> FeedBackList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public BindableTeacherFeedBackView()
		{

		}
	}
    public class BindableFeedbackStatusView
    {
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string ImageName { get; set; }
    }