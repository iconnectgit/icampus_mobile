using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Registration;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class AddNewChildForm : ViewModelBase
	{
        #region Declaration
        public ICommand GeneralDetailsCommand { get; set; }
        public ICommand AcademicsDetailsCommand { get; set; }
        public ICommand TransportDetailsCommand { get; set; }
        public ICommand MedicalDetailsCommand { get; set; }
        public ICommand OtherDetailsCommand { get; set; }
        public ICommand AttachmentsCommand { get; set; }
        #endregion

        #region Properties
        #endregion
        public AddNewChildForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Methods

        private async void InitializePage()
        {
            GeneralDetailsCommand = new Command(GeneralDetailsMethod);
            AcademicsDetailsCommand = new Command(AcademicsDetailsMethod);
            TransportDetailsCommand = new Command(TransportDetailsMethod);
            MedicalDetailsCommand = new Command(MedicalDetailsMethod);
            OtherDetailsCommand = new Command(OtherDetailsMethod);
            AttachmentsCommand = new Command(AttachmentsMethod);
        }

        public async Task GetRegistrationFormStudentDetails(int registrationId, bool? isCurrentYear)
        {
            try
            {
                string birthDate = string.Empty;
                string genderId = string.Empty;
                var studentDetails = await ApiHelper.GetObject<RegistrationStudentView>(string.Format(TextResource.GetRegistrationFormStudentDetailsApi, registrationId, isCurrentYear));
                if(string.IsNullOrEmpty(studentDetails.ErrorMessage))
                {
                    AppSettings.Current.StudentDetails = new ObservableCollection<BindableFormFieldsView>(_mapper.Map<IEnumerable<BindableFormFieldsView>>(studentDetails.FormFields));

                    foreach (var details in AppSettings.Current.StudentDetails)
                    {
                        details.AttachmentFiles = new ObservableCollection<AttachmentFileView>(
                            studentDetails.FileList
                            .Where(x => x.CategoryId == details.ExternalCategoryId)
                            .Select(a => new AttachmentFileView
                            {
                                FileName = a.UploadedFileName,
                            }));
                        details.AttachmentListViewHeight = details.AttachmentFiles.Count * 40;
                        if (details.FieldName.ToLower() == "birthdate")
                        {
                            birthDate = Convert.ToString(details.FieldValue);
                        }
                        if (details.FieldName.ToLower() == "gender")
                        {
                            genderId = Convert.ToString(details.FieldValue);
                        }

                    }
                    if(registrationId > 0)
                    {
                        string portalUrl = AppSettings.Current.PortalUrl;
                        AppSettings.Current.GradeUrl = $"Users/Registration/GetRegistrationGradesByAge?birthDate={birthDate}&genderId={genderId}&academicYear={AppSettings.Current.AcademicYear}";
                        var gradeList = new ObservableCollection<SelectionListView>(await ApiHelper.GetObjectList<SelectionListView>(AppSettings.Current.GradeUrl,apiUrl: portalUrl));

                        var gradeDetail = AppSettings.Current.StudentDetails.FirstOrDefault(details => details.DataFieldName.Equals("gradeid", StringComparison.OrdinalIgnoreCase));

                        if (gradeDetail != null)
                        {
                            gradeDetail.FieldText = gradeList.FirstOrDefault(x => x.ItemId == Convert.ToInt16(gradeDetail.FieldValue))?.ItemName;
                        }
                    }
                    

                    AppSettings.Current.RegistrationId = registrationId;
                    AppSettings.Current.FormData = studentDetails.FormData;
                    AppSettings.Current.FamilyId = studentDetails.ExistingOrNewFamilyId;
                    AppSettings.Current.ExistingAttachmentDetail = new ObservableCollection<FileDataView>(studentDetails.FileList);
                    
                }
                else
                {
                    await HelperMethods.ShowAlert("", studentDetails.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void GeneralDetailsMethod(object obj)
        {
            try
            {
                AddNewChildGeneralForm addNewChildGeneralForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "General Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildGeneralPage addNewChildGeneralPage = new ()
                {
                    BindingContext = addNewChildGeneralForm
                };
                await Navigation.PushAsync(addNewChildGeneralPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void AcademicsDetailsMethod(object obj)
        {
            try
            {
                AddNewChildAcademicsForm addNewChildAcademicsForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Academics Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildAcademicsPage addNewChildAcademicsPage = new AddNewChildAcademicsPage()
                {
                    BindingContext = addNewChildAcademicsForm
                };
                await Navigation.PushAsync(addNewChildAcademicsPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void TransportDetailsMethod(object obj)
        {
            try
            {
                AddNewChildTransportForm addNewChildTransport = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Transport Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildTransportPage addNewChildTransportPage = new ()
                {
                    BindingContext = addNewChildTransport
                };
                await Navigation.PushAsync(addNewChildTransportPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void MedicalDetailsMethod(object obj)
        {
            try
            {
                AddNewChildMedicalForm addNewChildMedicalForm = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Medical Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildMedicalPage addNewChildMedicalPage = new ()
                {
                    BindingContext = addNewChildMedicalForm
                };
                await Navigation.PushAsync(addNewChildMedicalPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void OtherDetailsMethod(object obj)
        {
            try
            {
                AddNewChildOtherForm addNewChildOtherForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Other Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildOtherPage addNewChildOtherPage = new ()
                {
                    BindingContext = addNewChildOtherForm
                };
                await Navigation.PushAsync(addNewChildOtherPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void AttachmentsMethod(object obj)
        {
            try
            {
                AddNewChildAttachmentsForm addNewChildAttachmentsForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Attachments",
                    MenuVisible = false,
                    BackVisible = true
                };
                AddNewChildAttachmentsPage addNewChildAttachmentsPage = new ()
                {
                    BindingContext = addNewChildAttachmentsForm
                };
                await Navigation.PushAsync(addNewChildAttachmentsPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }