using iCampus.Common.ViewModels;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp;
using AutoMapper;
public class MyMappingProfile : Profile
{
    public MyMappingProfile()
    {
        CreateMap<StudentPickListItem, BindableStudentPickListItem>().ReverseMap();
        CreateMap<ModuleStructureView, BindableModuleStructureView>().ReverseMap();
        // Define your mappings here
        // CreateMap<SourceType, DestinationType>();
    }
}