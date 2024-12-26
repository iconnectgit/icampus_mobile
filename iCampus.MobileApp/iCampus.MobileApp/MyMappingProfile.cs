using System.Collections.ObjectModel;
using iCampus.Common.ViewModels;
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
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp;

using AutoMapper;

public class MyMappingProfile : Profile
{
    public MyMappingProfile()
    {
        CreateMap<StudentPickListItem, BindableStudentPickListItem>().ReverseMap();
        CreateMap<BindableAttachmentFileView, AttachmentFileView>().ReverseMap();
        CreateMap<BindableAttachmentFileView, ExamFiles>().ReverseMap();
        CreateMap<ModuleStructureView, BindableModuleStructureView>().ReverseMap();
        CreateMap<BindableReceiptDetailView, ReceiptDetailView>().ReverseMap();
        CreateMap<BindableInvoiceDetails, InvoiceDetails>().ReverseMap();
        CreateMap<BindableReceiptDetailView, ReceiptDetailView>().ReverseMap();
        CreateMap<BindableResourceView, ResourceView>().ReverseMap();
        CreateMap<BindableResourceAttachmentView, ResourceAttachmentView>().ReverseMap();
        CreateMap<BindableResourceClassView, ResourceClassView>().ReverseMap();
        CreateMap<BindableOnlinePaymentInvoiceSubDetails, OnlinePaymentInvoiceSubDetails>().ReverseMap();
        CreateMap<BindableFinancialStatementHistoryView, FinancialStatementHistoryView>().ReverseMap();
        CreateMap<BindableStudentBookMasterView, StudentBookMasterView>().ReverseMap();
        CreateMap<BindableAppointmentAvailableTimeView, AppointmentAvailableTimeView>().ReverseMap();
        CreateMap<StudentGradingBookDataView, BindableStudentGradingBookDataView>()
            .ForMember(dest => dest.EffortsResultModel,
                src => src.MapFrom(x =>
                    (x.EffortResultsJson != null && !x.EffortResultsJson.Equals("[]"))
                        ? JsonConvert.DeserializeObject<List<EffortsResultJsonModel>>(x.EffortResultsJson)
                            .FirstOrDefault()
                        : new EffortsResultJsonModel()))
            .ForMember(dest => dest.ResultModel,
                src => src.MapFrom(x =>
                    (x.ResultsJson != null && !x.ResultsJson.Equals("[]"))
                        ? JsonConvert.DeserializeObject<List<ResultJsonModel>>(x.ResultsJson).FirstOrDefault()
                        : new ResultJsonModel()))
            .ForMember(dest => dest.MarksEntryModel,
                src => src.MapFrom(x =>
                    (x.MarksEntryStructureJson != null && !x.MarksEntryStructureJson.Equals("[]"))
                        ? JsonConvert.DeserializeObject<List<MarksEntryStructureJsonModel>>(x.MarksEntryStructureJson)
                            .FirstOrDefault()
                        : new MarksEntryStructureJsonModel()))
            .ForMember(dest => dest.ElementPropertiesModel,
                src => src.MapFrom(x =>
                    JsonConvert.DeserializeObject<ElementPropertiesJsonModel>(x.ElementPropertiesJson)));
        CreateMap<BindableFinancialStatementView, FinancialStatementView>()
            .ForMember(dest => dest.StatementList, opt => opt.MapFrom(src => src.StatementList)).ReverseMap();
        CreateMap<BindableAgendaView, AgendaView>().ReverseMap();
        CreateMap<BindableAgendaClassStudentView, AgendaClassStudentView>().ReverseMap();
        CreateMap<BindableAgendaClassView, AgendaClassView>().ReverseMap();
        CreateMap<BindableSeries, Series>().ReverseMap();
        CreateMap<AttendanceEntryView, BindableAttendanceEntryView>().ReverseMap();
        CreateMap<DataCollectionFormView, BindableDataCollectionFormView>().ReverseMap();
        CreateMap<DataCollectionFieldsView, BindableDataCollectionFieldsView>().ReverseMap();
        CreateMap<SurveyAnswerView, BindableSurveyAnswerView>().ReverseMap();
        CreateMap<BindableRegistrationExistingStudentView, RegistrationExistingStudentView>().ReverseMap();
        CreateMap<BindableRegistrationNewStudentView, RegistrationNewStudentView>().ReverseMap();
        CreateMap<BindableFormFieldsView, FormFieldsView>().ReverseMap();
        CreateMap<BindableCommunicationMessageView, CommunicationMessageView>().ReverseMap();
        CreateMap<BindableExamScheduleView, ExamScheduleView>()
            .ForMember(dest => dest.AttachmentsArray, opt => opt.MapFrom(src => src.ExamFiles))
            .ReverseMap();
        CreateMap<SurveyQuestionAnswerView, BindableSurveyQuestionAnswerView>().ReverseMap()
            .ForMember(dest => dest.SurveyAnswerList,
                src => src.MapFrom(x => new ObservableCollection<BindableSurveyAnswerView>(x.SurveyAnswerList)));
        CreateMap<BindableAttendanceEntryView, AttendanceDetailsEdit>()
            .ForMember(dest => dest.CommentId,
                src => src.MapFrom(x => x.AttendanceCommentId == 0 ? null : x.AttendanceCommentId))
            .ForMember(dest => dest.LateTime, src => src.MapFrom(x => Convert.ToDateTime(x.LateTime)))
            .ForMember(dest => dest.LeaveEarlyTime, src => src.MapFrom(x => Convert.ToDateTime(x.LeaveEarlyTime)));
        CreateMap<BindableAttendanceEntryView, AttendanceEdit>().ReverseMap();

        CreateMap<BindableAttachmentFileView, StudentSubmissionAttachmentView>()
            .ForMember(dest => dest.StudentSubmissionId, src => src.MapFrom(x => x.ParentFieldId))
            .ForMember(dest => dest.AttachmentFile, src => src.MapFrom(x => x.FileName))
            .ForMember(dest => dest.AttachmentUrl, src => src.MapFrom(x => x.FilePath));

        CreateMap<StudentSubmissionAttachmentView, BindableAttachmentFileView>()
            .ForMember(dest => dest.ParentFieldId, src => src.MapFrom(x => x.StudentSubmissionId))
            .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.AttachmentFile))
            .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.AttachmentFile))
            .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.AttachmentUrl));

        CreateMap<AttachmentFileView, BindableAttachmentFileView>()
            .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
            .ForMember(dest => dest.FileName, src => src.MapFrom(x => x.FileName))
            .ForMember(dest => dest.FileDevicePath, src => src.MapFrom(x => x.FilePath))
            .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.FilePath));
    }
}