using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Registration;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class KGInformationForm : ViewModelBase
	{
        #region Declaration
        public ICommand GeneralInformationCommand { get; set; }
        public ICommand FamilySummaryCommand { get; set; }
        public ICommand InformationforKGCommand { get; set; }
       
        #endregion

        #region Properties
        #endregion
        public KGInformationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Methods

        private async void InitializePage()
        {
            GeneralInformationCommand = new Command(GeneralInformationMethod);
            FamilySummaryCommand = new Command(FamilySummaryMethod);
            InformationforKGCommand = new Command(InformationforKGMethod);
        }

        public async Task GetKGInformationDetails(int registrationId)
        {
            try
            {
                var studentDetails = await ApiHelper.GetObject<RegistrationStudentView>(string.Format(TextResource.GetRegistrationFormKgStudentInformationApi, registrationId));
                AppSettings.Current.KGStudentDetails = new ObservableCollection<BindableFormFieldsView>(_mapper.Map<IEnumerable<BindableFormFieldsView>>(studentDetails.FormFields));

                AppSettings.Current.RegistrationId = Convert.ToInt16(studentDetails.FormData[0].FirstOrDefault(x => x.Key == "RegistrationKgInfoId").Value);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void GeneralInformationMethod(object obj)
        {
            try
            {
                KGGeneralInformationForm kGGeneralInformation = new (_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = "General Information",
                        MenuVisible = false,
                        BackVisible = true
                    };
                Views.UserModules.Registration.KGGeneralInformationPage kgGeneralInformationPage = new ()
                {
                    BindingContext = kGGeneralInformation
                };
                await Navigation.PushAsync(kgGeneralInformationPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void FamilySummaryMethod(object obj)
        {
            try
            {
                KGFamilySummaryForm kgFamilySummaryForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Family Summary",
                    MenuVisible = false,
                    BackVisible = true
                };
                KGFamilySummaryPage kgFamilySummaryPage = new ()
                {
                    BindingContext = kgFamilySummaryForm
                };
                await Navigation.PushAsync(kgFamilySummaryPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void InformationforKGMethod(object obj)
        {
            try
            {
                KGInformationforKGForm kgInformationforKgForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Information for KG",
                    MenuVisible = false,
                    BackVisible = true
                };   
                KGInformationforKGPage kgInformationforKgPage = new ()
                {
                    BindingContext = kgInformationforKgForm
                };
                await Navigation.PushAsync(kgInformationforKgPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        
        #endregion
    }