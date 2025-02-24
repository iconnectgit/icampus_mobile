using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Views.UserModules.Conduct;
using iCampus.Portal.ViewModels;
using Microsoft.Maui.Controls;

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
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }
    
    public override async void GetStudentData()
    {
        ConductForm conductForm = new(_mapper, _nativeServices, Navigation, true)
        {
            PageTitle = PageTitle,
            MenuVisible = true
        };
        AppSettings.Current.IsDisplayAllStudentList = false;
        conductForm.OpenStudentSelection();
        ConductPage conductPage = new()
        {
            BindingContext = conductForm
        };
        await Navigation.PushAsync(conductPage);
    }

}