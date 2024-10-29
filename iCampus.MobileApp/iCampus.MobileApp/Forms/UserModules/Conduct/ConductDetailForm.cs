using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Conduct;

public class ConductDetailForm : ViewModelBase
{
       
    #region Properties
    StudentConductView _selectedConduct = new StudentConductView();
    public StudentConductView SelectedConduct
    {
        get => _selectedConduct;
        set
        {
            _selectedConduct = value;
            OnPropertyChanged(nameof(SelectedConduct));
        }
    }

    #endregion

    public ConductDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        BackVisible = true;
    }
}