using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.Calendar;

namespace iCampus.MobileApp.Forms.UserModules.Event;

public class EventFilterForm : CalendarForm
{
    #region Declarations

    #endregion
    public EventFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;

    }

    #region Private methods


    #endregion
}