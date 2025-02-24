using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.UserModules.DataCollection;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.DataCollection;

public class DataCollectionForm : ViewModelBase
{
    #region Declarations

    public ICommand PickerChangedEventCommand { get; set; }
    public ICommand SubmitCommand { get; set; }
    public ICommand DeleteAttachmentClickCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }
    public ICommand AddAttachmentClickCommand { get; set; }
    public ICommand AttachmentListTappedCommand { get; set; }

    #endregion

    #region Properties

    private List<string> deletedAttachmentFiles = new();

    private DataCollectionView _dataCollection;

    public DataCollectionView DataCollection
    {
        get => _dataCollection;
        set
        {
            _dataCollection = value;
            OnPropertyChanged(nameof(DataCollection));
        }
    }

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

    private ObservableCollection<BindableDataCollectionFieldsView> _dataCollectionFieldList;

    public ObservableCollection<BindableDataCollectionFieldsView> DataCollectionFieldList
    {
        get => _dataCollectionFieldList;
        set
        {
            _dataCollectionFieldList = value;
            OnPropertyChanged(nameof(DataCollectionFieldList));
        }
    }

    private BindableDataCollectionFormView _activeCollectionForm = new();

    public BindableDataCollectionFormView ActiveCollectionForm
    {
        get => _activeCollectionForm;
        set
        {
            _activeCollectionForm = value;
            OnPropertyChanged(nameof(ActiveCollectionForm));
        }
    }

    //IList<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
    //public IList<AttachmentFileView> AttachmentFiles
    //{
    //    get => _attachmentFiles;
    //    set => this.RaiseAndSetIfChanged(ref _attachmentFiles, value);
    //}

    public int PendingFormId { get; set; }

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

    public bool IsFromMenu { get; set; }

    public static List<string> SubmittedFormIds { get; set; }

    #endregion

    public DataCollectionForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper,
        null, null)
    {
        HelperMethods.GetSelectedStudent();
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        if (AppSettings.Current.DataCollectionFormIdList == null)
            AppSettings.Current.DataCollectionFormIdList = new List<string>();

        if (SubmittedFormIds == null)
            SubmittedFormIds = new List<string>();

        SubmitCommand = new Command(SubmitClicked);
        DeleteAttachmentClickCommand = new Command<BindableAttachmentFileView>(DeleteAttachmentClicked);
        DownloadTappedCommand = new Command<BindableAttachmentFileView>(DownloadClicked);
        AddAttachmentClickCommand = new Command<BindableDataCollectionFieldsView>(AddAttachmentClicked);
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    #region methods

    private async void AddAttachmentClicked(BindableDataCollectionFieldsView obj)
    {
        try
        {
            var fileData = obj.FieldTypeId == (int)DataCollectionFieldTypeEnum.ImageUploader
                ? await HelperMethods.PickImageFromDevice()
                : await HelperMethods.PickFileFromDevice();
            if (fileData != null && fileData.FilePath != null)
            {
                var bindableAttachmentFileView = _mapper.Map<BindableAttachmentFileView>(fileData);
                bindableAttachmentFileView.ParentFieldId = obj.DataCollectionFieldId;
                bindableAttachmentFileView.IsNewFile = true;
                DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.DataCollectionFieldId)
                    .FirstOrDefault()?.AttachmentList?.Add(bindableAttachmentFileView);
                if (DataCollectionFieldList != null && DataCollectionFieldList.Count() > 0 && DataCollectionFieldList
                        .Where(x => x.DataCollectionFieldId == obj.DataCollectionFieldId)?.FirstOrDefault()
                        ?.AttachmentList != null)
                {
                    var count = DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.DataCollectionFieldId)
                        .FirstOrDefault().AttachmentList.Count();
                    DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.DataCollectionFieldId)
                        .FirstOrDefault().AttachmentListHeight = count * 50;
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
        // AttachmentFiles.AddFileToList(fileData);
    }

    private async void AttachmentListClicked(object obj)
    {
        if (obj != null)
            try
            {
                var selectedAttachment = (BindableAttachmentFileView)obj;
                if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                    await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DownloadClicked(BindableAttachmentFileView obj)
    {
        if (obj != null)
            try
            {
                var selectedAttachment = obj;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                else
                {
                    var selectedCollection = DataCollectionFieldList
                        .Where(x => x.DataCollectionFieldId == selectedAttachment.ParentFieldId).FirstOrDefault();
                    if (selectedCollection?.AttachmentList?.Where(x => x.FileName == selectedAttachment.FileName)
                            ?.FirstOrDefault() != null && selectedCollection.AttachmentList
                            .Where(x => x.FileName == selectedAttachment.FileName).FirstOrDefault().FileStatus == 0)
                    {
                        selectedCollection.AttachmentList.Where(x => x.FileName == selectedAttachment.FileName)
                            .FirstOrDefault().FileStatus = 1;
                        var filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            selectedCollection.AttachmentList.Where(x => x.FileName == selectedAttachment.FileName)
                                .FirstOrDefault().FileDevicePath = filePath;
                            selectedCollection.AttachmentList.Where(x => x.FileName == selectedAttachment.FileName)
                                .FirstOrDefault().FileStatus = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DeleteAttachmentClicked(BindableAttachmentFileView obj)
    {
        try
        {
            if (obj != null)
            {
                var action = await Application.Current.MainPage.DisplayAlert("", TextResource.DeleteText,
                    TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var attachmentFile = obj;
                    if (attachmentFile != null)
                    {
                        if (!attachmentFile.IsNewFile) deletedAttachmentFiles.Add(attachmentFile.FileName);
                        DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.ParentFieldId)
                            .FirstOrDefault()?.AttachmentList?.Remove(attachmentFile);
                        if (DataCollectionFieldList != null && DataCollectionFieldList.Count() > 0 &&
                            DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.ParentFieldId)
                                ?.FirstOrDefault()?.AttachmentList != null)
                        {
                            var count = DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.ParentFieldId)
                                .FirstOrDefault().AttachmentList.Count();
                            DataCollectionFieldList.Where(x => x.DataCollectionFieldId == obj.ParentFieldId)
                                .FirstOrDefault().AttachmentListHeight = count * 50;
                        }
                    }
                }
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
            var UAEPhoneNumberRegex = "^((05)[0-9]{8})$";
            var OmanPhoneNumberRegex = "^([0-9]{10})$";
            ValidationMessage = TextResource.SurveyValidationMsg;


            if (DataCollectionFieldList != null && DataCollectionFieldList.Count > 0)
            {
                PendingFormId = ActiveCollectionForm.FormId;
                foreach (var field in DataCollectionFieldList)
                {
                    if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.UAEPhoneNumber)
                    {
                        if (!string.IsNullOrEmpty(field.FieldValue) || field.IsRequired)
                        {
                            var contact = "05" + field.FieldValue;
                            IsError = !Regex.IsMatch(contact, UAEPhoneNumberRegex);
                            if (IsError) ValidationMessage = TextResource.PhoneValidationMsg;
                        }
                    }
                    else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.OmanPhoneNumber)
                    {
                        if (!string.IsNullOrEmpty(field.FieldValue) || field.IsRequired)
                        {
                            IsError = !Regex.IsMatch(field.FieldValue, OmanPhoneNumberRegex);
                            if (IsError) ValidationMessage = TextResource.PhoneValidationMsg;
                        }
                    }
                    else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.Email)
                    {
                        if (!string.IsNullOrEmpty(field.FieldValue) || field.IsRequired)
                        {
                            IsError = !HelperMethods.IsValidEmail(field.FieldValue);
                            if (IsError) ValidationMessage = TextResource.EmailValidationMsg;
                        }
                    }

                    if (field.IsRequired)
                    {
                        if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.FileInput)
                            IsError = field.AttachmentList.Count == 0;
                        else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.ImageUploader)
                            IsError = field.AttachmentList.Count == 0;
                        else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.Text)
                            IsError = string.IsNullOrEmpty(field.FieldValue);
                        else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.Date)
                            IsError = string.IsNullOrEmpty(field.FieldValue);
                        else if (field.FieldTypeId == (int)DataCollectionFieldTypeEnum.Select)
                            IsError = string.IsNullOrEmpty(field.FieldValue);
                    }

                    if (IsError)
                        break;
                }

                if (!IsError && DataCollectionFieldList != null && DataCollectionFieldList.Count() > 0)
                {
                    var attachmentDetail = new List<AttachmentFileView>();
                    var existingAttachmentDetail = new List<AttachmentFileView>();
                    var attachmentFiles = new List<AttachmentFileView>();
                    foreach (var data in DataCollectionFieldList.Where(x =>
                                 x.FieldTypeId == (int)DataCollectionFieldTypeEnum.UAEPhoneNumber))
                        data.FieldValue = "05" + data.FieldValue;
                    foreach (var data in DataCollectionFieldList.Where(x =>
                                 x.FieldTypeId == (int)DataCollectionFieldTypeEnum.FileInput ||
                                 x.FieldTypeId == (int)DataCollectionFieldTypeEnum.ImageUploader))
                    {
                        var tempAttachmentDetail = new List<AttachmentFileView>();
                        var tempExistingAttachmentDetail = new List<AttachmentFileView>();
                        var fieldValueStringList = new List<string>();
                        if (data.AttachmentList != null && data.AttachmentList.Count > 0)
                        {
                            attachmentFiles.AddRange(_mapper.Map<List<AttachmentFileView>>(data.AttachmentList));
                            foreach (var item in data.AttachmentList)
                                if (item.IsNewFile)
                                    tempAttachmentDetail.Add(new AttachmentFileView { FileName = item.FileName });
                                else
                                    tempExistingAttachmentDetail.Add(
                                        new AttachmentFileView { FileName = item.FileName });
                            if (tempAttachmentDetail != null && tempAttachmentDetail.Count > 0)
                                foreach (var attachmentDetailItem in tempAttachmentDetail)
                                    if (attachmentDetailItem != null)
                                    {
                                        fieldValueStringList.Add(attachmentDetailItem.FileName);
                                        data.FieldValue = attachmentDetailItem.FileName;
                                    }

                            if (tempExistingAttachmentDetail != null && tempExistingAttachmentDetail.Count > 0)
                                foreach (var existingAttachmentDetailItem in tempExistingAttachmentDetail)
                                    if (existingAttachmentDetailItem != null)
                                        fieldValueStringList.Add(existingAttachmentDetailItem.FileName);
                            data.FieldValue = string.Join(":", fieldValueStringList);
                        }

                        if (tempAttachmentDetail != null)
                            foreach (var attachment in tempAttachmentDetail)
                                attachmentDetail.Add(attachment);
                        if (tempExistingAttachmentDetail != null)
                            foreach (var attachment in tempExistingAttachmentDetail)
                                existingAttachmentDetail.Add(attachment);
                    }

                    var json = JsonConvert.SerializeObject(DataCollectionFieldList);
                    var attachmentDetailStr = JsonConvert.SerializeObject(attachmentDetail);
                    var existingAttachmentDetailStr = JsonConvert.SerializeObject(existingAttachmentDetail);
                    var deletedAttachmentFilesStr = string.Join(",", deletedAttachmentFiles);
                    var objDataCollection =
                        JsonConvert.DeserializeObject<ObservableCollection<DataCollectionFieldsView>>(json);

                    foreach (var data in objDataCollection.Where(x =>
                                 x.FieldTypeId == (int)DataCollectionFieldTypeEnum.FileInput ||
                                 x.FieldTypeId == (int)DataCollectionFieldTypeEnum.ImageUploader))
                        data.AttachmentList = new List<AttachmentFileView>();
                    var studentIdToPass = ActiveCollectionForm.FillDataPerFamily
                        ? (int?)null
                        : ActiveCollectionForm.StudentId;
                    var ansjson = JsonConvert.SerializeObject(objDataCollection);
                    if (ansjson != null)
                        await SubmitData(PendingFormId, ansjson, studentIdToPass, attachmentDetailStr,
                            existingAttachmentDetailStr, deletedAttachmentFilesStr, attachmentFiles);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public async void AssignFormData(DataCollectionFormView dataForm,
        IEnumerable<DataCollectionFieldsView> dataCollectionFields)
    {
        if (dataCollectionFields != null && dataCollectionFields.Count() > 0)
        {
            ActiveCollectionForm = _mapper.Map<BindableDataCollectionFormView>(dataForm);
            DataCollectionFieldList =
                new ObservableCollection<BindableDataCollectionFieldsView>(
                    _mapper.Map<List<BindableDataCollectionFieldsView>>(dataCollectionFields));
            if (DataCollectionFieldList != null)
                DataCollectionFieldList.ToList().Where(x => x.FieldTitle.ToLower().Equals("date")).ToList()
                    .ForEach(x => x.FieldValue = DateTime.Now.ToString());
            PendingFormId = ActiveCollectionForm.FormId;
            if (DataCollectionFieldList != null && DataCollectionFieldList.Count > 0)
            {
                NoRecordVisibility = false;
                foreach (var formData in DataCollectionFieldList)
                {
                    if (formData.FieldTypeId == (int)DataCollectionFieldTypeEnum.Select)
                    {
                        var output = formData.FieldOption.Split(
                            new[] { Environment.NewLine },
                            StringSplitOptions.None
                        );

                        formData.SelectAnswerList = output.ToList();
                    }

                    if (formData.FieldTypeId == (int)DataCollectionFieldTypeEnum.UAEPhoneNumber)
                        if (!string.IsNullOrEmpty(formData.FieldValue) && formData.FieldValue.Contains("05") &&
                            formData.FieldValue.Length > 8)
                            formData.FieldValue = formData.FieldValue.Substring(2, formData.FieldValue.Length - 2);
                    if (formData.FieldTypeId == (int)DataCollectionFieldTypeEnum.FileInput &&
                        formData.AttachmentList != null && formData.AttachmentList.Count() > 0)
                        foreach (var attachment in formData.AttachmentList)
                            attachment.ParentFieldId = formData.DataCollectionFieldId;
                    if (formData.FieldTypeId == (int)DataCollectionFieldTypeEnum.ImageUploader &&
                        formData.AttachmentList != null && formData.AttachmentList.Count() > 0)
                        foreach (var attachment in formData.AttachmentList)
                            attachment.ParentFieldId = formData.DataCollectionFieldId;
                }

                App.IsMandateDataCollection = ActiveCollectionForm.IsRequired;
                MenuVisible = !ActiveCollectionForm.IsRequired;
                if (ActiveCollectionForm.IsRequired)
                    PageTitle = "    " + TextResource.DataCollectionPageTitle;
                else
                    PageTitle = TextResource.DataCollectionPageTitle;

                IsSettingMenuHidden = ActiveCollectionForm.IsRequired;
            }
            else
            {
                App.IsMandateDataCollection = false;
                NoRecordVisibility = true;
                ActiveCollectionForm = new BindableDataCollectionFormView();
                DataCollectionFieldList = new ObservableCollection<BindableDataCollectionFieldsView>();
                HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                {
                    MenuVisible = true
                };
                HomePage homePage = new()
                {
                    BindingContext = homeForm
                };
                await Navigation.PushAsync(homePage);
            }
        }
    }

    private async Task SubmitData(int formId, string ansJson, int? studentId, string attachmentDetailStr,
        string existingAttachmentDetailStr, string deletedAttachmentFilesStr, List<AttachmentFileView> AttachmentFiles)
    {
        try
        {
            var result = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(
                string.Format(TextResource.SaveDataCollectionApiUrl, formId, ansJson, studentId, attachmentDetailStr,
                    existingAttachmentDetailStr, deletedAttachmentFilesStr), AppSettings.Current.ApiUrl, null,
                AttachmentFiles);
            if (result != null && result.Success)
            {
                if (!SubmittedFormIds.Contains(string.Format("{0}_{1}", PendingFormId, studentId)))
                    SubmittedFormIds.Add(string.Format("{0}_{1}", PendingFormId, studentId));

                await Application.Current.MainPage.DisplayAlert(TextResource.DataCollectionPageTitle,
                    TextResource.DataCollectionSubmitMsg, TextResource.OkText);
                if (result.Output != null)
                {
                    var json = JsonConvert.SerializeObject(result.Output);
                    var data = JsonConvert.DeserializeObject<DataCollectionView>(json);
                    if (data.DataCollectionFormFields != null && data.DataCollectionFormFields.Count() > 0)
                    {
                        AssignFormData(data.ActiveDataCollectionForm, data.DataCollectionFormFields);
                        NoRecordVisibility = false;
                    }
                    else
                    {
                        NoRecordVisibility = true;
                        if (IsFromMenu)
                        {
                            DataCollectionMainForm dataCollectionMainForm = new(_mapper, _nativeServices, Navigation)
                            {
                                PageTitle = TextResource.DataCollectionPageTitle,
                                MenuVisible = true
                            };
                            DataCollectionMainPage dataCollectionMainPage = new ()
                            {
                                BindingContext = dataCollectionMainForm
                            };
                            await Navigation.PushAsync(dataCollectionMainPage);
                        }
                        else
                        {
                            HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                            {
                                MenuVisible = true
                            };
                            HomePage homePage = new()
                            {
                                BindingContext = homeForm
                            };
                            await Navigation.PushAsync(homePage);
                        }
                    }
                }
                else if (IsFromMenu)
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                    {
                        MenuVisible = true
                    };
                    HomePage homePage = new()
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


    public async void GetAllDataCollection()
    {
        try
        {
            _dataCollectionFieldList = new ObservableCollection<BindableDataCollectionFieldsView>();

            DataCollection = await ApiHelper.GetObject<DataCollectionView>(TextResource.GetPendingDataCollectionApiUrl);

            GetPendingDataCollection();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    public void GetPendingDataCollection()
    {
        foreach (var form in DataCollection.DataCollectionForms.OrderByDescending(f => f.IsRequired)
                     .ThenBy(f => f.StudentName))
            if (form.IsRequired)
            {
                PendingFormId = form.FormId;
                break;
            }
            else if (!AppSettings.Current.DataCollectionFormIdList.Contains(string.Format("{0}_{1}", PendingFormId,
                         form.StudentId)))
            {
                PendingFormId = form.FormId;
                AppSettings.Current.DataCollectionFormIdList.Add(
                    string.Format("{0}_{1}", PendingFormId, form.StudentId));
                break;
            }

        if (PendingFormId > 0 && DataCollection.DataCollectionFormFields.Any())
        {
            var model = new DataCollectionView();
            var dataCollectionForm = DataCollection.DataCollectionForms.Where(f => f.FormId == PendingFormId)
                .FirstOrDefault();
            ActiveCollectionForm = _mapper.Map<BindableDataCollectionFormView>(dataCollectionForm);
            var FormFields = DataCollection.DataCollectionFormFields.Where(f => f.FormId == PendingFormId);

            AssignFormData(dataCollectionForm, FormFields);
        }
        else
        {
            NoRecordVisibility = true;
            ActiveCollectionForm = new BindableDataCollectionFormView();
            DataCollectionFieldList = new ObservableCollection<BindableDataCollectionFieldsView>();
        }
    }

    #endregion
}