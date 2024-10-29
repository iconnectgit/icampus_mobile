using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Library;

public class LibraryForm : ViewModelBase
	{
        public ICommand CurrentCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand CurrentExpandCollapseClickCommand { get; set; }
        public ICommand HistoryExpandCollapseClickCommand { get; set; }

        
        private bool _isHistory = false;
        public bool IsHistory
        {
            get => _isHistory;
            set
            {
                _isHistory = value;
                OnPropertyChanged(nameof(IsHistory));
            }
        }
        private bool _isCurrent = true;
        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                _isCurrent = value;
                OnPropertyChanged(nameof(IsCurrent));
            }
        }       
        private FontAttributes _currentFontType = FontAttributes.Bold;
        public FontAttributes CurrentFontType
        {
            get => _currentFontType;
            set
            {
                _currentFontType = value;
                OnPropertyChanged(nameof(CurrentFontType));
            }
        }
        private FontAttributes _historyFontType = FontAttributes.None;
        public FontAttributes HistoryFontType
        {
            get => _historyFontType;
            set
            {
                _historyFontType = value;
                OnPropertyChanged(nameof(HistoryFontType));
            }
        }
        
        IList<BindableLibraryView> _libraryList;
        public IList<BindableLibraryView> LibraryList
        {
            get
            {
                return _libraryList;
            }
            set
            {
                _libraryList = value;
                OnPropertyChanged(nameof(LibraryList));
            }
        }
        IList<BindableLibraryView> _selectedLibraryList;
        public IList<BindableLibraryView> SelectedLibraryList
        {
            get
            {
                return _selectedLibraryList;
            }
            set
            {
                _selectedLibraryList = value;
                OnPropertyChanged(nameof(SelectedLibraryList));
            }
        }
        IList<BindableLibraryView> _libraryHistoryList;
        public IList<BindableLibraryView> LibraryHistoryList
        {
            get
            {
                return _libraryHistoryList;
            }
            set
            {
                _libraryHistoryList = value;
                OnPropertyChanged(nameof(LibraryHistoryList));
            }
        }
        IList<BindableLibraryView> _selectedHistoryLibraryList;
        public IList<BindableLibraryView> SelectedHistoryLibraryList
        {
            get
            {
                return _selectedHistoryLibraryList;
            }
            set
            {
                _selectedHistoryLibraryList = value;
                OnPropertyChanged(nameof(SelectedHistoryLibraryList));
            }
        }
        bool _isNoRecordLibrary = false;
        public bool IsNoRecordLibrary
        {
            get => _isNoRecordLibrary;
            set
            {
                _isNoRecordLibrary = value;
                OnPropertyChanged(nameof(IsNoRecordLibrary));
            }
        }
        bool _isNoRecordHistory = false;
        public bool IsNoRecordHistory
        {
            get => _isNoRecordHistory;
            set
            {
                _isNoRecordHistory = value;
                OnPropertyChanged(nameof(IsNoRecordHistory));
            }
        }
        private bool _currentShadow = true;
        public bool CurrentShadow
        {
            get => _currentShadow;
            set
            {
                _currentShadow = value;
                OnPropertyChanged(nameof(CurrentShadow));
            }
        }
        private bool _historyShadow = false;
        public bool HistoryShadow
        {
            get => _historyShadow;
            set
            {
                _historyShadow = value;
                OnPropertyChanged(nameof(HistoryShadow));
            }
        }
        public LibraryForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
		{
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private async void InitializePage()
        {
            CurrentCommand = new Command(CurrentCommandClicked);
            HistoryCommand = new Command(HistoryCommandClicked);
            CurrentExpandCollapseClickCommand = new Command<BindableLibraryView>(CurrentExpandCollapseClicked);
            HistoryExpandCollapseClickCommand = new Command<BindableLibraryView>(HistoryExpandCollapseClicked);
        }
        private async Task GetDetails()
        {
            try
            {
                var data = await ApiHelper.GetObject<List<BindableLibraryView>>(string.Format(TextResource.GetGetLibraryDataUrl, AppSettings.Current.SelectedStudent.ItemId),cacheKeyPrefix: "library", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                var libraryList = data.Where(m => m.GroupName.Equals("Issues", StringComparison.OrdinalIgnoreCase)|| m.GroupName.Equals("Overdue", StringComparison.OrdinalIgnoreCase)).ToList();
                LibraryList = libraryList;
                foreach (var item in LibraryList)
                {
                    if(item.GroupName.Equals("Overdue", StringComparison.OrdinalIgnoreCase))
                    {
                        item.GroupName = item.GroupName.Replace("Overdue", "Over due");
                        item.IsOverDue = true;
                    }
                }
                var historyData = data.Where(m => m.GroupName.Equals("History", StringComparison.OrdinalIgnoreCase));
                LibraryHistoryList = _mapper.Map<IList<BindableLibraryView>>(historyData);

                IsNoRecordLibrary = LibraryList.Count == 0;
                IsNoRecordHistory = LibraryHistoryList.Count == 0;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private void HistoryCommandClicked(object obj)
        {
            this.IsHistory = true;
            this.IsCurrent = false;
            this.CurrentShadow = false;
            this.HistoryShadow = true;
            this.CurrentFontType = FontAttributes.None;
            this.HistoryFontType = FontAttributes.Bold;
        }

        private void CurrentCommandClicked(object obj)
        {
            this.IsHistory = false;
            this.IsCurrent = true;
            this.CurrentShadow = true;
            this.HistoryShadow = false;
            this.CurrentFontType = FontAttributes.Bold;
            this.HistoryFontType = FontAttributes.None;
        }

        public void HistoryExpandCollapseClicked(BindableLibraryView libraryData)
        {
            try
            {
                if (libraryData != null)
                {
                    foreach (var item in LibraryHistoryList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.Title == libraryData.Title)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                            }
                            else
                            {
                                item.DetailsVisibility = false;
                                item.ArrowImageSource = "dropdown_gray.png";
                            }
                        }
                    }
                    MessagingCenter.Send("", "HistoryExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        public void CurrentExpandCollapseClicked(BindableLibraryView libraryData)
        {
            try
            {
                if (libraryData != null)
                {
                    foreach (var item in LibraryList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.Title == libraryData.Title)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                            }
                            else
                            {
                                item.DetailsVisibility = false;
                                item.ArrowImageSource = "dropdown_gray.png";
                            }
                        }
                    }
                    MessagingCenter.Send("", "CurrentExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public async override void GetStudentData()
        {
            try
            {
                await GetDetails();
                base.GetStudentData();
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
    }