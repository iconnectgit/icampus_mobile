using System.Collections.ObjectModel;
using System.Globalization;
using Akavache;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules;
using iCampus.MobileApp.Forms.UserModules.Attendance;
using iCampus.MobileApp.Forms.UserModules.BooksReservation;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Forms.UserModules.Communication;
using iCampus.MobileApp.Forms.UserModules.DailyMarks;
using iCampus.MobileApp.Forms.UserModules.DataCollection;
using iCampus.MobileApp.Forms.UserModules.Exam;
using iCampus.MobileApp.Forms.UserModules.FinancialStatus;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Forms.UserModules.Resources;
using iCampus.MobileApp.Forms.UserModules.Survey;
using iCampus.MobileApp.Handlers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;
using Microcharts.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.LifecycleEvents;
using Newtonsoft.Json;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Splat;

namespace iCampus.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                fonts.AddFont("fontawesome-webfont.ttf", "FontAwesomeClassic");
                fonts.AddFont("Montserrat-SemiBold.ttf", "MontserratSemiBold");
                fonts.AddFont("Montserrat-Light.ttf", "MontserratLight");
                fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
                fonts.AddFont("Montserrat-Regular.ttf", "MontserratRegular");
            })
            .UseMauiCommunityToolkit()
            .UseFFImageLoading()
            .UseSkiaSharp()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<NoUnderlineEntry, EntryHandler>();
                NoUnderlineEntryHandler.MapBorderlessEntry(EntryHandler.Mapper);
                handlers.AddHandler<BorderlessEditor, BorderlessEditorHandler>();
                //handlers.AddHandler<SwipeGestureListview, SwipeGestureListviewHandler>();
            });
        
        DatePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });
        EditorHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        PickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        //Locator.CurrentMutable.RegisterConstant<INativeServices>(new NativeServices());
        builder.Services.AddAutoMapper(typeof(MyMappingProfile));
        //builder.Services.AddSingleton<INativeServices, NativeServices>();
        //builder.Services.AddSingleton<IMapper, Mapper>(); 
        //Locator.CurrentMutable.Register(() => new NativeServices(), typeof(INativeServices));
        Locator.CurrentMutable.Register(() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<StudentPickListItem, BindableStudentPickListItem>().ReverseMap();
            cfg.CreateMap<BindableAttachmentFileView, AttachmentFileView>().ReverseMap();
            cfg.CreateMap<BindableAttachmentFileView, ExamFiles>().ReverseMap();
            cfg.CreateMap<ModuleStructureView, BindableModuleStructureView>().ReverseMap();
            cfg.CreateMap<BindableReceiptDetailView, ReceiptDetailView>().ReverseMap();
            cfg.CreateMap<BindableInvoiceDetails, InvoiceDetails>().ReverseMap();
            cfg.CreateMap<BindableReceiptDetailView, ReceiptDetailView>().ReverseMap();
            cfg.CreateMap<BindableResourceView, ResourceView>().ReverseMap();
            cfg.CreateMap<BindableResourceAttachmentView, ResourceAttachmentView>().ReverseMap();
            cfg.CreateMap<BindableResourceClassView, ResourceClassView>().ReverseMap();
            cfg.CreateMap<BindableOnlinePaymentInvoiceSubDetails, OnlinePaymentInvoiceSubDetails>().ReverseMap();
            cfg.CreateMap<BindableFinancialStatementHistoryView, FinancialStatementHistoryView>().ReverseMap();
            cfg.CreateMap<BindableStudentBookMasterView, StudentBookMasterView>().ReverseMap();
            cfg.CreateMap<BindableAppointmentAvailableTimeView, AppointmentAvailableTimeView>().ReverseMap();
            cfg.CreateMap<StudentGradingBookDataView, BindableStudentGradingBookDataView>()
                .ForMember(dest => dest.EffortsResultModel, src => src.MapFrom(x => (x.EffortResultsJson != null && !x.EffortResultsJson.Equals("[]")) ? JsonConvert.DeserializeObject<List<EffortsResultJsonModel>>(x.EffortResultsJson).FirstOrDefault() : new EffortsResultJsonModel()))
                .ForMember(dest => dest.ResultModel, src => src.MapFrom(x => (x.ResultsJson!=null && !x.ResultsJson.Equals("[]"))? JsonConvert.DeserializeObject<List<ResultJsonModel>>(x.ResultsJson).FirstOrDefault():new ResultJsonModel()))
                .ForMember(dest => dest.MarksEntryModel, src => src.MapFrom(x => (x.MarksEntryStructureJson != null && !x.MarksEntryStructureJson.Equals("[]")) ? JsonConvert.DeserializeObject<List<MarksEntryStructureJsonModel>>(x.MarksEntryStructureJson).FirstOrDefault() : new MarksEntryStructureJsonModel()))
                .ForMember(dest => dest.ElementPropertiesModel,src => src.MapFrom(x=> JsonConvert.DeserializeObject<ElementPropertiesJsonModel>(x.ElementPropertiesJson)));
            cfg.CreateMap<BindableFinancialStatementView, FinancialStatementView>().ForMember(dest => dest.StatementList, opt => opt.MapFrom(src => src.StatementList)).ReverseMap();
            cfg.CreateMap<BindableAgendaView, AgendaView>().ReverseMap();
            cfg.CreateMap<BindableAgendaClassStudentView, AgendaClassStudentView>().ReverseMap();
            cfg.CreateMap<BindableAgendaClassView, AgendaClassView>().ReverseMap();
            cfg.CreateMap<BindableSeries, Series>().ReverseMap();
            cfg.CreateMap<AttendanceEntryView, BindableAttendanceEntryView>().ReverseMap();
            cfg.CreateMap<DataCollectionFormView, BindableDataCollectionFormView>().ReverseMap();
            cfg.CreateMap<DataCollectionFieldsView, BindableDataCollectionFieldsView>().ReverseMap();
            cfg.CreateMap<SurveyAnswerView, BindableSurveyAnswerView>().ReverseMap(); 
            cfg.CreateMap<BindableRegistrationExistingStudentView, RegistrationExistingStudentView>().ReverseMap();
            cfg.CreateMap<BindableRegistrationNewStudentView, RegistrationNewStudentView>().ReverseMap();
            cfg.CreateMap<BindableFormFieldsView, FormFieldsView>().ReverseMap();
            cfg.CreateMap<BindableCommunicationMessageView, CommunicationMessageView>().ReverseMap();
            cfg.CreateMap<BindableExamScheduleView, ExamScheduleView>()
                .ForMember(dest => dest.AttachmentsArray, opt => opt.MapFrom(src => src.ExamFiles))
                .ReverseMap();
            cfg.CreateMap<SurveyQuestionAnswerView, BindableSurveyQuestionAnswerView>().ReverseMap()
                .ForMember(dest => dest.SurveyAnswerList,src => src.MapFrom(x => new ObservableCollection<BindableSurveyAnswerView>(x.SurveyAnswerList)));
            cfg.CreateMap<BindableAttendanceEntryView, AttendanceDetailsEdit>()
                .ForMember(dest => dest.CommentId, src => src.MapFrom(x => x.AttendanceCommentId == 0 ? null : x.AttendanceCommentId))
                .ForMember(dest => dest.LateTime, src => src.MapFrom(x => Convert.ToDateTime(x.LateTime)))
                .ForMember(dest => dest.LeaveEarlyTime, src => src.MapFrom(x => Convert.ToDateTime(x.LeaveEarlyTime)));
            cfg.CreateMap<BindableAttendanceEntryView, AttendanceEdit>().ReverseMap();
            
            cfg.CreateMap<BindableAttachmentFileView, StudentSubmissionAttachmentView>()
                .ForMember(dest => dest.StudentSubmissionId, src => src.MapFrom(x => x.ParentFieldId))
                .ForMember(dest => dest.AttachmentFile, src => src.MapFrom(x => x.FileName))
                .ForMember(dest => dest.AttachmentUrl, src => src.MapFrom(x => x.FilePath));

            cfg.CreateMap<StudentSubmissionAttachmentView, BindableAttachmentFileView>()
                .ForMember(dest => dest.ParentFieldId, src => src.MapFrom(x => x.StudentSubmissionId))
                .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.AttachmentFile))
                .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.AttachmentFile))
                .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.AttachmentUrl));
            
            cfg.CreateMap<AttachmentFileView, BindableAttachmentFileView>() 
                .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
                .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
                .ForMember(dest => dest.FileDevicePath, src => src.MapFrom(x => x.FilePath))
                .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.FilePath));

        }).CreateMapper(), typeof(IMapper));
        BlobCache.ApplicationName = "iCampus.MobileApp";
        BlobCache.EnsureInitialized();
        BlobCache.ForcedDateTimeKind = DateTimeKind.Local;
        CultureInfo defaultCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

#if ANDROID
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new AndroidNativeServices());
#elif IOS       
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new iOSNativeServices());
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}