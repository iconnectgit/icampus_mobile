using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.Survey;

public class SurveyForm : ViewModelBase
{
    #region Declarations

    public ICommand CheckboxChangeCommand { get; set; }
    public ICommand PickerChangedEventCommand { get; set; }
    public ICommand RadioChangeCommand { get; set; }
    public ICommand SubmitCommand { get; set; }
    public ICommand TrueFalseChangeCommand { get; set; }

    #endregion

    #region Properties

    private UserSurveyView _userSurvey;

    public UserSurveyView UserSurvey
    {
        get => _userSurvey;
        set
        {
            _userSurvey = value;
            OnPropertyChanged(nameof(UserSurvey));
        }
    }

    private int _surveyId;

    public int SurveyId
    {
        get => _surveyId;
        set
        {
            _surveyId = value;
            OnPropertyChanged(nameof(SurveyId));
        }
    }

    private bool _isError;

    public bool IsError
    {
        get => _isError;
        set
        {
            _isError = value;
            OnPropertyChanged(nameof(IsError));
        }
    }

    private string _validationMessage;

    public string ValidationMessage
    {
        get => _validationMessage;
        set
        {
            _validationMessage = value;
            OnPropertyChanged(nameof(ValidationMessage));
        }
    }

    private BindableSurveyQuestionAnswerView _firstSurvey;

    public BindableSurveyQuestionAnswerView FirstSurvey
    {
        get => _firstSurvey;
        set
        {
            _firstSurvey = value;
            OnPropertyChanged(nameof(FirstSurvey));
        }
    }


    private ObservableCollection<BindableSurveyQuestionAnswerView> _userSurveyList;

    public ObservableCollection<BindableSurveyQuestionAnswerView> UserSurveyList
    {
        get => _userSurveyList;
        set
        {
            _userSurveyList = value;
            OnPropertyChanged(nameof(UserSurveyList));
        }
    }

    private bool _isMandateSurvey;

    public bool IsMandateSurvey
    {
        get => _isMandateSurvey;
        set
        {
            _isMandateSurvey = value;
            OnPropertyChanged(nameof(IsMandateSurvey));
        }
    }

    public bool IsFromMenu { get; set; }

    private bool _noRecordVisibility;

    public bool NoRecordVisibility
    {
        get => _noRecordVisibility;
        set
        {
            _noRecordVisibility = value;
            OnPropertyChanged(nameof(NoRecordVisibility));
        }
    }

    public static List<int> SubmittedSurveyIds { get; set; }

    #endregion

    public SurveyForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        HelperMethods.GetSelectedStudent();
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        CheckboxChangeCommand = new Command(CheckboxChanged);
        PickerChangedEventCommand = new Command(PickerChanged);
        RadioChangeCommand = new Command(RadioChanged);
        SubmitCommand = new Command(SubmitClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        TrueFalseChangeCommand = new Command(TrueFalseChanged);
        if (App.SurveyIdList == null)
            App.SurveyIdList = new List<int>();

        if (SubmittedSurveyIds == null)
            SubmittedSurveyIds = new List<int>();
        MessagingCenter.Subscribe<string>("", "SurveyRightSwipeSubscribe", (arg) =>
        {
            MessagingCenter.Subscribe<string>("", "SurveyRightSwipe", async (arguments) =>
            {
                //await SideMenuClicked();
            });
        });
    }

    #region Methods

