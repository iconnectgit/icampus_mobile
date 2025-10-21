using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Event;

namespace iCampus.MobileApp.Forms.UserModules.Event;

public class EventForm : ViewModelBase
{
    #region Declarations

    public ICommand ListTappedCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }
    public ICommand AttachmentClickCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }
    public ICommand PreviewIconTappedCommand { get; set; }
    public ICommand LinksClickCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand ExpandCollapseClickCommand { get; set; }

    #endregion

    #region Properties

    private BindableCalendarView _selectedEvent = new();

    public BindableCalendarView SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            _selectedEvent = value;
            OnPropertyChanged(nameof(SelectedEvent));
        }
    }

    private ObservableCollection<BindableCalendarView> _eventList;

    public ObservableCollection<BindableCalendarView> EventList
    {
        get => _eventList;
        set
        {
            _eventList = value;
            OnPropertyChanged(nameof(EventList));
        }
    }

    private DateTime _fromDate;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            if (value <= ToDate)
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private DateTime _toDate;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            if (value >= FromDate)
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }
    private bool _dateErrorMessageVisibility;

    public bool DateErrorMessageVisibility
    {
        get => _dateErrorMessageVisibility;
        set
        {
            _dateErrorMessageVisibility = value;
            OnPropertyChanged(nameof(DateErrorMessageVisibility));
        }
    }

    private string _eventPageTitle;

    public string EventPageTitle
    {
        get => _eventPageTitle;
        set
        {
            _eventPageTitle = value;
            OnPropertyChanged(nameof(EventPageTitle));
        }
    }

    private bool _noDataExist;

    public bool NoDataExist
    {
        get => _noDataExist;
        set
        {
            _noDataExist = value;
            OnPropertyChanged(nameof(NoDataExist));
        }
    }

    private IList<BindableAttachmentFileView> _selectedAttachmentList;

    public IList<BindableAttachmentFileView> SelectedAttachmentList
    {
        get => _selectedAttachmentList;
        set
        {
            _selectedAttachmentList = value;
            OnPropertyChanged(nameof(SelectedAttachmentList));
        }
    }
    IEnumerable<WebsiteLinkView> _selectedWebsiteLinks;
    public IEnumerable<WebsiteLinkView> SelectedWebsiteLinks
    {
        get => _selectedWebsiteLinks;
        set
        {
            _selectedWebsiteLinks = value;
            OnPropertyChanged(nameof(SelectedWebsiteLinks));
        }
    }

    #endregion

    public EventForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string id = null) : base(
        null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage(id);
    }

    #region Private methods

    private async void InitializePage(string id = null)
    {
        NotificationItemId = id;
        ToDate = ToDate == DateTime.MinValue ? DateTime.Now : ToDate;
        MenuVisible = true;
        BackVisible = false;
        ListTappedCommand = new Command<BindableCalendarView>(ListViewTapped);
        FilterClickCommand = new Command(FilterClicked);
        AttachmentClickCommand = new Command<BindableCalendarView>(AttachmentClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
        PreviewIconTappedCommand = new Command(PreviewIconClicked);
        LinksClickCommand = new Command<BindableCalendarView>(LinksClicked);
        SearchClickCommand = new Command(SearchClicked);
        ExpandCollapseClickCommand = new Command<BindableCalendarView>(ExpandCollapseClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        await GetCalendarEventList();
        MessagingCenter.Subscribe<string>("", "ListViewRightSwipeEvents", async (arg) =>
        {
            //await SideMenuClicked();
        });
        //MessagingCenter.Subscribe<string>("", "subscribeEventsSwipe", (arg) =>
        //{
        //    MessagingCenter.Subscribe<string>("", "ListViewRightSwipeEvents", async (argu) =>
        //    {
        //        await SideMenuClicked();
        //    });

        //});
        MessagingCenter.Subscribe<string>("", "ListViewRightSwipeEventsSu", (arg) =>
        {
            MessagingCenter.Subscribe<string>("", "ListViewRightSwipeEvents", async (args) =>
            {
                //await SideMenuClicked();
            });
        });
    }

    public void ExpandCollapseClicked(BindableCalendarView bindableCalendarView)
    {
        if (bindableCalendarView != null)
            foreach (var item in EventList.ToList())
                if (item != null)
                {
                    if (item.EventId == bindableCalendarView.EventId)
                    {
                        item.DetailsVisibility = !item.DetailsVisibility;
                        item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                            ? "dropdown_gray.png"
                            : "uparrow_gray.png";
                    }
                    else
                    {
                        item.DetailsVisibility = false;
                        item.ArrowImageSource = "dropdown_gray.png";
                    }
                }
        MessagingCenter.Send("", "EventExpandCollapse");
    }

    private async void LinksClicked(BindableCalendarView sender)
    {
        SelectedWebsiteLinks = sender.WebsiteLinks;
        foreach (var link in SelectedWebsiteLinks)
        {
            if (!string.IsNullOrEmpty(link.Title))
                link.Title = Uri.UnescapeDataString(link.Title);
        }
        var websiteLinksPopup = new WebsiteLinksPopup()
        {
            BindingContext = this
        };
        SetPopupInstance(websiteLinksPopup);
        Application.Current.MainPage.ShowPopup(websiteLinksPopup);
        
    }

    private async void AttachmentClicked(BindableCalendarView sender)
    {
        AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
        {
            SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentList)
        };
        var attachmentListPopup = new AttachmentListPopup()
        {
            BindingContext = attachmentListPopupForm
        };
        SetPopupInstance(attachmentListPopup);
        await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
    }

    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }
    private async void FilterClicked(object obj)
    {
        EventPageTitle = PageTitle;
        PageTitle = TextResource.FilterEventTitle;
        MenuVisible = false;
        BackVisible = true;
        IsPopUpPage = true;
        ToDate = DateTime.Now;
        FromDate = ToDate;  
        EventFilterPage eventFilterPage = new()
        {
            BindingContext = this
        };
        await Navigation.PushAsync(eventFilterPage);
    }

    private async void ListViewTapped(BindableCalendarView obj)
    {
        if (obj != null)
        {
            EventDetailForm eventDetailForm = new(_mapper, _nativeServices, Navigation)
            {
                SelectedEvent = obj,
                SelectedDate = DateTime.Now,
                PageTitle = obj.EventCategoryName,
                BackVisible = true,
                MenuVisible = false,
                AttachmentListViewHeight = obj.AttachmentCount * 40,
                LinkListViewHeight = obj.WebsiteLinksCount * 40,
                AttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(obj.AttachmentList)
            };
            EventDetails eventDetails = new EventDetails()
            {
                BindingContext = eventDetailForm
            };
            await Navigation.PushAsync(eventDetails);
            SelectedEvent = null;
        }
    }

    public async Task<ObservableCollection<BindableCalendarView>> GetCalendarEventList()
    {
        try
        {
            var toDate = !string.IsNullOrEmpty(NotificationItemId) ? null : ToDate.ToPickerDateFormatted();
            var fromDate = FromDate == DateTime.MinValue ? string.Empty : FromDate.ToPickerDateFormatted();
            FromDate = string.IsNullOrEmpty(fromDate) ? DateTime.Now : FromDate;
            var apiUrl = string.Format(TextResource.EventPageApiUrl, fromDate, toDate);
            var eventList = await ApiHelper.GetObjectList<BindableCalendarView>(apiUrl);
            EventList = new ObservableCollection<BindableCalendarView>(eventList);
            NoDataExist = !EventList.Any();
            if (EventList != null && EventList.Count > 0 && !string.IsNullOrEmpty(NotificationItemId))
            {
                var eventView = EventList.Where(x => x.EventId == Convert.ToInt32(NotificationItemId)).FirstOrDefault();
                if (eventView != null)
                    ListViewTapped(eventView);
                NotificationItemId = null;
            }

            EventList = new ObservableCollection<BindableCalendarView>(EventList.OrderBy(x => x.EventFromDate)
                .ToList());
            return EventList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.EventPageTitle);
            return new ObservableCollection<BindableCalendarView>();
        }
    }


    private async void PreviewIconClicked(object obj)
    {
        if (obj != null)
            try
            {
                await Launcher.Default.OpenAsync(new Uri(obj.ToString()));
                if (obj.ToString().StartsWith("http://") || obj.ToString().StartsWith("https://"))
                {
                    await Launcher.Default.OpenAsync(new Uri(obj.ToString()));
                }
                else
                {
                    obj = "http://" + obj.ToString();
                    await Launcher.Default.OpenAsync(new Uri(obj.ToString()));
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DownloadClicked(object obj)
    {
        if (obj != null)
            try
            {
                await HelperMethods.DownloadFile(obj.ToString());
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }


    private async void SearchClicked(object obj)
    {
        try
        {
            if(DateErrorMessageVisibility)
                return;
            PageTitle = EventPageTitle;
            MenuVisible = true;
            BackVisible = false;
            IsPopUpPage = false;
            EventList = await GetCalendarEventList();
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    public override void BackClicked(object obj)
    {
        base.BackClicked(obj);
        PageTitle = EventPageTitle;
        MenuVisible = true;
        BackVisible = false;
    }

    #endregion
}