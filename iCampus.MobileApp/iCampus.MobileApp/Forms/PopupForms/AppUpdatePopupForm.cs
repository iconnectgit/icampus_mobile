using System.Windows.Input;

namespace iCampus.MobileApp.Forms.PopupForms;

public class AppUpdatePopupForm:ViewModelBase
    {
        #region Declarations
        public ICommand UpdateNowClickCommand { get; set; }
        public ICommand UpdateLaterClickCommand { get; set; }
        #endregion
        #region Properties
        bool _isForceUpdate;
        public bool IsForceUpdate
        {
            get => _isForceUpdate;
            set
            {
                _isForceUpdate = value;
                OnPropertyChanged(nameof(IsForceUpdate));
            }
        }

        string _updateTitle;
        public string UpdateTitle
        {
            get => _updateTitle;
            set
            {
                _updateTitle = value;
                OnPropertyChanged(nameof(UpdateTitle));
            }
        }

        string _updateMessage;
        public string UpdateMessage
        {
            get => _updateMessage;
            set
            {
                _updateMessage = value;
                OnPropertyChanged(nameof(UpdateMessage));
            }
        }

        #endregion
        public AppUpdatePopupForm() : base(null, null, null)
        {
            this.UpdateNowClickCommand = new Command(UpdateNowClicked);
            this.UpdateLaterClickCommand = new Command(UpdateLaterClicked);
        }
        #region Methods
        private async void UpdateNowClicked(object obj)
        {
            string storeURL = string.Empty;
            string clientGroupCode = (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
            if (App.ClientCode?.ToLower() == "gipa")
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/us/app/id1526415373";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.Gipa.GipaMobile";
            }
            else if(App.ClientCode?.ToLower() == "madar")
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/in/app/madar-schools-portal/id1568036763";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.madar.MadarMobile";
            }
            else if (App.ClientCode?.ToLower() == "cs")
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/in/app/cs-schools-portal/id1569548832";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.cs.CsMobile";
            }
            else if (App.ClientCode?.ToLower() == "cas")
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/in/app/cas-schools-portal/id1569548187";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.CAS.CASMobile";
            }
            else if (clientGroupCode.Equals("Beam"))
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/in/app/beam-schools-portal/id1581764434";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.beam.BeamSchoolsPortal";
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                    storeURL = "https://apps.apple.com/gb/app/icampusapp/id1495580840";
                else
                    storeURL = "https://play.google.com/store/apps/details?id=com.icampus.iCampusMobile";
            }
            await Launcher.Default.OpenAsync(new Uri(storeURL));
            AppSettings.Current.CurrentPopup?.Close();
        }
        private async void UpdateLaterClicked(object obj)
        {
            // Func<PopupPage, bool> predicate = x => x.GetType() == typeof(AppUpdatePopup);
            // if (PopupNavigation.Instance.PopupStack.Any(predicate))
            // {
            //     var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(predicate);
            //     await PopupNavigation.Instance.RemovePageAsync(popupPage);
            // }
        }
        #endregion
    }