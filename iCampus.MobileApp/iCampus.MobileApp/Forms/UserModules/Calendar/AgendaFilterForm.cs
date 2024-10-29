using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class AgendaFilterForm : CalendarForm
{
    #region Declarations

    #endregion
    public AgendaFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;

    }

    #region Private methods


    #endregion
}