using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Helpers.CustomCalendar;
using iCampus.MobileApp.Views.UserModules.BooksReservation;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.BooksReservation;

public class BooksReservationForm : ViewModelBase
    {
        #region Declaration
        public ICommand CheckBoxCheckedCommand { get; set; }
        public ICommand StudentChangeCommand { get; set; }
        public ICommand SkipStudentCommand { get; set; }
        public ICommand SelectCollectionDateCommand { get; set; }
        public ICommand SaveAndConfirmClickedCommand { get; set; }
        #endregion Declaration

        #region Properties
        ObservableCollection<BindableStudentBookMasterView> _booksList = new ObservableCollection<BindableStudentBookMasterView>();
        public ObservableCollection<BindableStudentBookMasterView> BooksList
        {
            get
            {
                return _booksList;
            }
            set
            {
                _booksList = value;
                OnPropertyChanged(nameof(BooksList));
            }
        }
        ObservableCollection<BindableStudentBookMasterView> _selectedBook = new ObservableCollection<BindableStudentBookMasterView>();
        public ObservableCollection<BindableStudentBookMasterView> SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }
        ObservableCollection<BindableStudentBookMasterView> _allStudentBooksList = new ObservableCollection<BindableStudentBookMasterView>();
        public ObservableCollection<BindableStudentBookMasterView> AllStudentBooksList
        {
            get
            {
                return _allStudentBooksList;
            }
            set
            {
                _allStudentBooksList = value;
                OnPropertyChanged(nameof(AllStudentBooksList));
            }
        }
        IList<BindableStudentPickListItem> _studentListForBooksReservation = new List<BindableStudentPickListItem>();
        public IList<BindableStudentPickListItem> StudentListForBooksReservation
        {
            get => _studentListForBooksReservation;
            set
            {
                _studentListForBooksReservation = value;
                OnPropertyChanged(nameof(StudentListForBooksReservation));
            }
        }

        BindableStudentPickListItem _selectedStudentForBooksReservation = new BindableStudentPickListItem();
        public BindableStudentPickListItem SelectedStudentForBooksReservation
        {
            get => _selectedStudentForBooksReservation;
            set
            {
                _selectedStudentForBooksReservation = value;
                OnPropertyChanged(nameof(SelectedStudentForBooksReservation));
                StudentChangeButtontext = SelectedStudentForBooksReservation.SerialNumber < StudentListForBooksReservation.Count() ? "Next Student" : "Previous Student";
            }
        }
        private int _listViewHeight;
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                OnPropertyChanged(nameof(ListViewHeight));
            }
        }
        bool _isNoRecordMsg = true;
        public bool IsNoRecordMsg
        {
            get => _isNoRecordMsg;
            set
            {
                _isNoRecordMsg = value;
                OnPropertyChanged(nameof(IsNoRecordMsg));
            }
        }
        BooksReservationView _booksReservationData = new BooksReservationView();
        public BooksReservationView BooksReservationData
        {
            get
            {
                return _booksReservationData;
            }
            set
            {
                _booksReservationData = value;
                OnPropertyChanged(nameof(BooksReservationData));
            }
        }
        string _studentChangeButtontext;
        public string StudentChangeButtontext
        {
            get
            {
                return _studentChangeButtontext;
            }
            set
            {
                _studentChangeButtontext = value;
                OnPropertyChanged(nameof(StudentChangeButtontext));
            }
        }
        bool _skipButtonVisibility=true;
        public bool SkipButtonVisibility
        {
            get => _skipButtonVisibility;
            set
            {
                _skipButtonVisibility = value;
                OnPropertyChanged(nameof(SkipButtonVisibility));
            }
        }
        bool _selectCollectionDateButtonVisibility=false;
        public bool SelectCollectionDateButtonVisibility
        {
            get => _selectCollectionDateButtonVisibility;
            set
            {
                _selectCollectionDateButtonVisibility = value;
                OnPropertyChanged(nameof(SelectCollectionDateButtonVisibility));
            }
        }
        string _currency;
        public string Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }
        decimal _totalAmount;
        public decimal TotalAmount
        {
            get
            {
                return _totalAmount;
            }
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }
        bool _isReserved;
        public bool IsReserved
        {
            get => _isReserved;
            set
            {
                _isReserved = value;
                OnPropertyChanged(nameof(IsReserved));
            }
        }
        string _selectedDateTime;
        public string SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }
        bool _studentChangeButtonVisibility=true;
        public bool StudentChangeButtonVisibility
        {
            get => _studentChangeButtonVisibility;
            set
            {
                _studentChangeButtonVisibility = value;
                OnPropertyChanged(nameof(StudentChangeButtonVisibility));
            }
        }
        string _headerMessage;
        public string HeaderMessage
        {
            get
            {
                return _headerMessage;
            }
            set
            {
                _headerMessage = value;
                OnPropertyChanged(nameof(HeaderMessage));
            }
        }
        bool _isListviewEnabled = true;
        public bool IsListviewEnabled
        {
            get => _isListviewEnabled;
            set
            {
                _isListviewEnabled = value;
                OnPropertyChanged(nameof(IsListviewEnabled));
            }
        }
        bool _saveAndConfirmVisibility;
        public bool SaveAndConfirmVisibility
        {
            get => _saveAndConfirmVisibility;
            set
            {
                _saveAndConfirmVisibility = value;
                OnPropertyChanged(nameof(SaveAndConfirmVisibility));
            }
        }
        string _selectedDate;
        public string SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        DateTime _selectedDateForComparison;
        public DateTime SelectedDateForComparison
        {
            get
            {
                return _selectedDateForComparison;
            }
            set
            {
                _selectedDateForComparison = value;
                OnPropertyChanged(nameof(SelectedDateForComparison));
            }
        }
        TimeSpan _selectedTime;
        public TimeSpan SelectedTime
        {
            get
            {
                return _selectedTime;
            }
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }
        private bool _isOption1Checked;
        public bool IsOption1Checked
        {
            get => _isOption1Checked;
            set
            {
                _isOption1Checked = value;
                OnPropertyChanged(nameof(IsOption1Checked));
            }
        }

        private bool _isOption2Checked;
        public bool IsOption2Checked
        {
            get => _isOption2Checked;
            set
            {
                _isOption2Checked = value;
                OnPropertyChanged(nameof(IsOption2Checked));
            }
        }
        #endregion Properties
        public BooksReservationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods

        private void OnCheckBoxChecked(BindableStudentBookMasterView book)
        {
            if (book != null)
            {
                var selectedBook = BooksList
                    .Where(x => x.StudentBookMasterId.Equals(book.StudentBookMasterId))
                    ?.FirstOrDefault();
                if (selectedBook != null)
                {
                    selectedBook.IsChecked = book.IsChecked;
                }

                var selectedBookFromAllStudentBooksList = AllStudentBooksList.Where(x =>
                    x.StudentBookMasterId.Equals(book.StudentBookMasterId) &&
                    x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.FirstOrDefault();
                if (selectedBookFromAllStudentBooksList != null)
                {
                    selectedBookFromAllStudentBooksList.IsChecked = book.IsChecked;
                    if (book.IsChecked)
                    {
                        AllStudentBooksList
                            .Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)
                            ?.ToList().ForEach(x => x.IsSkipped = false);

                        if (IsReserved)
                        {
                            AllStudentBooksList
                                .Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)
                                ?.ToList()
                                .ForEach(x => x.SkippedStudentBooksSelectionAfterBooking = true);
                        }
                    }
                }

                var checkedData = BooksList.Where(x => x.IsChecked).ToList();
                if (checkedData != null && checkedData.Count() > 0)
                {
                    SkipButtonVisibility = true;
                }

                var MandatoryBooks = AllStudentBooksList.Where(x => x.IsMandatory).ToList();
                if (MandatoryBooks != null && MandatoryBooks.Count > 0)
                {
                    var checkMandatoryData = AllStudentBooksList
                        .Where(x => x.IsMandatory && !x.IsChecked && !x.IsSkipped).ToList();
                    if (checkMandatoryData == null || checkMandatoryData.Count() <= 0)
                    {
                        SelectCollectionDateButtonVisibility = true;
                        if (IsReserved)
                        {
                            var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList
                                .Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                            if (skippedStudentBooksSelectionAfterBookingList != null &&
                                skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                            {
                                SaveAndConfirmVisibility = true;
                            }
                            else
                            {
                                SaveAndConfirmVisibility = false;
                            }
                        }
                    }
                    else
                    {
                        SelectCollectionDateButtonVisibility = false;
                    }
                }
                else
                {
                    var checkData = AllStudentBooksList.Where(x => x.IsChecked).ToList();
                    if (checkData != null && checkData.Count() > 0)
                    {
                        SelectCollectionDateButtonVisibility = true;
                        if (IsReserved)
                        {
                            var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList
                                .Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                            if (skippedStudentBooksSelectionAfterBookingList != null &&
                                skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                            {
                                SaveAndConfirmVisibility = true;
                            }
                            else
                            {
                                SaveAndConfirmVisibility = false;
                            }
                        }
                    }
                    else
                    {
                        SelectCollectionDateButtonVisibility = false;
                    }
                }
                GetTotalAount(BooksList.ToList());
            }
        }

        
        async void InitializePage()
        {
            CheckBoxCheckedCommand = new Command<BindableStudentBookMasterView>(OnCheckBoxChecked);
            StudentChangeCommand = new Command(StudentChangeClicked);
            SkipStudentCommand = new Command(SkipStudentClicked);
            SelectCollectionDateCommand = new Command(SelectCollectionDateClicked);
            SaveAndConfirmClickedCommand = new Command(SaveAndConfirmClicked);
            if (AppSettings.Current.StudentList!=null&& AppSettings.Current.StudentList.Count()>0)
            {
                StudentListForBooksReservation = AppSettings.Current.StudentList;
                if (StudentListForBooksReservation != null && StudentListForBooksReservation.Count > 0 && StudentListForBooksReservation.Count == 1)
                {
                    StudentChangeButtonVisibility = false;
                }
                var counter = 1;
                foreach (var x in StudentListForBooksReservation)
                {
                    x.SerialNumber = counter;
                    counter++;
                }
                if(AppSettings.Current.SelectedStudent==null||string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
                {
                    SelectedStudentForBooksReservation = StudentListForBooksReservation.ToList().FirstOrDefault();
                }
                else
                {
                    SelectedStudentForBooksReservation = StudentListForBooksReservation.ToList().Where(x => x.ItemId == AppSettings.Current.SelectedStudent.ItemId)?.FirstOrDefault();
                }
                MessagingCenter.Subscribe<string>("", "BooksReservationPageSubscribe", (arg) =>
                {
                    MessagingCenter.Subscribe<string>("", "BooksReservationPageSwipe", async (booksReservationPageArg) =>
                    {
                        //await SideMenuClicked();
                    });
                });
                MessagingCenter.Subscribe<BindableStudentBookMasterView>(this, "UpdateAmountOnQuantityChanged", async (args) =>
                {
                    if (args != null)
                    {
                        BindableStudentBookMasterView bindableStudentBookMasterView = (BindableStudentBookMasterView)args;
                        var selectedBook = BooksList.Where(x => x.StudentBookMasterId.Equals(bindableStudentBookMasterView.StudentBookMasterId))?.FirstOrDefault();
                        if (selectedBook != null)
                        {
                            selectedBook.Amount = selectedBook.Quantity* selectedBook.Price;
                            GetTotalAount(BooksList.ToList());
                        }
                    }
                });
            }
            await GetBooksData();
            MessagingCenter.Subscribe<BindableStudentBookMasterView>(this, "BooksSelection", async (args) =>
            {
                if (args != null)
                {
                    BindableStudentBookMasterView bindableResourcesPickListItem = (BindableStudentBookMasterView)args;
                    var selectedBook = BooksList.Where(x => x.StudentBookMasterId.Equals(bindableResourcesPickListItem.StudentBookMasterId))?.FirstOrDefault();
                    if (selectedBook != null)
                    {
                        selectedBook.IsChecked = bindableResourcesPickListItem.IsChecked;
                    }
                    var selectedBookFromAllStudentBooksList= AllStudentBooksList.Where(x => x.StudentBookMasterId.Equals(bindableResourcesPickListItem.StudentBookMasterId) && x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.FirstOrDefault();
                    if (selectedBookFromAllStudentBooksList != null)
                    {
                        selectedBookFromAllStudentBooksList.IsChecked = bindableResourcesPickListItem.IsChecked;
                        if (bindableResourcesPickListItem.IsChecked)
                        {
                            AllStudentBooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.ToList().ForEach(x => x.IsSkipped = false);
                            if (IsReserved)
                            {
                                AllStudentBooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.ToList().ForEach(x => x.SkippedStudentBooksSelectionAfterBooking = true);
                            }
                        }
                    }
                }
                var checkedData = BooksList.Where(x => x.IsChecked).ToList();
                if(checkedData != null && checkedData.Count() > 0)
                {
                    SkipButtonVisibility = true;
                }
            var MandatoryBooks = AllStudentBooksList.Where(x => x.IsMandatory).ToList();
                if (MandatoryBooks != null && MandatoryBooks.Count > 0)
                {
                    var checkMandatoryData = AllStudentBooksList.Where(x => x.IsMandatory && !x.IsChecked && !x.IsSkipped).ToList();
                    if (checkMandatoryData == null || checkMandatoryData.Count() <= 0)
                    {
                        SelectCollectionDateButtonVisibility = true;
                        if (IsReserved)
                        {
                            var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList.Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                            if (skippedStudentBooksSelectionAfterBookingList != null && skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                            {
                                SaveAndConfirmVisibility = true;
                            }
                            else
                            {
                                SaveAndConfirmVisibility = false;
                            }
                        }
                    }
                    else
                    {
                        SelectCollectionDateButtonVisibility = false;
                    }
                }
                else
                {
                    var checkData = AllStudentBooksList.Where(x => x.IsChecked).ToList();
                    if (checkData != null && checkData.Count() > 0)
                    {
                        SelectCollectionDateButtonVisibility = true;
                        if (IsReserved)
                        {
                            var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList.Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                            if (skippedStudentBooksSelectionAfterBookingList != null && skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                            {
                                SaveAndConfirmVisibility = true;
                            }
                            else
                            {
                                SaveAndConfirmVisibility = false;
                            }
                        }

                    }
                    else
                    {
                        SelectCollectionDateButtonVisibility = false;
                    }
                }
                GetTotalAount(BooksList.ToList());
            });
            MessagingCenter.Subscribe<string>("", "RefreshBooksReservationPage",async (arg) =>
            {
               await GetBooksData();
            });
            MessagingCenter.Subscribe<string>("", "StudentSelectionIconTapped", async (arg) =>
            {
                await StudentSelectionIconTapped();
            });
        }
        async Task GetBooksData()
        {
            try
            {
                var apiUrl = string.Format(TextResource.GetBooksReservationDataApiUrl);
                BooksReservationData = await ApiHelper.GetObject<BooksReservationView>(apiUrl);
                if (BooksReservationData!=null)
                {
                    Currency = BooksReservationData.CurrencySettings != null && BooksReservationData.CurrencySettings.CurrencyCode != null ? BooksReservationData.CurrencySettings.CurrencyCode : string.Empty;
                    IsReserved = BooksReservationData.BooksReservationData != null && BooksReservationData.BooksReservationData.IsReserved ? true : false;
                    if (BooksReservationData.BooksCollectionList!= null)
                    {
                        AllStudentBooksList = _mapper.Map<ObservableCollection<BindableStudentBookMasterView>>(BooksReservationData.BooksCollectionList.ToList());
                        AllStudentBooksList.Where(x => x.IsMandatory == true).ToList().ForEach(x => x.IsChecked = true);
                        AllStudentBooksList.ForEach(x => x.Amount = x.Price * x.Quantity);
                        if (IsReserved)
                        {
                            SelectedDate = BooksReservationData.BooksReservationData.BookingDate.ToString("MM/dd/yyyy");
                            SelectedDateForComparison = BooksReservationData.BooksReservationData.BookingDate;
                            SelectedTime = BooksReservationData.BooksReservationData.BookingTime;
                        }
                        StudentWiseBooksData(Currency);
                    }
                    SelectedDateTime = IsReserved ? string.Concat(BooksReservationData.BooksReservationData.BookingDate.ToString("dddd, MMMM dd, yyyy")," ", DateTime.Today.Add(BooksReservationData.BooksReservationData.BookingTime).ToString("hh:mm tt")):string.Empty;
                    HeaderMessage = BooksReservationData.BookCollectionSettings != null&& BooksReservationData.BookCollectionSettings.HeaderMessage!=null ? BooksReservationData.BookCollectionSettings.HeaderMessage:string.Empty;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        async void StudentChangeClicked()
        {
            if(SkipButtonVisibility)
            {
                if (!StudentChangeValidation())
                {
                    await HelperMethods.ShowAlert("", TextResource.MandatoryBookSelectionAlert);
                    return;
                }
            }
            if (SelectedStudentForBooksReservation.SerialNumber < StudentListForBooksReservation.Count())
            {
                SelectedStudentForBooksReservation = StudentListForBooksReservation.ElementAt(SelectedStudentForBooksReservation.SerialNumber );
            }
            else
            {
                SelectedStudentForBooksReservation = StudentListForBooksReservation.ElementAt(SelectedStudentForBooksReservation.SerialNumber - 2);
            }
            AppSettings.Current.SelectedStudent = SelectedStudentForBooksReservation;
            StudentWiseBooksData(Currency);
            MessagingCenter.Send<string>("", "BooksListViewTopScroll");
        }
        async void SkipStudentClicked()
        {
            try
            {
                var isSkipChild = await App.Current.MainPage.DisplayAlert("", TextResource.SkipChildConfirmationMessage, TextResource.YesText, TextResource.NoText);
                if (isSkipChild)
                { 
                    var apiUrl = string.Format(TextResource.SkipStudentForBookReservationApiUrl,SelectedStudentForBooksReservation.ItemId);
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(apiUrl);
                if(result.Success)
                {
                    BooksList.Where(x=>x.StudentId.ToString()==SelectedStudentForBooksReservation.ItemId).ForEach(x => x.IsSkipped = true);
                    BooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId).ForEach(x => x.IsChecked = false);
                    BooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId).ForEach(x => x.IsSkipped = true);
                    AllStudentBooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.ToList().ForEach(x => x.IsSkipped = true);
                    AllStudentBooksList.Where(x => x.StudentId.ToString() == SelectedStudentForBooksReservation.ItemId)?.ToList().ForEach(x => x.IsChecked = false);

                   
                        var MandatoryBooks = AllStudentBooksList.Where(x => x.IsMandatory).ToList();
                        if (MandatoryBooks != null && MandatoryBooks.Count > 0)
                        {
                            var checkData = AllStudentBooksList.Where(x => x.IsMandatory && x.IsChecked && !x.IsSkipped).ToList();
                            if (checkData != null && checkData.Count() > 0)
                            {
                                SelectCollectionDateButtonVisibility = true;
                            }
                            else
                            {
                                SelectCollectionDateButtonVisibility = false;
                            }
                        }
                        else
                        {
                            var checkData = AllStudentBooksList.Where(x => x.IsChecked).ToList();
                            if (checkData != null && checkData.Count() > 0)
                            {
                                SelectCollectionDateButtonVisibility = true;
                            }
                            else
                            {
                                SelectCollectionDateButtonVisibility = false;
                            }
                        }
                        SkipButtonVisibility = false;
                }
                }
                MessagingCenter.Send<string>("", "BooksListViewTopScroll");
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        async void SelectCollectionDateClicked()
        {
            var action = await App.Current.MainPage.DisplayAlert("", TextResource.SelectBooksCollectionDateConfirmationMessage, TextResource.OkText, TextResource.CancelText);
            if (action)
            {
                try
                {
                    var apiUrl = string.Format(TextResource.GetAppointmentAvailableTimes, SelectedStudentForBooksReservation.ItemId);
                    AppointmentAvailableTimeView appointmentAvailableTimeData = await ApiHelper.GetObject<AppointmentAvailableTimeView>(apiUrl);
                    if (appointmentAvailableTimeData != null)
                    {
                        CollectionDateSelectionForm collectionDateSelectionForm = new(_mapper, _nativeServices, Navigation);
                        if (appointmentAvailableTimeData.appointmentAvailableTimesList != null)
                        {
                            collectionDateSelectionForm.AvailableTimeList = _mapper.Map<ObservableCollection<BindableAppointmentAvailableTimeView>>(appointmentAvailableTimeData.appointmentAvailableTimesList);
                            var grpList = from time in collectionDateSelectionForm.AvailableTimeList
                                          group time by time.BookingDateFormatted into timeGroup
                                          select new Grouping<string, BindableAppointmentAvailableTimeView>(timeGroup.Key, timeGroup.Where(x => x.BookingDateFormatted == timeGroup.Key).FirstOrDefault(), timeGroup);
                            var timeSlotList = collectionDateSelectionForm.AvailableTimeList.GroupBy(
    p => p.BookingDate,
    p => new BindableTimeSlotClass() {TimeSlotString=p.FormattedTimeSlot },
    (key, g) => new BindableAppointmentAvailableTimeView { Date = key.ToString("dddd, MMMM dd, yyyy"), TimeSlotList = g.ToList() }).ToList();
                            if (timeSlotList != null && timeSlotList.Count>0)
                            {
                                collectionDateSelectionForm.AvailableTimeList = new ObservableCollection<BindableAppointmentAvailableTimeView>(timeSlotList);
                            }
                        }
                        collectionDateSelectionForm.IsNoRecordMsg = (collectionDateSelectionForm.AvailableTimeList != null && collectionDateSelectionForm.AvailableTimeList.Count > 0)? false : true;
                        collectionDateSelectionForm.AllStudentBooksList = AllStudentBooksList;
                        collectionDateSelectionForm.PageTitle = TextResource.SelectCollectionDateText;
                        collectionDateSelectionForm.BackVisible = true;
                        collectionDateSelectionForm.MenuVisible = false;
                        collectionDateSelectionForm.IsPopUpPage = false;
                        SelectCollectionDatePage selectCollectionDatePage = new ()
                        {
                            BindingContext = collectionDateSelectionForm
                        };
                        await Navigation.PushAsync(selectCollectionDatePage);
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
            }
        }
         bool StudentChangeValidation()
        {
          var data=  BooksList.Where(x=>x.IsMandatory&&!x.IsChecked)?.ToList();
            bool isValidate = data != null && data.Count() > 0 ? false : true;
            return isValidate;
        }
        async void StudentWiseBooksData(string currencyCode)
        {
            BooksList = new ObservableCollection<BindableStudentBookMasterView>(AllStudentBooksList.Where(x => x.StudentId.ToString().Equals(SelectedStudentForBooksReservation.ItemId))?.ToList());

            if (IsReserved)
            {
                if (SelectedDateForComparison!=null&&DateTime.Now.CompareTo(SelectedDateForComparison) < 0)
                {
                    IsListviewEnabled = false;
                    var skippedStudentList = BooksList.Where(x => x.IsSkipped == true).ToList();
                    var skippedStudentBooksSelectionAfterBookingList = BooksList.Where(x => x.SkippedStudentBooksSelectionAfterBooking == true).ToList();
                    if (skippedStudentList == null || skippedStudentList.Count <= 0)
                    {
                        if (skippedStudentBooksSelectionAfterBookingList == null || skippedStudentBooksSelectionAfterBookingList.Count <= 0)
                        {
                            BooksList = new ObservableCollection<BindableStudentBookMasterView>(BooksList.Where(x => x.IsChecked).ToList());
                        }
                    }
                    else
                    {
                        IsListviewEnabled = true;
                    }
                }
                else
                {
                    IsListviewEnabled = false;
                }
            }


            BooksList.ForEach(x => x.Currency = currencyCode);
            BooksList.Where(x => x.IsMandatory == true).ToList().ForEach(x => x.IsChecked = true);
            var skippedList =BooksList.Where(x => x.IsSkipped == true).ToList();
            if(skippedList!=null&&skippedList.Count>0)
            {
                SkipButtonVisibility = false;
                skippedList.ForEach(x=>x.IsChecked=false);
            }
            else
            {
                SkipButtonVisibility = true;
            }
            var checkedData = BooksList.Where(x => x.IsChecked).ToList();
            if (checkedData != null && checkedData.Count() > 0)
            {
                SkipButtonVisibility = true;
            }
            var checkMandatoryData = BooksList.Where(x => x.IsMandatory && x.IsChecked && !x.IsSkipped).ToList();
            if (checkMandatoryData != null && checkMandatoryData.Count() > 0)
            {
                SelectCollectionDateButtonVisibility = true;
            }
            else
            {
                SelectCollectionDateButtonVisibility = false;
            }

            var MandatoryBooks= AllStudentBooksList.Where(x => x.IsMandatory).ToList();
            if(MandatoryBooks != null&& MandatoryBooks.Count>0)
            {
                var checkData = AllStudentBooksList.Where(x => x.IsMandatory && x.IsChecked && !x.IsSkipped).ToList();
                if (checkData != null && checkData.Count() > 0)
                {
                    SelectCollectionDateButtonVisibility = true;
                    if (IsReserved)
                    {
                        var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList.Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                        if (skippedStudentBooksSelectionAfterBookingList != null && skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                        {
                            SaveAndConfirmVisibility = true;
                        }
                        else
                        {
                            SaveAndConfirmVisibility = false;
                        }
                    }
                }
                else
                {
                    SelectCollectionDateButtonVisibility = false;
                }
            }
            else
            {
                var checkData = AllStudentBooksList.Where(x =>x.IsChecked).ToList();
                if (checkData != null && checkData.Count() > 0)
                {
                    SelectCollectionDateButtonVisibility = true;
                    if (IsReserved)
                    {
                        var skippedStudentBooksSelectionAfterBookingList = AllStudentBooksList.Where(x => x.SkippedStudentBooksSelectionAfterBooking).ToList();
                        if (skippedStudentBooksSelectionAfterBookingList != null && skippedStudentBooksSelectionAfterBookingList.Count() > 0)
                        {
                            SaveAndConfirmVisibility = true;
                        }
                        else
                        {
                            SaveAndConfirmVisibility = false;
                        }
                    }
                }
                else
                {
                    SelectCollectionDateButtonVisibility = false;
                }
            }
            ListViewHeight = BooksList != null && BooksList.Count > 0 ? BooksList.Count * 155 : 0;
            IsNoRecordMsg = BooksList != null && BooksList.Count > 0 ? false : true;
            GetTotalAount(BooksList.ToList());
        }
        void GetTotalAount(List<BindableStudentBookMasterView> list)
        {
            if(list!=null)
            {
               TotalAmount= list.Where(x=>x.IsChecked).Sum(x=>x.Amount);
            }
        }
        async Task StudentSelectionIconTapped()
        {
            try
            {
                if (SkipButtonVisibility)
                {
                    if (!StudentChangeValidation())
                    {
                        await HelperMethods.ShowAlert("", TextResource.MandatoryBookSelectionAlert);
                        return;
                    }
                }
                if(StudentListForBooksReservation.ToList()!=null&&StudentListForBooksReservation.ToList().Count>0)
                {
                    var data = StudentListForBooksReservation.ToList().Where(x => x.ItemId == AppSettings.Current.SelectedStudent.ItemId);
                    if(data!=null&&data.FirstOrDefault()!=null)
                    {
                        SelectedStudentForBooksReservation = data.FirstOrDefault();
                    }
                }
                StudentWiseBooksData(Currency);
                MessagingCenter.Send<string>("", "BooksListViewTopScroll");
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        async void SaveAndConfirmClicked()
        {
            try
            {
                List<StudentBookReservationDetailView> list = new List<StudentBookReservationDetailView>();
                foreach (var item in AllStudentBooksList)
                {
                    if (item != null && item.IsChecked&&item.SkippedStudentBooksSelectionAfterBooking)
                    {
                        list.Add(new StudentBookReservationDetailView() { StudentId = item.StudentId.ToString(), StudentBookMasterId = item.StudentBookMasterId.ToString(), Title = item.Title, Quantity = item.Quantity, Price = item.Price, Amount = item.Price });
                    }
                }
                string json = JsonConvert.SerializeObject(list);
                var apiUrl = string.Format(TextResource.BooksReservationApiUrl, SelectedDate, SelectedTime, json);
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(apiUrl);
                if (result.Success)
                {
                    MessagingCenter.Send("", "RefreshBooksReservationPage");
                    //await PopupNavigation.Instance.PushAsync(new BookingConfirmationPopup(), true);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        #endregion Methods
    }