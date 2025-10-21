using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Views.PopUpViews;

namespace iCampus.MobileApp.Forms.PopupForms;

public class BirthdayPopupForm : ViewModelBase
    {
        public ICommand OkClickCommand { get; set; }
        #region Properties

        string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        string _studentName;
        public string StudentName
        {
            get => _studentName;
            set
            {
                _studentName = value;
                OnPropertyChanged(nameof(StudentName));
            }
        }
        string _itemId;
        public string ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }
        #endregion
        public BirthdayPopupForm() : base(null, null, null)
		{
            OkClickCommand = new Command(OkClickedMethod);
        }

        private async void OkClickedMethod(object obj)
        {
            var item = obj as BirthdayPopupForm;
            AppSettings.Current.BirthdayPopup.Close();
            var currentStudent = AppSettings.Current.StudentList.FirstOrDefault(x => x.ItemId == item.ItemId);
            if(currentStudent != null)
            {
                var indexOfStudent = AppSettings.Current.StudentList.IndexOf(currentStudent);
                if ((indexOfStudent + 1) < AppSettings.Current.StudentList.Count)
                {
                    var nextStudent = AppSettings.Current.StudentList[indexOfStudent + 1];
                    if(!string.IsNullOrEmpty(nextStudent.BirthdayNotficationMessage))
                    {
                        BirthdayPopupForm birthdayPopupForm = new ()
                        {
                            Message = nextStudent.BirthdayNotficationMessage,
                            StudentName = nextStudent.StudentName,
                            ItemId = nextStudent.ItemId,
                            Title = TextResource.BirthdayPopupTitle
                        };
                        BirthdayPopup birthdayPopup = new ()
                        {
                            BindingContext = birthdayPopupForm
                        };
                        SetPopupInstance(birthdayPopup);
                        Application.Current.MainPage.ShowPopup(birthdayPopup);   
                    }
                }
            }
            
        }
    }