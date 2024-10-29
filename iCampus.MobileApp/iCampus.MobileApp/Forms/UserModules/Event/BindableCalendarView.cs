using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Event;

public class BindableCalendarView : INotifyPropertyChanged
    {
       
		public int EventId
		{
			get;
			set;
		}

		public short? EventCategoryId
		{
			get;
			set;
		}

		public short? SystemLanguageId
		{
			get;
			set;
		}

		public short? InternalPageId
		{
			get;
			set;
		}

		public bool IsInternalPage
		{
			get;
			set;
		}

		public string EventName
		{
			get;
			set;
		}

		public string EventDatePeriod
		{
			get;
			set;
		}

		public string EventBrief
		{
			get;
			set;
		}

		public string EventText
		{
			get;
			set;
		}

		public string URL
		{
			get;
			set;
		}

		public string AttachmentPath
		{
			get;
			set;
		}

		public bool IsEventCategory
		{
			get;
			set;
		}

		public string EventCategoryName
		{
			get;
			set;
		}

		public string EventCategoryColor
		{
			get;
			set;
		}

		public string EventDescription
		{
			get;
			set;
		}

		public string EventCategoryDescription
		{
			get;
			set;
		}

		public string EventDate
		{
			get;
			set;
		}

		public DateTime EventFromDate
		{
			get;
			set;
		}

		public DateTime EventToDate
		{
			get;
			set;
		}

		public DateTime? CreatedDate
		{
			get;
			set;
		}

		public int NoOfEvent
		{
			get;
			set;
		}

		public string AttachmentName
		{
			get;
			set;
		}

		public string PageTitle
		{
			get;
			set;
		}

		public int AttachmentCount
		{
			get;
			set;
		}

		public List<AttachmentFileView> AttachmentList
		{
			get;
			set;
		}

		public IEnumerable<WebsiteLinkView> WebsiteLinks
		{
			get;
			set;
		}

		public int WebsiteLinksCount
		{
			get;
			set;
		}

		public string HrefAttribute
		{
			get;
			set;
		}

		public string LinkText
		{
			get;
			set;
		}

		private bool _detailsVisibility;
		public bool DetailsVisibility
		{
			get
			{
				return _detailsVisibility;
			}
			set
			{
				_detailsVisibility = value;
				OnPropertyChanged("DetailsVisibility");
			}
		}

		private string _arrowImageSource = "dropdown_gray.png";
		public string ArrowImageSource
		{
			get
			{
				return _arrowImageSource;
			}
			set
			{
				_arrowImageSource = value;
				OnPropertyChanged("ArrowImageSource");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}