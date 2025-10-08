using System.ComponentModel;
using System.Text.RegularExpressions;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms;

public class BindableSiteNewsView : INotifyPropertyChanged
    {
		public int SiteNewsId
		{
			get;
			set;
		}

		public int SiteNews
		{
			get;
			set;
		}

		public string NewsTitle
		{
			get;
			set;
		}

		public string NewsDescription
		{
			get;
			set;
		}

		public string NewsData
		{
			get;
			set;
		}

		public string NewsBrief
		{
			get;
			set;
		}

		public DateTime DueDateTime
		{
			get;
			set;
		}

		public string FormattedDueDateTime
		{
			get;
			set;
		}

		public string ImageName
		{
			get;
			set;
		}

		public int DisplayOrder
		{
			get;
			set;
		}

		public short NewsLanguageId
		{
			get;
			set;
		}

		public string AttachmentName
		{
			get;
			set;
		}

		public string AttachmentPath
		{
			get;
			set;
		}

		public bool IsInternalPage
		{
			get;
			set;
		}

		public string ExternalLink
		{
			get;
			set;
		}

		public string PageTitle
		{
			get;
			set;
		}

		public int InternalPageId
		{
			get;
			set;
		}

		public string NewsImageUrl
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

		public bool IsArabic
		{
			get;
			set;
		}

		public bool IsFeaturedNews
		{
			get;
			set;
		}

		public bool IsPinnedNews
		{
			get;
			set;
		}

		public bool IsPlainHTML
		{
			get;
			set;
		}
		public List<string> AttachmentList { get; set; }

        string _newsDataString;
        public string NewsDataString
        {
            get
            {
                return _newsDataString;
            }
            set
            {
				if(NewsData!=null)
                {
					value = Regex.Replace(NewsData.ToString(), "<.*?>", String.Empty);
                }
                _newsDataString = value;
                OnPropertyChanged("NewsDataString");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
		
	}