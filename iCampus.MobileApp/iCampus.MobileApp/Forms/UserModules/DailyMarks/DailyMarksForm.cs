using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.DailyMarks;

namespace iCampus.MobileApp.Forms.UserModules.DailyMarks;

public class DailyMarksForm : ViewModelBase
    {
        #region Declarations
        public ICommand SearchClickCommand { get; set; }
        public ICommand ExpandClickCommand { get; set; }
        #endregion
        #region Properties
        bool _isNoRecordMsg;
        public bool IsNoRecordMsg
        {
            get => _isNoRecordMsg;
            set
            {
                _isNoRecordMsg = value;
                OnPropertyChanged(nameof(IsNoRecordMsg));
            }
        }
        bool _searchOptionVisibility=true;
        public bool SearchOptionVisibility
        {
            get => _searchOptionVisibility;
            set
            {
                _searchOptionVisibility = value;
                OnPropertyChanged(nameof(SearchOptionVisibility));
            }
        }
        bool _isEffortVisible;
        public bool IsEffortVisible
        {
            get => _isEffortVisible;
            set
            {
                _isEffortVisible = value;
                OnPropertyChanged(nameof(IsEffortVisible));
            }
        }

        BindableGradingBookView _dailyMarksData = new BindableGradingBookView();
        public BindableGradingBookView DailyMarksData
        {
            get => _dailyMarksData;
            set
            {
                _dailyMarksData = value;
                OnPropertyChanged(nameof(DailyMarksData));
            }
        }
        ObservableCollection<BindableDailyMarksModel> _dailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>();
        public ObservableCollection<BindableDailyMarksModel> DailyMarksParentChildList
        {
            get => _dailyMarksParentChildList;
            set
            {
                _dailyMarksParentChildList = value;
                OnPropertyChanged(nameof(DailyMarksParentChildList));
            }
        }
        IList<ExtPickListItem> _termList = new List<ExtPickListItem>();
        public IList<ExtPickListItem> TermList
        {
            get => _termList;
            set
            {
                _termList = value;
                OnPropertyChanged(nameof(TermList));
            }
        }

        IList<ExtPickListItem> _courseList = new List<ExtPickListItem>();
        public IList<ExtPickListItem> CourseList
        {
            get => _courseList;
            set
            {
                _courseList = value;
                OnPropertyChanged(nameof(CourseList));
            }
        }

        int _selectedTermIndex = 0;
        public int SelectedTermIndex
        {
            get => _selectedTermIndex;
            set
            {
                _selectedTermIndex = value;
                OnPropertyChanged(nameof(SelectedTermIndex));
            }
        }

        int _selectedCourseIndex = 0;
        public int SelectedCourseIndex
        {
            get => _selectedCourseIndex;
            set
            {
                _selectedCourseIndex = value;
                OnPropertyChanged(nameof(SelectedCourseIndex));
            }
        }

        bool _loadFilterPanelList;
        public bool LoadFilterPanelList
        {
            get => _loadFilterPanelList;
            set
            {
                _loadFilterPanelList = value;
                OnPropertyChanged(nameof(LoadFilterPanelList));
            }
        }

        #endregion
        public DailyMarksForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            SearchClickCommand = new Command(DailyMarksSearchClicked);
            ExpandClickCommand = new Command<BindableDailyMarksModel>(ExpandClicked);
            MessagingCenter.Subscribe<DailyMarksFilterForm>(this, "SearchDailyMarks", async (filterFormData) =>
            {
                SelectedTermIndex = filterFormData.SelectedTermIndex;
                SelectedCourseIndex = filterFormData.SelectedCourseIndex;
                TermList = filterFormData.TermList;
                CourseList = filterFormData.CourseList;
                await GetMarksListByStudent();
            });
            //MessagingCenter.Subscribe<string>("", "ListViewRightSwipeDailyMarks", async (arg) =>
            //{
            //    await SideMenuClicked();
            //    MessagingCenter.Unsubscribe<string>("", "ListViewRightSwipeDailyMarks");

            //});
            MessagingCenter.Subscribe<string>("", "ListViewRightSwipeDailyMarksSubscribe", (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "ListViewRightSwipeDailyMarks", async (argu) =>
                {
                    //await SideMenuClicked();
                });

            });
        }
        #region methods
        private async void DailyMarksSearchClicked(object obj)
        {
            DailyMarksFilterForm dailyMarksFilterForm = new (_mapper, _nativeServices, Navigation)
            {
                PageTitle = TextResource.SearchDailyMarks,
                MenuVisible = false,
                BackVisible = true,
                TermList = this.TermList,
                CourseList = this.CourseList,
                SelectedCourseIndex = SelectedCourseIndex,
                SelectedTermIndex = SelectedTermIndex
            };
            DailyMarksFilter dailyMarksFilter = new DailyMarksFilter()
            {
                BindingContext = dailyMarksFilterForm
            };
            await Navigation.PushAsync(dailyMarksFilter);
        }

        public override async void GetStudentData()
        {
            base.GetStudentData();
            UpdateFilterValues();
            await GetMarksListByStudent();
        }

        public void UpdateFilterValues()
        {
            LoadFilterPanelList = true;
            SelectedTermIndex = 0;
            SelectedCourseIndex = 0;
        }
        public async Task<BindableGradingBookView> GetMarksListByStudent()
        {
            try
            {
                await ApiHelper.ShowProcessingIndicatorPopup();
                IsNoRecordMsg = false;
                DailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>();
                bool loadFilterPanelList = !TermList.Any() || !CourseList.Any() || LoadFilterPanelList;
                string selectedTermId = TermList != null && TermList.Count > 0 && SelectedTermIndex >=0 ? TermList[SelectedTermIndex].ItemId:null;
                string selectedCourseId = CourseList!=null && CourseList.Count > 0 && SelectedCourseIndex >=0 ? CourseList[SelectedCourseIndex].ItemId:null;
                DailyMarksData = await ApiHelper.GetObject<BindableGradingBookView>(string.Format(TextResource.DailyMarksApiUrl,AppSettings.Current.SelectedStudent.ItemId,
                    loadFilterPanelList, selectedCourseId, null, selectedTermId, null,null), loadFromCacheWhenNoInternetConnection: true,isLoader:false);
                if(DailyMarksData!=null&&DailyMarksData.Permissions!=null)
                {
                    if(!String.IsNullOrEmpty(DailyMarksData.Permissions.NoModuleAccessMessage))
                    {
                        IsNoRecordMsg = true;
                        SearchOptionVisibility = false;
                        NoDataFound = DailyMarksData.Permissions.NoModuleAccessMessage;
                        await ApiHelper.HideProcessingIndicatorPopup();
                        return new BindableGradingBookView();
                    }
                }
                if (loadFilterPanelList)
                {
                    TermList = DailyMarksData?.TermList;
                    CourseList = DailyMarksData?.StudentCourses;
                }
                if (DailyMarksData != null && DailyMarksData.GradingBookDataPerStudent!=null && DailyMarksData.GradingBookDataPerStudent.Count > 0)
                {
                    var bindableData = _mapper.Map<IList<BindableStudentGradingBookDataView>>(DailyMarksData.GradingBookDataPerStudent);
                    if (bindableData !=null && bindableData.Count > 0) 
                    {
                        if(this.CourseList != null && this.CourseList.Count > 0 && this.CourseList[SelectedCourseIndex].ItemId != "0")
                        {
                            DailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>(bindableData
                                .Where(y => (y.CurriculumId.ToString() == this.CourseList[SelectedCourseIndex].ItemId))
                                .GroupBy(x => x.CurriculumId)
                           .Select(grp => new BindableDailyMarksModel
                           {
                               ParentMarksData = bindableData
                               .FirstOrDefault(stu => stu.CurriculumId == grp.Key),
                               SubCourseList = grp != null ? grp.Where(y => y.LevelNo != 0).ToList() : new List<BindableStudentGradingBookDataView>()
                           }));
                        }
                        else
                        {
                            DailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>(bindableData
                           .GroupBy(x => x.CurriculumId)
                           .Select(grp => new BindableDailyMarksModel
                           {
                               ParentMarksData = bindableData
                               .FirstOrDefault(stu => stu.CurriculumId == grp.Key),
                               SubCourseList = grp!=null ? grp.Where(y => y.LevelNo != 0).ToList() : new List<BindableStudentGradingBookDataView>()
                           }));
                            Console.WriteLine("icampus > after grouping " + DateTime.Now);
                        }
                    }
                    IsEffortVisible = DailyMarksData.IsAtleastOneCourseHasEffort;
                }
                else
                {
                    DailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>();
                }
                IsNoRecordMsg = DailyMarksParentChildList.Count > 0 ? false : true;
                await ApiHelper.HideProcessingIndicatorPopup();
                return DailyMarksData;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.CalendarPageTitle);
                await ApiHelper.HideProcessingIndicatorPopup();
                return new BindableGradingBookView();
            }
        }
        private void ExpandClicked(BindableDailyMarksModel bindableDailyMarksModel)
        {
            if (bindableDailyMarksModel != null)
            {
                //BindableDailyMarksModel bindableDailyMarksModel = (BindableDailyMarksModel)obj;
                if (bindableDailyMarksModel != null)
                    DailyMarksParentChildList[DailyMarksParentChildList.IndexOf(bindableDailyMarksModel)].ParentMarksData.IsExpandMode = !DailyMarksParentChildList[DailyMarksParentChildList.IndexOf(bindableDailyMarksModel)].ParentMarksData.IsExpandMode;
                DailyMarksParentChildList = new ObservableCollection<BindableDailyMarksModel>(DailyMarksParentChildList);
                bindableDailyMarksModel.ListViewHeight = bindableDailyMarksModel.SubCourseList.Count * 50;
            }
        }
        #endregion
    }