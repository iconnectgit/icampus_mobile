using System.Windows.Input;
using Android.Net.Wifi.Aware;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Conduct;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Conduct;

public class ConductForm : ViewModelBase
    {
        #region Declarations
        public ICommand FilterClickCommand { get; set; }
        public ICommand SearchClickCommand { get; set; }
        public ICommand ListTappedCommand { get; set; }
        #endregion

        #region Properties
        StudentConductViewModel _studentConductData = new StudentConductViewModel();
        public StudentConductViewModel StudentConductData
        {
            get => _studentConductData;
            set
            {
                _studentConductData = value;
                OnPropertyChanged(nameof(StudentConductData));
            }
        }
        StudentConductViewModel _finalStudentConductData = new StudentConductViewModel();
        public StudentConductViewModel FinalStudentConductData
        {
            get => _finalStudentConductData;
            set
            {
                _finalStudentConductData = value;
                OnPropertyChanged(nameof(FinalStudentConductData));
            }
        }
        DateTime _fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }

        DateTime _toDate = DateTime.Now;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }

        bool _hasConductData;
        public bool HasConductData
        {
            get => _hasConductData;
            set
            {
                _hasConductData = value;
                OnPropertyChanged(nameof(HasConductData));
            }
        }

        string _conductType = string.Empty;
        public string ConductType
        {
            get => _conductType;
            set
            {
                _conductType = value;
                OnPropertyChanged(nameof(ConductType));
            }
        }

        ExtPickListItem _selectedConductType = new ExtPickListItem();
        public ExtPickListItem SelectedConductType
        {
            get => _selectedConductType;
            set
            {
                _selectedConductType = value;
                OnPropertyChanged(nameof(SelectedConductType));
                if (!string.IsNullOrEmpty(this.SelectedConductType.ItemName))
                    IsConductTypeErrVisible = false;
            }
        }

        IList<ExtPickListItem> _conductTypeList = new List<ExtPickListItem>();
        public IList<ExtPickListItem> ConductTypeList
        {
            get => _conductTypeList;
            set
            {
                _conductTypeList = value;
                OnPropertyChanged(nameof(ConductTypeList));
            }
        }
        private string _conductPageTitle;
        public string ConductPageTitle
        {
            get => _conductPageTitle;
            set
            {
                _conductPageTitle = value;
                OnPropertyChanged(nameof(ConductPageTitle));
            }
        }

        StudentConductView _selectedConduct = new StudentConductView();
        public StudentConductView SelectedConduct
        {
            get => _selectedConduct;
            set
            {
                _selectedConduct = value;
                OnPropertyChanged(nameof(SelectedConduct));
            }
        }

        bool _noDataExist;
        public bool NoDataExist
        {
            get => _noDataExist;
            set
            {
                _noDataExist = value;
                OnPropertyChanged(nameof(NoDataExist));
            }
        }

        int _screenWidth;
        public int ScreenWidth
        {
            get => _screenWidth;
            set
            {
                _screenWidth = value;
                OnPropertyChanged(nameof(ScreenWidth));
            }
        }

        int _collectionViewSpanCount = 1 ;
        public int CollectionViewSpanCount
        {
            get => _collectionViewSpanCount;
            set
            {
                _collectionViewSpanCount = value;
                OnPropertyChanged(nameof(CollectionViewSpanCount));
            }
        }

        bool _isConductTypeErrVisible;
        public bool IsConductTypeErrVisible
        {
            get => _isConductTypeErrVisible;
            set
            {
                _isConductTypeErrVisible = value;
                OnPropertyChanged(nameof(IsConductTypeErrVisible));
            }
        }
        bool _isNoRecordMsg = false;
        public bool IsNoRecordMsg
        {
            get => _isNoRecordMsg;
            set
            {
                _isNoRecordMsg = value;
                OnPropertyChanged(nameof(IsNoRecordMsg));
            }
        }
        bool _isNoRecordMsgSearch = false;
        public bool IsNoRecordMsgSearch
        {
            get => _isNoRecordMsgSearch;
            set
            {
                _isNoRecordMsgSearch = value;
                OnPropertyChanged(nameof(IsNoRecordMsgSearch));
            }
        }
        #endregion

        public ConductForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Methods
        private async void InitializePage()
        {
            FilterClickCommand = new Command(FilterClicked);
            SearchClickCommand = new Command(SearchClicked);
            ListTappedCommand = new Command<StudentConductView>(ListViewTapped);
        }
        public override async void GetStudentData()
        {
            base.GetStudentData();
            await GetStudentConductData();
        }
        private async Task<StudentConductViewModel> GetStudentConductData()
        {
            try
            {
                string cacheKeyPrefix = "studentconduct";
                bool loadFilterPanelLists = !ConductTypeList.Any();
                var apiUrl = string.Format(TextResource.StudentConductDataApiUrl, "", "", "", AppSettings.Current.SelectedStudent.ItemId, loadFilterPanelLists);
                StudentConductData = await ApiHelper.GetObject<StudentConductViewModel>(apiUrl, attachStudentIdIfParent: true, cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                NoDataExist = !StudentConductData.StudentConducts.Any();
                CollectionViewSpanCount = StudentConductData.ConductSummaries.Count();
                foreach (var item in StudentConductData.ConductSummaries)
                {
                    if (item.Summary != null && item.Summary.ToLower().Contains("demerits"))
                    {
                        item.Summary = "Demerits";
                    }
                    else if (item.Summary != null && item.Summary.ToLower().Contains("merits") && !item.Summary.ToLower().Contains("demerits"))
                    {
                        item.Summary = "Merits";
                    }
                }
                FinalStudentConductData = StudentConductData;
                IsNoRecordMsg = FinalStudentConductData?.StudentConducts?.Count() > 0 ? false : true;

                if (loadFilterPanelLists)
                    ConductTypeList = StudentConductData.ConductTypeList;
                return StudentConductData;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                return new StudentConductViewModel();
            }
        }

        private async void FilterClicked(object obj)
        {
            try
            {
                ConductSearchForm conductSearchForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = TextResource.FilterConductTitle,
                    MenuVisible = false,
                    BackVisible = true,
                    ConductTypeList = ConductTypeList
                };
                ConductSearchPage conductSearchPage = new ConductSearchPage()
                {
                    BindingContext = conductSearchForm
                };
                await Navigation.PushAsync(conductSearchPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void SearchClicked(object obj)
        {
            try
            {
                if (ValidateData())
                {
                    ConductType = SelectedConductType.ItemId;
                    PageTitle = TextResource.FilterConductTitle;
                    MenuVisible = false;
                    BackVisible = true;
                    IsPopUpPage = true;

                    var filteredList = StudentConductData.StudentConducts
                        .Where(conduct => conduct.FormattedIncidentDate.ToDateTime().Date >= FromDate.Date && conduct.FormattedIncidentDate.ToDateTime().Date <= ToDate.Date && conduct.ConductTypeCode == SelectedConductType.ItemId)
                        .ToList();

                    FinalStudentConductData = new StudentConductViewModel
                    {
                        StudentConducts = filteredList,
                        ConductSummaries = StudentConductData.ConductSummaries,
                        ConductTypeList = StudentConductData.ConductTypeList
                    };
                    IsNoRecordMsg = FinalStudentConductData?.StudentConducts?.Count() > 0 ? false : true;

                    int totalPoints = 0;

                    if (FinalStudentConductData.StudentConducts.Count() > 0)
                    {
                        foreach (var item in FinalStudentConductData.StudentConducts)
                        {
                            totalPoints += item.ConductTypeCode.Equals("D") ? Math.Abs(item.Points) * -1 : Math.Abs(item.Points);
                        }
                        FinalStudentConductData.StudentConducts.First().Points = Math.Abs(totalPoints);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
            }
        }

        public override void BackClicked(object obj)
        {
            base.BackClicked(obj);
            this.PageTitle = this.ConductPageTitle;
            this.MenuVisible = true;
            this.BackVisible = false;
        }


        private async void ListViewTapped(StudentConductView obj)
        {
            if (obj != null)
            {
                ConductDetailForm conductDetailForm = new (_mapper, _nativeServices, Navigation)
                {
                    SelectedConduct = obj,
                    PageTitle = this.PageTitle
                };
                ConductDetailPage conductDetailPage = new ConductDetailPage()
                {
                    BindingContext = conductDetailForm
                };
                await Navigation.PushAsync(conductDetailPage);
                SelectedConduct = null;
            }
        }
        private bool ValidateData()
        {
            IsConductTypeErrVisible = SelectedConductType != null && SelectedConductType.ItemName != null ? false : true;
            return !IsConductTypeErrVisible;
        }
        #endregion
    }