    public void GetSurveyDetails()
    {
        try
        {
            if (UserSurvey != null && UserSurvey.SurveyQuestions != null && UserSurvey.SurveyQuestions.Count() > 0)
            {
                IsMandateSurvey = UserSurvey.SurveyQuestions.FirstOrDefault().IsRequired;
                MenuVisible = !IsMandateSurvey;
                App.IsMandateSurvey = IsMandateSurvey;

                if (IsMandateSurvey)
                    PageTitle = "    " + TextResource.SurveyPageTitle;
                else
                    PageTitle = TextResource.SurveyPageTitle;

                IsSettingMenuHidden = IsMandateSurvey;

                try
                {
                    var list = _mapper.Map<List<BindableSurveyQuestionAnswerView>>(UserSurvey.SurveyQuestions);

                    list.Where(x => x.IsLinkedQuestion == false).Select(y => y.IsVisible = true).ToList();

                    UserSurveyList = new ObservableCollection<BindableSurveyQuestionAnswerView>(list);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                FirstSurvey = UserSurveyList.FirstOrDefault();

                if (!App.SurveyIdList.Contains(FirstSurvey.SurveyId) && !FirstSurvey.IsRequired)
                    App.SurveyIdList.Add(FirstSurvey.SurveyId);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SubmitClicked(object obj)
    {
        try
        {
            IsError = false;
            var surveyAnswers = new List<SurveyAnswerView>();

            ValidationMessage = TextResource.SurveyValidationMsg;
            if (UserSurveyList != null && UserSurveyList.Count > 0)
            {
                foreach (var ques in UserSurveyList)
                {
                    var surveyAnswer = new SurveyAnswerView();
                    surveyAnswer.SurveyQuestionId = ques.SurveyQuestionId;

                    //surveyAnswer.SurveyChoiceAnswerId = null;
                    surveyAnswer.SurveyQuestionTypeId = ques.QuestionTypeId;
                    surveyAnswer.AnswerText = "";
                    // surveyAnswers.AddRange(Mapper.Map<List<SurveyAnswerView>>(ques.SurveyAnswerList));
                    if (ques.IsVisible)
                    {
                        if (ques.QuestionTypeId == (int)QuestionTypeEnum.Text)
                        {
                            if (string.IsNullOrEmpty(ques.AnswerText) && ques.IsRequiredQuestion)
                                IsError = true;

                            if (!IsError)
                            {
                                surveyAnswer.AnswerText = ques.AnswerText;
                                if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                                surveyAnswers.Add(surveyAnswer);
                            }
                        }

                        if (ques.QuestionTypeId == (int)QuestionTypeEnum.SingleAnswerchoice)
                        {
                            var selected = ques.SurveyAnswerList.Where(x => x.IsSelected == true).FirstOrDefault();
                            if (selected == null && ques.IsRequiredQuestion)
                                IsError = true;

                            if (!IsError)
                            {
                                surveyAnswer.AnswerText = selected?.SurveyChoiceAnswerId.ToString();
                                if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                                surveyAnswers.Add(surveyAnswer);
                            }
                        }

                        if (ques.QuestionTypeId == (int)QuestionTypeEnum.Select)
                        {
                            var selected = ques.SelecedSurveyAnswer;
                            if (selected == null && ques.IsRequiredQuestion)
                                IsError = true;

                            if (!IsError)
                            {
                                surveyAnswer.AnswerText = selected?.SurveyChoiceAnswerId.ToString();
                                if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                                surveyAnswers.Add(surveyAnswer);
                            }
                        }

                        if (ques.QuestionTypeId == (int)QuestionTypeEnum.TrueOrFalse)
                        {
                            if (!ques.IsTrue && !ques.IsFalse && ques.IsRequiredQuestion)
                                IsError = true;

                            if (!IsError)
                            {
                                surveyAnswer.AnswerText = ques.IsTrue.ToString();
                                if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                                surveyAnswers.Add(surveyAnswer);
                            }
                        }

                        if (ques.QuestionTypeId == (int)QuestionTypeEnum.MultipleAnswerschoice)
                        {
                            var selected = ques.SurveyAnswerList.Where(x => x.IsSelected == true).ToList();
                            if (selected == null || (selected.Count == 0 && ques.IsRequiredQuestion))
                                IsError = true;

                            if (!IsError)
                                foreach (var data in ques.SurveyAnswerList)
                                {
                                    var surveyAns = new SurveyAnswerView();
                                    surveyAns.SurveyQuestionId = ques.SurveyQuestionId;
                                    surveyAns.SurveyQuestionTypeId = ques.QuestionTypeId;
                                    surveyAns.AnswerText = data?.SurveyChoiceAnswerId.ToString();
                                    if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                                    surveyAnswers.Add(surveyAns);
                                }
                        }
                    }
                    else
                    {
                        if (surveyAnswer.AnswerText == null) surveyAnswer.AnswerText = "";
                        surveyAnswers.Add(surveyAnswer);
                    }
                }

                if (!IsError && surveyAnswers != null && surveyAnswers.Count() > 0)
                {
                    var json = JsonConvert.SerializeObject(surveyAnswers);
                    if (json != null)
                        await SubmitData(json);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private async Task SubmitData(string ansJson)
    {
        try
        {
            SurveyId = 0;
            var result = await ApiHelper.PostRequest<OperationDetails>(
                string.Format(TextResource.SurveySubmitApiUrl, FirstSurvey.SurveyId, UserSurvey.StudentId,
                    HttpUtility.UrlEncode(ansJson)), AppSettings.Current.ApiUrl);
            if (result != null && result.Success)
            {
                if (!SubmittedSurveyIds.Contains(FirstSurvey.SurveyId))
                    SubmittedSurveyIds.Add(FirstSurvey.SurveyId);

                await Application.Current.MainPage.DisplayAlert(TextResource.SurveyPageTitle,
                    TextResource.SurveySubmitMsg, TextResource.OkText);
                if (result.Output != null)
                {
                    SurveyId = FirstSurvey.SurveyId;
                    var json = JsonConvert.SerializeObject(result.Output);
                    UserSurvey = JsonConvert.DeserializeObject<UserSurveyView>(json);
                    NoRecordVisibility = false;
                    //GetSurveyDetails();
                }
                else if (IsFromMenu)
                {
                    foreach (var data in AppSettings.Current.SurveyViews)
                        if (!SubmittedSurveyIds.Contains(data.SurveyId))
                        {
                            SurveyId = data.SurveyId;
                            GetSurveyDetailsById();
                            return;
                        }
                }
                else
                {
                    HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                    {
                        MenuVisible = true
                    };
                    var homePage = new HomePage()
                    {
                        BindingContext = homeForm
                    };
                    await Navigation.PushAsync(homePage);
                }

                if (SurveyId > 0)
                {
                    HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                    {
                        MenuVisible = true
                    };
                    var homePage = new HomePage()
                    {
                        BindingContext = homeForm
                    };
                    await Navigation.PushAsync(homePage);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private void TrueFalseChanged(object obj)
    {
        if (obj != null)
        {
            var bindableSurvey = (BindableSurveyQuestionAnswerView)obj;
        }
    }

    private void CheckboxChanged(object obj)
    {
        try
        {
            if (obj != null)
            {
                var bindableSurvey = (BindableSurveyAnswerView)obj;
                var selectedQues = UserSurveyList.Where(x => x.SurveyQuestionId == bindableSurvey.SurveyQuestionId)
                    .FirstOrDefault();
                var selectedAnswers = selectedQues.SurveyAnswerList.Where(x => x.IsSelected).ToList();

                GetLinkedQuestion(bindableSurvey.SurveyQuestionId, UserSurveyList);
                foreach (var ans in selectedAnswers)
                    if (ans.HasLinkedQuestions)
                        UserSurveyList.Where(x => x.ParentSurveyChoiceAnswerId == ans.SurveyChoiceAnswerId).ToList()
                            .ForEach(x => x.IsVisible = true);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private void PickerChanged(object obj)
    {
        try
        {
            if (obj != null)
            {
                var bindableSurvey = (BindableSurveyQuestionAnswerView)obj;

                GetLinkedQuestion(bindableSurvey.SelecedSurveyAnswer.SurveyQuestionId, UserSurveyList);

                if (bindableSurvey.SelecedSurveyAnswer.HasLinkedQuestions)
                    UserSurveyList
                        .Where(x => x.ParentSurveyChoiceAnswerId ==
                                    bindableSurvey.SelecedSurveyAnswer.SurveyChoiceAnswerId).ToList()
                        .ForEach(x => x.IsVisible = true);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private void RadioChanged(object obj)
    {
        try
        {
            if (obj != null)
            {
                var bindableSurvey = (BindableSurveyAnswerView)obj;

                GetLinkedQuestion(bindableSurvey.SurveyQuestionId, UserSurveyList);

                if (bindableSurvey.HasLinkedQuestions)
                    UserSurveyList.Where(x => x.ParentSurveyChoiceAnswerId == bindableSurvey.SurveyChoiceAnswerId)
                        .ToList().ForEach(x => x.IsVisible = true);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private void CheckLinkedQuestions()
    {
    }

    public List<BindableSurveyQuestionAnswerView> GetLinkedQuestion(int questionId,
        ObservableCollection<BindableSurveyQuestionAnswerView> surveyQuestions)
    {
        var questionList = new List<BindableSurveyQuestionAnswerView>();
        if (UserSurveyList.Any())
        {
            UserSurveyList.Where(x => x.ParentQuestionId == questionId).ToList().ForEach(y => y.IsVisible = false);
            var linkedQuestions = UserSurveyList.Where(x => x.ParentQuestionId == questionId).ToList();
            questionList = linkedQuestions; // assign all top level questions
            foreach (var question in linkedQuestions)
            {
                question.IsVisible = false;
                if (question.SurveyAnswerList.Any(x => x.HasLinkedQuestions == true))
                {
                    var childLinkedQuestions = GetLinkedQuestion(question.SurveyQuestionId, UserSurveyList);
                    // if (childLinkedQuestions.Any())
                    // questionList.AddRange(childLinkedQuestions);
                }
            }
        }

        return questionList;
    }

    public async void GetAllPendingSurveys()
    {
        try
        {
            AppSettings.Current.SurveyViews = new List<SurveyView>();

            AppSettings.Current.SurveyViews =
                await ApiHelper.GetObjectList<SurveyView>(TextResource.GetPendingSurveysApiUrl);
            if (AppSettings.Current.SurveyViews != null && AppSettings.Current.SurveyViews.Count > 0)
                foreach (var data in AppSettings.Current.SurveyViews)
                    if (!SubmittedSurveyIds.Contains(data.SurveyId))
                    {
                        SurveyId = data.SurveyId;
                        GetSurveyDetailsById();
                        NoRecordVisibility = false;
                        return;
                    }
                    else
                    {
                        NoRecordVisibility = true;
                    }
            else
                NoRecordVisibility = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void GetSurveyDetailsById()
    {
        try
        {
            UserSurvey =
                await ApiHelper.GetObject<UserSurveyView>(string.Format(TextResource.SurveyDetailsApiUrl, SurveyId));
            if (UserSurvey != null && UserSurvey.SurveyQuestions != null && UserSurvey.SurveyQuestions.Count > 0)
            {
                GetSurveyDetails();
                NoRecordVisibility = false;
            }
            else
            {
                NoRecordVisibility = true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    #endregion
}