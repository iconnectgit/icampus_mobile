using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Calendar;
using iCampus.Portal.EditModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class WeeklyPlanDetailFrom : ViewModelBase
	{
        #region Declaration
        public ICommand EditClickCommand { get; set; }
        #endregion
        #region Properties
        BindableAgendaWeeklyGroupView _selectedWeeklyAgenda = new BindableAgendaWeeklyGroupView();
        public BindableAgendaWeeklyGroupView SelectedWeeklyAgenda
        {
            get => _selectedWeeklyAgenda;
            set
            {
                _selectedWeeklyAgenda = value;
                OnPropertyChanged(nameof(SelectedWeeklyAgenda));
            }
        }
        bool _isEditButtonVisibleForTeacher;
        public bool IsEditButtonVisibleForTeacher
        {
            get => _isEditButtonVisibleForTeacher;
            set
            {
                _isEditButtonVisibleForTeacher = value;
                OnPropertyChanged(nameof(IsEditButtonVisibleForTeacher));
            }
        }
        string _weeklyPlanHeaderText;
        public string WeeklyPlanHeaderText
        {
            get => _weeklyPlanHeaderText;
            set
            {
                _weeklyPlanHeaderText = value;
                OnPropertyChanged(nameof(WeeklyPlanHeaderText));
            }
        }
        AgendaWeeklyGroupEdit _editQuickPostData = new AgendaWeeklyGroupEdit();
        public AgendaWeeklyGroupEdit EditQuickPostData
        {
            get => _editQuickPostData;
            set
            {
                _editQuickPostData = value;
                OnPropertyChanged(nameof(EditQuickPostData));
            }
        }
        #endregion
        public WeeklyPlanDetailFrom(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
            BackVisible = true;
            EditClickCommand = new Command(EditClickCommandMethod);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        }

        private async void EditClickCommandMethod(object obj)
        {
            try
            {
                if(SelectedWeeklyAgenda != null)
                {
                    await AddEditData();
                    QuickPostForm quickPostForm = new QuickPostForm(_mapper, _nativeServices, Navigation);
                    quickPostForm.PageTitle = "Quick Post";
                    quickPostForm.MenuVisible = false;
                    quickPostForm.BackVisible = true;
                    quickPostForm.IsPopUpPage = false;
                    quickPostForm.IsEditPostVisible = true;
                    quickPostForm.IsEditMode = true;
                    quickPostForm.IsNewPostVisible = false;
                    quickPostForm.NewPostRadioButtonImage = "unselected_radio_button.png";
                    quickPostForm.EditPostRadioButtonImage = "selected_radio_button.png";
                    quickPostForm.CourseList = quickPostForm.FilteredCourseList = EditQuickPostData.AgendaForList;
                    quickPostForm.CourseListViewHeight = quickPostForm.CourseList.Count * 32;
                    quickPostForm.SelectedCourse = quickPostForm.FilteredCourseList.FirstOrDefault(x => x.ItemId == EditQuickPostData.AgendaWeeklyGroupDetails.CurriculumId.ToString());
                    quickPostForm.SelectedAgendaForText = quickPostForm.SelectedCourse?.ItemName;
                    quickPostForm.MonthList = EditQuickPostData.Months;
                    quickPostForm.MonthListViewHeight = quickPostForm.MonthList.Count * 32;
                    quickPostForm.SelectedMonthList = EditQuickPostData.Months.FirstOrDefault(x => x.ItemId == EditQuickPostData.AgendaWeeklyGroupDetails.MonthOfWeekStartDate.ToString());
                    quickPostForm.MonthSelectedText = quickPostForm.SelectedMonthList.ItemName;
                    quickPostForm.GroupList = EditQuickPostData.GroupTitles;
                    quickPostForm.GroupListViewHeight = quickPostForm.GroupList.Count * 32;
                    //quickPostForm.SelectedGroupList = EditQuickPostData.GroupTitles.FirstOrDefault(x => x.ItemId == EditQuickPostData.AgendaWeeklyGroupDetails.AgendaWeeklyGroupId.ToString());
                    quickPostForm.GroupSelectedText = quickPostForm.SelectedGroupList.ItemName;
                    quickPostForm.GroupTitle = EditQuickPostData.AgendaWeeklyGroupDetails.Title;
                    quickPostForm.IsTitleVisible = true;
                    quickPostForm.DateRangeText = EditQuickPostData.AgendaWeeklyGroupDetails.DateRange;
                    quickPostForm.IsDateRangeVisible = true;
                    quickPostForm.IsFromEditButton = true;
                    quickPostForm.AgendaWeeklyGroupId = EditQuickPostData.AgendaWeeklyGroupDetails.AgendaWeeklyGroupId;
                    quickPostForm.TypeId = EditQuickPostData.AgendaWeeklyGroupDetails.AgendaTypeId;
                    quickPostForm.WeekStartDate = EditQuickPostData.AgendaWeeklyGroupDetails.WeekStartDate.ToString("dd-MMM-yyyy");
                    quickPostForm.WeekEndDate = EditQuickPostData.AgendaWeeklyGroupDetails.WeekEndDate.ToString("dd-MMM-yyyy");
                    quickPostForm.ClassList = _mapper.Map<ObservableCollection<BindableAgendaClassView>>(EditQuickPostData.AgendaClassesViewModel.AgendaClassList);
                    if (quickPostForm.ClassList != null)
                    {
                        quickPostForm.ClassListViewHeight = quickPostForm.ClassList != null && quickPostForm.ClassList.Count > 0 ? (quickPostForm.ClassList.Count % 3 == 0 ? (quickPostForm.ClassList.Count / 3) * 50 : ((quickPostForm.ClassList.Count / 3) * 50) + 50) : quickPostForm.ClassListViewHeight;
                        quickPostForm.ClassesVisibility = true;
                        var classData = quickPostForm.ClassList.Where(x => !x.IsChecked);
                        quickPostForm.IsClassesSelected = !classData.Any();
                    }
                    
                    quickPostForm.DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                    foreach (var item in EditQuickPostData.AgendaWeekDatesFormatted)
                    {
                        BindableQuickPost quickPost = new BindableQuickPost();
                        quickPost.AgendaWeekDatesFormatted = item;
                        quickPostForm.DateWiseQuickPost.Add(quickPost);
                    }
                    
                    quickPostForm.CurriculumStandardsVisibility = true;
                    quickPostForm.CurriculamStandardsText = EditQuickPostData.AgendaWeeklyGroupDetails.CurriculumStandards;
                    quickPostForm.RemarkVisibility = true;
                    quickPostForm.RemarkText = EditQuickPostData.AgendaWeeklyGroupDetails.Remarks;
                    quickPostForm.IsEnableLearningOutcomes = EditQuickPostData.CalendarControlSetting.EnableLearningOutcomes;


                    var groupedAgendaData = EditQuickPostData.GroupedAgendaData.ToList();
                    var agendaAttachmentList = EditQuickPostData.AgendaAttachmentList;

                    foreach (var item in groupedAgendaData)
                    {
                        if (DateTime.TryParse(item.DueDate, out DateTime dueDate))
                        {
                            var bindableQuickPost = quickPostForm.DateWiseQuickPost
                                .FirstOrDefault(qp => DateTime.TryParse(qp.AgendaWeekDatesFormatted, out DateTime qpDate) 
                                                      && qpDate.Date == dueDate.Date);

                            if (bindableQuickPost != null)
                            {
                                bindableQuickPost.AgendaUId = item.AgendaUId;
                                bindableQuickPost.AgendaId = item.AgendaId;
                                bindableQuickPost.AgendaDescription = item.AgendaDescription;
                                bindableQuickPost.LearningOutcomes = item.LearningOutcomes;
                                bindableQuickPost.IsDeleted = item.IsDeleted;

                                bindableQuickPost.AttachmentFiles = new ObservableCollection<AttachmentFileView>(
                                    agendaAttachmentList
                                        .Where(a => a.AgendaId == item.AgendaId)
                                        .Select(a => new AttachmentFileView
                                        {
                                            FileName = a.Attachment,
                                        }));

                                // Adjust the attachment list height dynamically
                                bindableQuickPost.AttachmentListViewHeight = bindableQuickPost.AttachmentFiles.Count * 40;
                            }
                        }
                    }
                    var datesList = EditQuickPostData.AgendaWeekDates;
                    for (int i = 0; i < datesList.Count; i++)
                    {
                        if (i < quickPostForm.DateWiseQuickPost.Count)
                        {
                            quickPostForm.DateWiseQuickPost[i].DueDate = datesList[i].ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (var item in quickPostForm.DateWiseQuickPost)
                    {
                        item.LearningOutcomesLabel = EditQuickPostData.CalendarControlSetting.LearningOutcomesLabel;
                        item.AssignmentsLabel = EditQuickPostData.CalendarControlSetting.AssignmentsLabel;
                        //item.AttachmentListViewHeight = item.AttachmentFiles.Count * 40;
                    }
                    quickPostForm.IsCancelButtonVisibleForTeacher = EditQuickPostData.IsDeleteButtonVisibleForTeacher;
                    quickPostForm.SaveButtonVisibility = true;

                    QuickPostPage quickPostPage = new QuickPostPage()
                    {
                        BindingContext = quickPostForm
                    };
                    await Navigation.PushAsync(quickPostPage);
                }

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public async Task AddEditData()
        {
            try
            {
                EditQuickPostData = await ApiHelper.GetObject<AgendaWeeklyGroupEdit>(string.Format(TextResource.LoadAgendaWeeklyPostPageInEditModeApi, SelectedWeeklyAgenda.AgendaWeeklyGroupId));
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }