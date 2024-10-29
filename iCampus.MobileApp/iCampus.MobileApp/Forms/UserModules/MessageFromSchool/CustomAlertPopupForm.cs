using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Library.FormValidation;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class CustomAlertPopupForm : ViewModelBase
{
    private FeedBackAlertMessageView _customAlertObject = new();
    private ValidatableObject<string> _feedBackMessage;
    public ICommand AlertButtonOptionClicked { get; set; }
    public ICommand OnSaveButtonTapped { get; set; }
    public ICommand FeedBackTextChangedEventCommand { get; set; }
    private bool isFeedBackMessageValid;

    private bool _isFeedErrorLabelVisible;

    public bool IsFeedBackErrorLableVisible
    {
        get => _isFeedErrorLabelVisible;
        set
        {
            _isFeedErrorLabelVisible = value;
            OnPropertyChanged(nameof(IsFeedBackErrorLableVisible));
        }
    }

    private bool _isFromHomePage;

    public bool IsFromHomePage
    {
        get => _isFromHomePage;
        set
        {
            _isFromHomePage = value;
            OnPropertyChanged(nameof(IsFromHomePage));
        }
    }

    private bool _isCloseOption;

    public bool IsCloseOption
    {
        get => _isCloseOption;
        set
        {
            _isCloseOption = value;
            OnPropertyChanged(nameof(IsCloseOption));
        }
    }

    private Color _backGroundColor;

    public Color BackGroundColor
    {
        get
        {
            _backGroundColor = Colors.WhiteSmoke;
            return _backGroundColor;
        }
        set
        {
            _backGroundColor = value;
            OnPropertyChanged(nameof(BackGroundColor));
        }
    }

    private bool _showFeedBackField;

    public bool ShowFeedBackField
    {
        get => _showFeedBackField;
        set
        {
            _showFeedBackField = value;
            OnPropertyChanged(nameof(ShowFeedBackField));
        }
    }

    private int _selectedAlertButtonId;

    public int SelectedAlertButtonId
    {
        get => _selectedAlertButtonId;
        set
        {
            _selectedAlertButtonId = value;
            OnPropertyChanged(nameof(SelectedAlertButtonId));
        }
    }

    public ValidatableObject<string> FeedBackMessage
    {
        get => _feedBackMessage;
        set
        {
            _feedBackMessage = value;
            OnPropertyChanged(nameof(FeedBackMessage));
        }
    }

    public FeedBackAlertMessageView CustomAlertObject
    {
        get => _customAlertObject;
        set
        {
            _customAlertObject = value;
            OnPropertyChanged(nameof(CustomAlertObject));
        }
    }

    private int _listViewHeight;

    public int ListViewHeight
    {
        get => _listViewHeight;
        set
        {
            _listViewHeight = value;
            OnPropertyChanged(nameof(ListViewHeight));
        }
    }

    private string _titleColor;

    public string TitleColor
    {
        get => _titleColor;
        set
        {
            _titleColor = value;
            OnPropertyChanged(nameof(TitleColor));
        }
    }

    public CustomAlertPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        AlertButtonOptionClicked = new Command(CategoryButtonTapped);
        OnSaveButtonTapped = new Command(SaveUserCustomAlertFeedBack);
        FeedBackTextChangedEventCommand = new Command(ExecuteFeedBackTextChangedCommand);
        _feedBackMessage = new ValidatableObject<string>();
        FeedBackMessage.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = TextResource.PleaseEnterComment
        });
        CustomAlertPopup customAlertPopup = new()
        {
            BindingContext = this
        };
        SetPopupInstance(customAlertPopup);
        Application.Current.MainPage.ShowPopup(customAlertPopup);
    }

    private void ExecuteFeedBackTextChangedCommand(object obj)
    {
        isFeedBackMessageValid = FeedBackMessage.Validate();
        if (isFeedBackMessageValid)
            IsFeedBackErrorLableVisible = false;
        else
            IsFeedBackErrorLableVisible = true;
    }

    private void CategoryButtonTapped(object obj)
    {
        if (obj != null)
        {
            SelectedAlertButtonId = (int)obj;
            if (!CustomAlertObject.IsFeedback)
                SaveUserCustomAlertFeedBack();
            else
                ShowFeedBackField = true;
        }
    }

    private async void SaveUserCustomAlertFeedBack()
    {
        try
        {
            var isValid = ShowFeedBackField ? FeedBackMessage.Validate() : true;
            if (isValid && CustomAlertObject != null)
            {
                IsFeedBackErrorLableVisible = false;
                var param = "?buttonId=" + SelectedAlertButtonId + "&customAlertsId=" +
                            CustomAlertObject.CustomAlertsId + "&feedbackData=" +
                            (CustomAlertObject.IsFeedback ? FeedBackMessage.Value : string.Empty);
                var result = await ApiHelper.PostRequest<OperationDetails>(TextResource.UpdateCustomAlertApiUrl + param,
                    AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    if (AppSettings.Current.CurrentPopup is CustomAlertPopup)
                    {
                        AppSettings.Current.CurrentPopup.Close();
                        if (IsFromHomePage)
                            MessagingCenter.Send(CustomAlertObject, "HomeCustomAlertSavedSuccess");
                        else
                            MessagingCenter.Send(CustomAlertObject, "CustomAlertSavedSuccess");
                        CustomAlertObject = null;
                        return;
                    }

                    if (App.CustomAlertIdList.Contains(CustomAlertObject.CustomAlertsId))
                        App.CustomAlertIdList.Remove(CustomAlertObject.CustomAlertsId);
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new ExceptionAlertPopup(result.Message));
                }
            }
            else
            {
                IsFeedBackErrorLableVisible = true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }
}