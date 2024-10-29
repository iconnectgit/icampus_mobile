using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Registration;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class UpdateFamilyDetailsForm : ViewModelBase
	{
        #region Declaration
        public ICommand FatherCommand { get; set; }
        public ICommand MotherCommand { get; set; }
        public ICommand GeneralCommand { get; set; }
        public ICommand FatherAttachmentsCommand { get; set; }
        public ICommand MotherAttachmentsCommand { get; set; }
        #endregion

        #region Properties
       
        #endregion
        public UpdateFamilyDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Methods
        private async void InitializePage()
        {
            FatherCommand = new Command(FatherCommandMethod);
            MotherCommand = new Command(MotherCommandMethod);
            GeneralCommand = new Command(GeneralCommandMethod);
            FatherAttachmentsCommand = new Command(FatherAttachmentsMethod);
            MotherAttachmentsCommand = new Command(MotherAttachmentsMethod);

        }

        
        public async Task GetRegistrationFormFamilyDetails()
        {
            try
            {
                var familyDetails = await ApiHelper.GetObject<RegistrationFamilyDetailView>(string.Format(TextResource.GetRegistrationFormFamilyDetailsApi, ""));
                AppSettings.Current.FamilyDetails = new ObservableCollection<BindableFormFieldsView>(_mapper.Map<IEnumerable<BindableFormFieldsView>>(familyDetails.FormFields));

                foreach (var details in AppSettings.Current.FamilyDetails)
                {
                    details.AttachmentFiles = new ObservableCollection<AttachmentFileView>(
                        familyDetails.FileList
                        .Where(x => x.CategoryId == details.ExternalCategoryId)
                        .Select(a => new AttachmentFileView
                        {
                            FileName = a.UploadedFileName,
                        }));
                    details.AttachmentListViewHeight = details.AttachmentFiles.Count * 40;
                }
                AppSettings.Current.FormData = familyDetails.FormData;
                AppSettings.Current.FamilyId = familyDetails.ExistingOrNewFamilyId;
                AppSettings.Current.ExistingAttachmentDetail = new ObservableCollection<FileDataView>(familyDetails.FileList);

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void FatherCommandMethod(object obj)
        {
            try
            {
                FatherDetailsForm fatherDetailsForm = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Father Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                FatherDetailsPage fatherDetailsPage = new ()
                {
                    BindingContext = fatherDetailsForm
                };
                await Navigation.PushAsync(fatherDetailsPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void MotherCommandMethod(object obj)
        {
            try
            {
                MotherDetailsForm motherDetails = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Mother Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                MotherDetailsPage motherDetailsPage = new ()
                {
                    BindingContext = motherDetails
                };
                await Navigation.PushAsync(motherDetailsPage);            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void GeneralCommandMethod(object obj)
        {
            try
            {
                GeneralDetailsForm generalDetails = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "General Details",
                    MenuVisible = false,
                    BackVisible = true
                };
                GeneralDetailsPage fatherDetailsPage = new ()
                {
                    BindingContext = generalDetails
                };
                await Navigation.PushAsync(fatherDetailsPage);            
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void FatherAttachmentsMethod(object obj)
        {
            try
            {
                FatherAttachmentsForm fatherAttachments = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Father Attachments",
                    MenuVisible = false,
                    BackVisible = true
                };
                FatherAttachmentsPage fatherAttachmentsPage = new ()
                {
                    BindingContext = fatherAttachments
                };
                await Navigation.PushAsync(fatherAttachmentsPage);   
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void MotherAttachmentsMethod(object obj)
        {
            try
            {
                MotherAttachmentsForm motherAttachments = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Mother Attachments",
                    MenuVisible = false,
                    BackVisible = true
                };
                MotherAttachmentsPage motherAttachmentsPage = new ()
                {
                    BindingContext = motherAttachments
                };
                await Navigation.PushAsync(motherAttachmentsPage);   
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        #endregion
    }