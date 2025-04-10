using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Resources;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class ParentStudentResourcesForm : ViewModelBase
    {
        #region Declarations
        private Popup _currentPopup;
        public ICommand SearchCommand { get; set; }
        public ICommand FilterClickCommand { get; set; }
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand ArrowClickedCommand { get; set; }
        public ICommand RefreshedCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        #endregion

        #region Properties
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
        ObservableCollection<BindableResourceView> _resourceList = new ObservableCollection<BindableResourceView>();
        public ObservableCollection<BindableResourceView> ResourceList
        {
            get
            {
                return _resourceList;
            }
            set
            {
                _resourceList = value;
                OnPropertyChanged(nameof(ResourceList));
            }
        }
        ObservableCollection<BindableResourceView> _filteredResourceList = new ObservableCollection<BindableResourceView>();
        public ObservableCollection<BindableResourceView> FilteredResourceList
        {
            get
            {
                return _filteredResourceList;
            }
            set
            {
                _filteredResourceList = value;
                OnPropertyChanged(nameof(FilteredResourceList));
            }
        }
        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        ResourceViewModel _resourceData = new ResourceViewModel();
        public ResourceViewModel ResourceData
        {
            get => _resourceData;
            set
            {
                _resourceData = value;
                OnPropertyChanged(nameof(ResourceData));
            }
        }
        List<PickListItem> _termList = new List<PickListItem>();
        public List<PickListItem> TermList
        {
            get
            {
                return _termList;
            }
            set
            {
                _termList = value;
                OnPropertyChanged(nameof(TermList));
            }
        }
        PickListItem _selectedTerm = new PickListItem();
        public PickListItem SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                _selectedTerm = value;
                OnPropertyChanged(nameof(SelectedTerm));
            }
        }
        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        DateTime _fromDate = DateTime.MinValue;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }

        DateTime _toDate = DateTime.MaxValue;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }
        List<CurriculumView> _courseList = new List<CurriculumView>();
        public List<CurriculumView> CourseList
        {
            get => _courseList;
            set
            {
                _courseList = value;
                OnPropertyChanged(nameof(CourseList));
            }
        }
        CurriculumView _selectedCourse = new CurriculumView();
        public CurriculumView SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));
            }
        }
        IList<BindableAttachmentFileView> _selectedAttachmentList;
        public IList<BindableAttachmentFileView> SelectedAttachmentList
        {
            get => _selectedAttachmentList;
            set
            {
                _selectedAttachmentList = value;
                OnPropertyChanged(nameof(SelectedAttachmentList));
            }
        }
        #endregion
        public ParentStudentResourcesForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        public async override void GetStudentData()
        {
            base.GetStudentData();
            await GetResourceData();
        }
        private async void InitializePage()
        {
            HelperMethods.GetSelectedStudent();
            this.MenuVisible = true;
            SearchCommand = new Command(Search);
            FilterClickCommand = new Command(FilterClicked);
            AttachmentClickCommand = new Command<BindableResourceView>(AttachmentClicked);
            ArrowClickedCommand = new Command<BindableResourceView>(ArrowClicked);
            this.RefreshedCommand = new Command(RefreshResourceList);
            SearchButtonClickCommand = new Command(SearchButtonClickedMethod);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            MessagingCenter.Subscribe<ResourcesFilterForm>(this, "Search", async (filterFormData) =>
            {
                SelectedTerm = filterFormData.SelectedTerm;
                SelectedCourse = filterFormData.SelectedCourse;
                FromDate = filterFormData.FromDate;
                ToDate = filterFormData.ToDate;
                await GetResourceData();
            });
        }
        public async Task GetResourceData()
        {
            try
            {
                string cacheKeyPrefix = "resources";
                var curriculumId = SelectedCourse.CurriculumId == 0 ? null : SelectedCourse.CurriculumId.ToString();
                bool loadFilterPanelLists = true;
                var apiUrl = string.Format(TextResource.ResourcesDataAPIUrl, AppSettings.Current.SelectedStudent.ItemId, "", "", "", "", loadFilterPanelLists);
                ResourceData = await ApiHelper.GetObject<ResourceViewModel>(apiUrl, cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                if (ResourceData != null)
                {
                    if (ResourceData.ResourcesList != null && ResourceData.ResourcesList.Any())
                    {
                        var bindableResourceList = _mapper.Map<List<BindableResourceView>>(ResourceData.ResourcesList);
        
                        ResourceList = new ObservableCollection<BindableResourceView>(bindableResourceList);
                    }
                    else
                    {
                        ResourceList = new ObservableCollection<BindableResourceView>();
                    }
                    
                    TermList = ResourceData.Terms != null ? ResourceData.Terms.ToList() : new List<PickListItem>();
                    CourseList = ResourceData.Courses != null ? ResourceData.Courses.ToList() : new List<CurriculumView>();
                    var sortedResources = ResourceList.OrderByDescending(r => DateTime.Parse(r.Date));
                    ResourceList = new ObservableCollection<BindableResourceView>(sortedResources);

                    foreach (var item in ResourceList)
                    {
                        item.IsCourseVisible = item.CurriculumId != null ? true : false;
                    }
                }
                IsNoRecordMsg = ResourceList != null && ResourceList.Count > 0 ? false : true;
                FilteredResourceList = ResourceList;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        private async void Search()
        {
            if (!string.IsNullOrEmpty(SearchText) && SearchText.Length >= 3)
            {
                FilteredResourceList = _mapper.Map<ObservableCollection<BindableResourceView>>(new ObservableCollection<BindableResourceView>(ResourceList.Where(i => i.Title.ToLower().Contains(SearchText.ToLower()) || i.Data.ToLower().Contains(SearchText.ToLower()) || i.UserName.ToLower().Contains(SearchText.ToLower())).ToList()));
            }
            else
            {
                FilteredResourceList = ResourceList;
            }
        }
        async void SearchButtonClickedMethod()
        {
            DateTime fromDate = FromDate.Date;
            DateTime toDate = ToDate;
            var filteredResourceList = new ObservableCollection<BindableResourceView>(ResourceList.Where(x => DateTime.Parse(x.Date) >= fromDate && DateTime.Parse(x.Date) <= toDate));
            FilteredResourceList = filteredResourceList;

        }
        private async void FilterClicked(object obj)
        {
            ResourcesFilterForm resourcesFilterForm = new(_mapper, _nativeServices, Navigation)
            {
                PageTitle = TextResource.FilterResourcesTitle,
                MenuVisible = false,
                BackVisible = true,
                TermList = TermList,
                CourseList = CourseList
            };
            await resourcesFilterForm.GetResourceData();
            resourcesFilterForm.SetList();
            resourcesFilterForm.ResourceList = ResourceList;
            FilterResources filterResources = new ()
            {
                BindingContext = resourcesFilterForm
            };
            await Navigation.PushAsync(filterResources);
        }
        private async void AttachmentClicked(BindableResourceView sender)
        {
            try
            {
                AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentFileList)
                };
                var attachmentListPopup = new AttachmentListPopup()
                {
                    BindingContext = attachmentListPopupForm
                };
                SetPopupInstance(attachmentListPopup);
                await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        async void ArrowClicked(BindableResourceView bindableResourceView)
        {
            if (bindableResourceView != null)
            {
                foreach (var item in ResourceList.ToList())
                {
                    if (item != null)
                    {
                        if (item.ResourceId == bindableResourceView.ResourceId)
                        {
                            item.DescriptionVisibility = !item.DescriptionVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DescriptionVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }
                }
            }
            MessagingCenter.Send("", "ExpandCollapse");
        }
        
        private async void RefreshResourceList()
        {
            IsRefreshing = true;
            await GetResourceData();
            IsRefreshing = false;
        }
        #endregion
    }