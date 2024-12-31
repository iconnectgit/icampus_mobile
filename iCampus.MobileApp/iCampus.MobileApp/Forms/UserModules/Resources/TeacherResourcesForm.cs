using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Resources;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class TeacherResourcesForm:ViewModelBase
    {
        #region Declarations
        private Popup _currentPopup;
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand AddNewResourceCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand DeleteClickCommand { get; set; }
        public ICommand RefreshedCommand { get; set; }
        #endregion

        #region Properties
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
        List<PickListItem> _gradeList = new List<PickListItem>();
        public List<PickListItem> GradeList
        {
            get
            {
                return _gradeList;
            }
            set
            {
                _gradeList = value;
                OnPropertyChanged(nameof(GradeList));
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
        ResourceViewModel _resourceDetails = new ResourceViewModel();
        public ResourceViewModel ResourceDetails
        {
            get => _resourceDetails;
            set
            {
                _resourceDetails = value;
                OnPropertyChanged(nameof(ResourceDetails));
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
        IList<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
        public IList<AttachmentFileView> AttachmentFiles
        {
            get => _attachmentFiles;
            set
            {
                _attachmentFiles = value;
                OnPropertyChanged(nameof(AttachmentFiles));
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
        public TeacherResourcesForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        public async Task GetResourceData()
        {
            try
            {
                string cacheKeyPrefix = "resources";
                bool loadFilterPanelLists = !GradeList.Any()||!TermList.Any();
                var apiUrl = string.Format(TextResource.ResourcesDataAPIUrl, SelectedStudent.ItemId, "","","","", loadFilterPanelLists);
                ResourceData = await ApiHelper.GetObject<ResourceViewModel>(apiUrl, cacheKeyPrefix: cacheKeyPrefix, cacheType: AppSettings.Current.RefreshResourseData ? ApiHelper.CacheTypeParam.LoadFromServerAndCache : ApiHelper.CacheTypeParam.LoadFromCache);
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    AppSettings.Current.RefreshResourseData = false;
                if (ResourceData!=null)
                {
                    ResourceList = (ResourceData.ResourcesList != null && ResourceData.ResourcesList.Count() > 0) ? ResourceList = new ObservableCollection<BindableResourceView>(_mapper.Map<List<BindableResourceView>>(ResourceData.ResourcesList)) : new ObservableCollection<BindableResourceView>();
                    if(loadFilterPanelLists)
                    {
                        this.GradeList = ResourceData.Grades != null ? ResourceData.Grades.ToList() : new List<PickListItem>();
                        this.TermList = ResourceData.Terms != null ? ResourceData.Terms.ToList() : new List<PickListItem>();
                        this.CourseList = ResourceData.Courses != null ? ResourceData.Courses.ToList() : new List<CurriculumView>();
                    }
                    var sortedResources = ResourceList.OrderByDescending(r => DateTime.Parse(r.Date));
                    ResourceList = new ObservableCollection<BindableResourceView>(sortedResources);
                    foreach (var item in ResourceList)
                    {
                        item.IsCourseVisible = item.CurriculumId != null ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
            IsNoRecordMsg = ResourceList != null && ResourceList.Count > 0 ? false : true;
            FilteredResourceList = ResourceList;
        }
        private void InitializePage()
        {
            MenuVisible = true;
            IsNoRecordMsg = false;
            AttachmentClickCommand = new Command<BindableResourceView>(AttachmentClicked);
            AddNewResourceCommand = new Command(AddIconClicked);
            SearchCommand = new Command(Search);
            EditClickCommand = new Command<BindableResourceView>(EditClicked);
            DeleteClickCommand = new Command<BindableResourceView>(DeleteClicked);
            this.RefreshedCommand = new Command(RefreshResourceList);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            MessagingCenter.Subscribe<AddNewResourcesForm>(this, "UpdateResourcesData", async (arg) =>
            {
                AppSettings.Current.RefreshResourseData = true;
                await GetResourceData();
            });
        }

        private async void AttachmentClicked(BindableResourceView sender)
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
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        private async void AddIconClicked()
        {
            AddNewResourcesForm addNewResourcesForm = new (_mapper, _nativeServices, Navigation)
            {
                PageTitle = PageTitle,
                BackVisible = true,
                MenuVisible = false,
                IsPopUpPage = false,
                GradeList = this.GradeList,
                TermList = this.TermList,
                CourseList = this.CourseList,
                FilterCourseList = this.CourseList,
                SelectedResource =
                {
                    IsAddMode = true
                }
            };
            AddNewResourcePage addNewResourcePage = new()
            {
                BindingContext = addNewResourcesForm
            };
            await Navigation.PushAsync(addNewResourcePage);
        }
        private void Search()
        {
            if (!string.IsNullOrEmpty(SearchText) && SearchText.Length >=3)
            {
                    FilteredResourceList = _mapper.Map<ObservableCollection<BindableResourceView>>(ResourceList.Where(i => i.Title.ToLower().Contains(SearchText.ToLower())).ToList());
            }
            else
            {
                FilteredResourceList = ResourceList;
            }
        }
        private async void EditClicked(BindableResourceView resourceView)
        {
                AddNewResourcesForm addNewResourcesForm = new AddNewResourcesForm(_mapper, _nativeServices, Navigation);
                addNewResourcesForm.PageTitle = PageTitle;
                addNewResourcesForm.BackVisible = true;
                addNewResourcesForm.MenuVisible = false;
                addNewResourcesForm.IsPopUpPage = false;
                addNewResourcesForm.SelectedResource.IsAddMode = false;
                addNewResourcesForm.SelectedResource.ResourceId = resourceView.ResourceId;
                addNewResourcesForm.CourseList = CourseList;
                addNewResourcesForm.IsEditMode = true;
                addNewResourcesForm.AttachmentFiles = new ObservableCollection<AttachmentFileView>(resourceView.AttachmentFileList);
                addNewResourcesForm.AttachmentListViewHeight = addNewResourcesForm.AttachmentFiles.Count * 45;
            try
                {
                    var apiUrl = string.Format(TextResource.ResourceDetailsApiUrl, resourceView.ResourceId);
                    ResourceDetails = await ApiHelper.GetObject<ResourceViewModel>(apiUrl);
                    if(ResourceDetails != null)
                    {
                    addNewResourcesForm.GradeList = ResourceDetails.Grades!=null?ResourceDetails.Grades.ToList():new List<PickListItem>();
                    addNewResourcesForm.TermList = ResourceDetails.Terms!=null?ResourceDetails.Terms.ToList():new List<PickListItem>();
                    addNewResourcesForm.SelectedGrade = addNewResourcesForm.PreviousSelectedGrade =  addNewResourcesForm.GradeList != null ? addNewResourcesForm.GradeList.Where(x => x.ItemId == ResourceDetails.ResourceData.GradeId.ToString())?.FirstOrDefault() : new PickListItem();
                    addNewResourcesForm.SelectedTerm = addNewResourcesForm.TermList != null ? addNewResourcesForm.TermList.Where(x => x.ItemId == ResourceDetails.ResourceData.TermId.ToString())?.FirstOrDefault() : new PickListItem();
                    if (ResourceDetails.ResourceData != null)
                    {
                        addNewResourcesForm.Title = ResourceDetails.ResourceData.Title_1;
                        addNewResourcesForm.Data=ResourceDetails.ResourceData.Data!=null ? Regex.Replace(ResourceDetails.ResourceData.Data.ToString(), "<.*?>", String.Empty) : string.Empty;
                        addNewResourcesForm.SelectedResource = _mapper.Map<BindableResourceView>(ResourceDetails.ResourceData);
                        addNewResourcesForm.IsPublishSelected = ResourceDetails.ResourceData.IsPublished;
                        addNewResourcesForm.SelectedClassList = ResourceDetails.ResourceData.ResourceClassList.ToList();
                    }
                    addNewResourcesForm.SelectedCourse = addNewResourcesForm.PreviousSelectedCourse = CourseList.Where(x => x.CurriculumId == resourceView.CurriculumId).FirstOrDefault();
                    addNewResourcesForm.FilterCourseList = CourseList.Where(x => x.GradeId.ToString().Equals(addNewResourcesForm.SelectedGrade.ItemId)).ToList();
                }
            }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
                addNewResourcesForm.SelectedResource.AttachmentFileList = _mapper.Map<List<AttachmentFileView>>(resourceView.AttachmentFileList);
                AddNewResourcePage addNewResourcePage = new()
                {
                    BindingContext = addNewResourcesForm
                };
                await Navigation.PushAsync(addNewResourcePage);
        }

        private async void DeleteClicked(BindableResourceView resourceClass)
        {
            try
            {
                if (resourceClass != null)
                {
                    var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.DeleteResourceApiUrl, resourceClass.ResourceId));
                        if (result.Success)
                        {
                            var deletedObj = ResourceList.Where(x => x.ResourceId.Equals(resourceClass.ResourceId));
                            if (deletedObj != null && deletedObj.FirstOrDefault() != null)
                            {
                                ResourceList.Remove(deletedObj.FirstOrDefault());
                                AppSettings.Current.RefreshResourseData = true;
                                await GetResourceData();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        public async override void GetStudentData()
        {
            base.GetStudentData();
            await GetResourceData();
        }
        private async void RefreshResourceList()
        {
            IsRefreshing = true;
            await GetResourceData();
            IsRefreshing = false;
        }
        #endregion
    }