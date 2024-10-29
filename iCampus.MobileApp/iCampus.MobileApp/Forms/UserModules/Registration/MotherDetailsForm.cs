using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class MotherDetailsForm : ViewModelBase
	{
        #region Declaration
        #endregion
        #region Properties
        #endregion
        public MotherDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
	        _mapper = mapper;
	        _nativeServices = nativeServices;
	        Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
        }
        #endregion
    }