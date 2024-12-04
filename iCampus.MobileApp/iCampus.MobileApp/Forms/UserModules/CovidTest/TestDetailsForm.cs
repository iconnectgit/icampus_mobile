using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.CovidTest;

namespace iCampus.MobileApp.Forms.UserModules.CovidTest;

public class TestDetailsForm : ViewModelBase
{
    #region Declarations

    private Popup _currentPopup;
    public ICommand SearchClickCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }
    public ICommand EditClickCommand { get; set; }
    public ICommand DeleteClickCommand { get; set; }
    public ICommand AddCovidTestDetailsCommand { get; set; }
    public ICommand DocumentClickCommand { get; set; }
    public ICommand ListViewTabbedCommand { get; set; }

    #endregion

    #region Properties

    private ObservableCollection<CovidTestView> _testDetailsList = new();

    public ObservableCollection<CovidTestView> TestDetailsList
    {
        get => _testDetailsList;
        set
        {
            _testDetailsList = value;
            OnPropertyChanged(nameof(TestDetailsList));
        }
    }
    private ObservableCollection<CovidTestView> _masterTestDetailsList = new();

    public ObservableCollection<CovidTestView> MasterTestDetailsList
    {
        get => _masterTestDetailsList;
        set
        {
            _masterTestDetailsList = value;
            OnPropertyChanged(nameof(MasterTestDetailsList));
        }
    }

    private DateTime _fromDate = DateTime.Now;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            if (value <= ToDate)
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private DateTime _toDate = DateTime.Now;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            if (value >= FromDate)
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private bool _dateErrorMessageVisibility;

    public bool DateErrorMessageVisibility
    {
        get => _dateErrorMessageVisibility;
        set
        {
            _dateErrorMessageVisibility = value;
            OnPropertyChanged(nameof(DateErrorMessageVisibility));
        }
    }
    private CovidTestView _selectedDetails = new();

    public CovidTestView SelectedDetails
    {
        get => _selectedDetails;
        set
        {
            _selectedDetails = value;
            OnPropertyChanged(nameof(SelectedDetails));
        }
    }

    private ObservableCollection<ExtPickListItem> _personTypeList = new();

    public ObservableCollection<ExtPickListItem> PersonTypeList
    {
        get => _personTypeList;
        set
        {
            _personTypeList = value;
            OnPropertyChanged(nameof(PersonTypeList));
        }
    }

    private FamilyInformation _familyInformation = new();

    public FamilyInformation FamilyInformation
    {
        get => _familyInformation;
        set
        {
            _familyInformation = value;
            OnPropertyChanged(nameof(FamilyInformation));
        }
    }

    private bool _isNoRecordMsg;

    public bool IsNoRecordMsg
    {
        get => _isNoRecordMsg;
        set
        {
            _isNoRecordMsg = value;
            OnPropertyChanged(nameof(IsNoRecordMsg));
        }
    }

    private IList<BindableAttachmentFileView> _selectedAttachmentList;

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

    public TestDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        SearchClickCommand = new Command(SearchClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        ToDate = DateTime.Now;
        FromDate = ToDate.AddDays(-30);
        FilterClickCommand = new Command(FilterClicked);
        EditClickCommand = new Command<CovidTestView>(EditClicked);
        DeleteClickCommand = new Command<CovidTestView>(DeleteClicked);
        MessagingCenter.Subscribe<string>("", "ListViewRightSwipeCovidTestDetailsSubscribe", (argCovidTestDetails) =>
        {
            MessagingCenter.Subscribe<string>("", "ListViewRightSwipeCovidTestDetails",
                async (argumentCovidTestDetails) =>
                {
                    //await SideMenuClicked();
                });
        });
        AddCovidTestDetailsCommand = new Command(AddCovidTestDetailsClicked);
        DocumentClickCommand = new Command<CovidTestView>(DocumentClicked);
        ListViewTabbedCommand = new Command<CovidTestView>(ListViewTabbedMethod);
        await GetCovidTestDetails();
        MessagingCenter.Subscribe<AddTestDetailsForm>(this, "AddTestDetails",
            async (arg) => { await GetCovidTestDetails(); });
    }

    public void ListViewTabbedMethod(CovidTestView covidTestView)
    {
        if (covidTestView != null)
        {
            foreach (var item in TestDetailsList.ToList())
                if (item != null)
                {
                    if (item.CovidTestId == covidTestView.CovidTestId)
                    {
                        item.DetailsVisibility = !item.DetailsVisibility;
                        item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                            ? "dropdown_gray.png"
                            : "uparrow_gray.png";
                    }
                    else
                    {
                        item.DetailsVisibility = false;
                        item.ArrowImageSource = "dropdown_gray.png";
                    }
                }
            MessagingCenter.Send("", "ExpandCollapse");
        }
    }

    private async void SearchClicked(object obj)
    {
        try
        {
            MenuVisible = true;
            BackVisible = false;
            IsPopUpPage = false;
            await GetCovidTestDetails();
            var filteredList = MasterTestDetailsList
                .Where(test => DateTime.TryParse(test.FormattedTestDate, out DateTime testDate)
                               && testDate >= FromDate && testDate <= ToDate)
                .ToList();

            TestDetailsList = new ObservableCollection<CovidTestView>(filteredList);
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    private async void FilterClicked(object obj)
    {
        try
        {
            MenuVisible = false;
            BackVisible = true;
            IsPopUpPage = true;
            TestDetailsFilterPage testDetailsFilterPage = new()
            {
                BindingContext = this
            };
            await Navigation.PushAsync(testDetailsFilterPage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    public override void BackClicked(object obj)
    {
        base.BackClicked(obj);
        MenuVisible = true;
        BackVisible = false;
    }

    private async void EditClicked(CovidTestView obj)
    {
        if (obj != null)
        {
            AddTestDetailsForm addTestDetailsForm = new(_mapper, _nativeServices, Navigation)
            {
                BackVisible = true,
                MenuVisible = false,
                SelectedDetails = obj
            };
            addTestDetailsForm.SelectedDetails.IsAddMode = false;
            addTestDetailsForm.SelectedDetails.PersonName = AppSettings.Current.DetailedDisplayName;
            addTestDetailsForm.TestLocation = obj.TestLocation;
            addTestDetailsForm.TestStatus = obj.TestStatus;
            addTestDetailsForm.TestDate = obj.TestDate;
            await addTestDetailsForm.SetTestStatus();
            //HostScreen.Router.Navigate.Execute(addTestDetailsForm).Subscribe();
        }
    }

    private async void DeleteClicked(CovidTestView obj)
    {
        try
        {
            if (obj != null)
            {
                var deleteTapAction = await Application.Current.MainPage.DisplayAlert(
                    TextResource.DeleteConfirmationTitle, TextResource.DeleteCovidTestDetailsConfirmationMessage,
                    TextResource.YesText, TextResource.NoText);
                if (deleteTapAction)
                {
                    var result = await ApiHelper.PostRequest<OperationDetails>(
                        string.Format(TextResource.DeleteCovidTestData, obj.CovidTestId, obj.DocumentName),
                        AppSettings.Current.ApiUrl);
                    if (result.Success)
                    {
                        var deletedObj = TestDetailsList.Where(x => x.CovidTestId.Equals(obj.CovidTestId));
                        if (deletedObj != null && deletedObj.FirstOrDefault() != null) await GetCovidTestDetails();
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async void AddCovidTestDetailsClicked(object obj)
    {
        try
        {
            AddTestDetailsForm addTestDetailsForm = new(_mapper, _nativeServices, Navigation)
            {
                BackVisible = true,
                MenuVisible = false,
                PageTitle = PageTitle,
                SelectedDetails =
                {
                    IsAddMode = true
                }
            };
            if (AppSettings.Current.IsParent)
            {
                addTestDetailsForm.PersonTypeList = PersonTypeList;
                addTestDetailsForm.SelectedDetails.FatherName = FamilyInformation.FatherName;
                addTestDetailsForm.SelectedDetails.MotherName = FamilyInformation.MotherName;
                if (PersonTypeList != null && PersonTypeList.Count > 0)
                {
                    var covidTestView = PersonTypeList.Where(x => x.ItemId.ToLower().Equals("f"));
                    addTestDetailsForm.SelectedPersonTested =
                        covidTestView != null && covidTestView.FirstOrDefault() != null
                            ? covidTestView.FirstOrDefault()
                            : new ExtPickListItem();
                }
                //addTestDetailsForm.PersonTested = FamilyInformation.FatherName;
                //await addTestDetailsForm.SetData();
            }
            AddTestDetails addTestDetails = new ()
            {
                BindingContext = addTestDetailsForm
            };
            await Navigation.PushAsync(addTestDetails);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void DocumentClicked(CovidTestView sender)
    {
        try
        {
            if (sender != null)
            {
                List<AttachmentFileView> attachmentList = new List<AttachmentFileView>();
                attachmentList.Add(sender.AttachmentView);
                AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(attachmentList)
                };
                var attachmentListPopup = new AttachmentListPopup()
                {
                    BindingContext = attachmentListPopupForm
                };
                SetPopupInstance(attachmentListPopup);
                await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
                
                
                
                
            }
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

    public async Task<IEnumerable<CovidTestView>> GetCovidTestDetails()
    {
        try
        {
            bool loadFilterPanelLists;
            loadFilterPanelLists = AppSettings.Current.IsTeacher ? false : true;
            var covidTestData = await ApiHelper.GetObject<BindableCovidTestViewModel>(
                string.Format(TextResource.GetCovidTestData, FromDate, ToDate, loadFilterPanelLists));
            if (covidTestData != null)
            {
                if (covidTestData.CovidTestList != null && covidTestData.CovidTestList.Count > 0)
                {
                    MasterTestDetailsList = new ObservableCollection<CovidTestView>(covidTestData.CovidTestList);
                    foreach (var item in TestDetailsList)
                        if (item != null)
                            item.FormattedTestStatus = item.TestStatus ? "Positive" : "Negative";
                    MasterTestDetailsList = new ObservableCollection<CovidTestView>(MasterTestDetailsList
                        .OrderByDescending(x => DateTime.Parse(x.FormattedTestDate)).ToList());
                }

                if (covidTestData.PersonTypes != null && covidTestData.PersonTypes.ToList().Count > 0)
                    PersonTypeList = new ObservableCollection<ExtPickListItem>(covidTestData.PersonTypes.ToList());
                if (covidTestData.FamilyInformation != null) FamilyInformation = covidTestData.FamilyInformation;
            }

            TestDetailsList = MasterTestDetailsList;
            IsNoRecordMsg = TestDetailsList != null && TestDetailsList.Count > 0 ? false : true;
            return TestDetailsList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new List<CovidTestView>();
        }
    }

    #endregion
}