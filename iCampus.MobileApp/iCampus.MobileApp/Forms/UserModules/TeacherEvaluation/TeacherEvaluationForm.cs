using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.TeacherEvaluation;

public class TeacherEvaluationForm : ViewModelBase
{
    #region Declaration

    public ICommand SelectStarCommandI { get; set; }
    public ICommand SelectStarCommandII { get; set; }
    public ICommand SelectStarCommandIII { get; set; }
    public ICommand SelectStarCommandIV { get; set; }
    public ICommand SelectStarCommandV { get; set; }
    public ICommand SubmitCommand { get; set; }

    #endregion

    #region Properties

    private List<BindableTeacherFeedBackView> _feedbackDataList;

    public List<BindableTeacherFeedBackView> FeedbackDataList
    {
        get => _feedbackDataList;
        set
        {
            _feedbackDataList = value;
            OnPropertyChanged(nameof(FeedbackDataList));
        }
    }

    private List<BindableTeacherFeedBackView> _selectedFeedbackDataList;

    public List<BindableTeacherFeedBackView> SelectedFeedbackDataList
    {
        get => _selectedFeedbackDataList;
        set
        {
            _selectedFeedbackDataList = value;
            OnPropertyChanged(nameof(SelectedFeedbackDataList));
        }
    }

    private bool _isNoRecordFound = false;

    public bool IsNoRecordFound
    {
        get => _isNoRecordFound;
        set
        {
            _isNoRecordFound = value;
            OnPropertyChanged(nameof(IsNoRecordFound));
        }
    }

    #endregion


    public TeacherEvaluationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        SelectStarCommandI = new Command<BindableTeacherFeedBackView>(SelectStarCommandClickedI);
        SelectStarCommandII = new Command<BindableTeacherFeedBackView>(SelectStarCommandClickedII);
        SelectStarCommandIII = new Command<BindableTeacherFeedBackView>(SelectStarCommandClickedIII);
        SelectStarCommandIV = new Command<BindableTeacherFeedBackView>(SelectStarCommandClickedIV);
        SelectStarCommandV = new Command<BindableTeacherFeedBackView>(SelectStarCommandClickedV);
        SubmitCommand = new Command<BindableTeacherFeedBackView>(SubmitCommandClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private async Task GetDetails()
    {
        try
        {
            FeedbackDataList = await ApiHelper.GetObject<List<BindableTeacherFeedBackView>>(
                string.Format(TextResource.GetTeacherFeedbackDataUrl, AppSettings.Current.SelectedStudent.ItemId));
            foreach (var item in FeedbackDataList)
            {
                item.StarImageSource1 = item.FeedBackId >= 1 ? "yellow_star.png" : "gray_star.png";
                item.StarImageSource2 = item.FeedBackId >= 2 ? "yellow_star.png" : "gray_star.png";
                item.StarImageSource3 = item.FeedBackId >= 3 ? "yellow_star.png" : "gray_star.png";
                item.StarImageSource4 = item.FeedBackId >= 4 ? "yellow_star.png" : "gray_star.png";
                item.StarImageSource5 = item.FeedBackId >= 5 ? "yellow_star.png" : "gray_star.png";
                item.SubmitButtonVisibility = item.FeedBackId > 0;
                item.FeedBackText = GetRatingText(item.FeedBackId);
            }

            IsNoRecordFound = FeedbackDataList.Count == 0;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private string GetRatingText(int rating)
    {
        switch (rating)
        {
            case 1:
                return "Unacceptable";
            case 2:
                return "Poor";
            case 3:
                return "Good";
            case 4:
                return "Very Good";
            case 5:
                return "Outstanding";
            default:
                return "";
        }
    }

    private async void SubmitCommandClicked(BindableTeacherFeedBackView obj)
    {
        try
        {
            var result = await ApiHelper.PostRequest<OperationDetails>(
                string.Format(TextResource.UpdateTeacherFeedbackDataUrl, obj.StudentId, obj.TeacherId, obj.CurriculumId,
                    obj.FeedBackId), AppSettings.Current.ApiUrl);
            if (result.Success) await HelperMethods.ShowAlert("", TextResource.DataCollectionSubmitMsg);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void SelectStarCommandClickedI(BindableTeacherFeedBackView starSelected)
    {
        HighLightStars(starSelected, 1);
    }

    private void SelectStarCommandClickedII(BindableTeacherFeedBackView starSelected)
    {
        HighLightStars(starSelected, 2);
    }

    private void SelectStarCommandClickedIII(BindableTeacherFeedBackView starSelected)
    {
        HighLightStars(starSelected, 3);
    }

    private void SelectStarCommandClickedIV(BindableTeacherFeedBackView starSelected)
    {
        HighLightStars(starSelected, 4);
    }

    private void SelectStarCommandClickedV(BindableTeacherFeedBackView starSelected)
    {
        HighLightStars(starSelected, 5);
    }

    private void HighLightStars(BindableTeacherFeedBackView starSelected, int feedback)
    {
        try
        {
            if (feedback >= 1 && feedback <= 5)
            {
                if (feedback == 1 && starSelected.StarImageSource1.Equals("yellow_star.png"))
                {
                    starSelected.StarImageSource1 = "gray_star.png";
                    starSelected.StarImageSource2 = "gray_star.png";
                    starSelected.StarImageSource3 = "gray_star.png";
                    starSelected.StarImageSource4 = "gray_star.png";
                    starSelected.StarImageSource5 = "gray_star.png";
                    feedback--;
                }
                else
                {
                    starSelected.StarImageSource1 = feedback >= 1 ? "yellow_star.png" : "gray_star.png";
                    starSelected.StarImageSource2 = feedback >= 2 ? "yellow_star.png" : "gray_star.png";
                    starSelected.StarImageSource3 = feedback >= 3 ? "yellow_star.png" : "gray_star.png";
                    starSelected.StarImageSource4 = feedback >= 4 ? "yellow_star.png" : "gray_star.png";
                    starSelected.StarImageSource5 = feedback == 5 ? "yellow_star.png" : "gray_star.png";
                }
            }

            starSelected.FeedBackText = GetRatingText(feedback);
            starSelected.SubmitButtonVisibility = feedback > 0;
            starSelected.FeedBackId = feedback;
            MessagingCenter.Send("", "CurrentExpandCollapse");
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public override async void GetStudentData()
    {
        try
        {
            await GetDetails();
            base.GetStudentData();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}