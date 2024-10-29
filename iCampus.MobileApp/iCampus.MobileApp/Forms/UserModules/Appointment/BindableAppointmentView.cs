namespace iCampus.MobileApp.Forms.UserModules.Appointment;

public class BindableAppointmentView
    {
        public class TeacherAppointmentListView
        {
            public int AppointmentBookingId { get; set; }
            public int AppointmentSettingsId { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string TeacherName { get; set; }
            public int TeacherId { get; set; }
            public string BookingDateTimeFormatted { get; set; }
            public string StudentCourses { get; set; }
            public string ParentComments { get; set; }
            public object TeacherComments { get; set; }
            public string ClassName { get; set; }
            public bool IsApproved { get; set; }
            public bool IsHeadOfSection { get; set; }
            public string AppointmentStatus { get; set; }
            public string AppointmentStatusColor { get; set; }
            public DateTime BookingDate { get; set; }
            public string BookingTime { get; set; }
            public int GenderId { get; set; }
            public bool IsCancelled { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsScheduled { get; set; }
            public bool IsScheduledAndUpComing { get; set; }
        }

        public class TeacherCurriculumListView
        {
            public int AppointmentSettingsId { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string TeacherName { get; set; }
            public int TeacherId { get; set; }
            public bool IsHeadOfSection { get; set; }
            public string StudentCourses { get; set; }
            public string ClassName { get; set; }
            public int GenderId { get; set; }
        }

        
            public List<TeacherAppointmentListView> TeacherAppointmentList { get; set; }
            public List<TeacherCurriculumListView> TeacherCurriculumList { get; set; }
            public List<object> FamilyAppointmentBookingList { get; set; }
        

    }