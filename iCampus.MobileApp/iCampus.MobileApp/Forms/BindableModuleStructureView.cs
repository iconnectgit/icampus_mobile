using System.ComponentModel;
using Microsoft.Maui.Graphics;
namespace iCampus.MobileApp.Forms;

public class BindableModuleStructureView: INotifyPropertyChanged
    {
        public BindableModuleStructureView() { }
        public short? ModuleId { get; set; }
        public string ModuleName { get; set; }  
        public string ModuleName_1 { get; set; }
        public string ModuleName_2 { get; set; }
        public string ModuleName_3 { get; set; }
        public string ModuleIconCssClass { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleUrl { get; set; }
        public string DefaultChildUrl { get; set; }
        public short? ParentModuleId { get; set; }
        public decimal Sequence { get; set; }
        public short LevelNumber { get; set; }
        public bool IsPwdReVerificationEnabled { get; set; }
        public bool ShowIcon { get; set; }
        public string ModuleImageName { get; set; }
        string _moduleImageUrl;
        public string ModuleImageUrl
        {
            get
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                return _moduleImageUrl;
            }
            set
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                _moduleImageUrl = value;
                OnPropertyChanged("ModuleImageUrl");
            }
        }
        public string ModuleCustomImageName { get; set; }
        public bool IsNewTab { get; set; }
        public string AbsoluteModuleUrl { get; set; }
        public bool IsPlainURL { get; set; }
        public string ActionUrl { get; set; }
        public bool IsUserQueryId { get; set; }
        public bool IsUserQueryEmail { get; set; }
        public string MessageIfDisabled { get; set; }
        public bool IsDisabledAndShowPopup { get; set; }
        public short TypeCode { get; set; }
        public bool IsFooterMenuOnMobile { get; set; }
        public string ModuleBackgroundColor { get; set; }
        public int MobileAppMenuSequence {  get;   set;  }
        public string ModuleShortName { get; set; }

        private byte[] _imageData;
        public byte[] ImageData
        {
            get
            {
                return _imageData;
            }
            set
            {
                _imageData = value;
                OnPropertyChanged("ImageData");
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        Color _menuLabelTextColor;
        public Color MenuLabelTextColor
        {
            get
            {
                _menuLabelTextColor = String.IsNullOrEmpty(ModuleBackgroundColor) ? Color.FromArgb("#707070") : Colors.White;
                return _menuLabelTextColor;
            }
            set
            {
                _menuLabelTextColor = value;
                OnPropertyChanged("MenuLabelTextColor");
            }
        }
        Color _menuBackgroundColor;
        public Color MenuBackgroundColor
        {
            get
            {
                return _menuBackgroundColor;
            }
            set
            {
                _menuBackgroundColor = value;
                OnPropertyChanged("MenuBackgroundColor");
            }
        }
        private bool _newsIconVisibility;
        public bool NewsIconVisibility
        {
            get
            {
                _newsIconVisibility = ShowIcon && (!string.IsNullOrEmpty(ModuleShortName) && ModuleShortName.ToLower().Equals("news"))?true:false;
                if(_newsIconVisibility)
                {
                    HomeIconVisibility = false;
                    FooterIconVisibilityExceptHomeAndNews = false;
                }
                return _newsIconVisibility;
            }
            set
            {
                _newsIconVisibility = value;
                OnPropertyChanged("NewsIconVisibility");
            }
        }
        private bool _homeIconVisibility;
        public bool HomeIconVisibility
        {
            get
            {
                _homeIconVisibility = ShowIcon && (!string.IsNullOrEmpty(ModuleShortName) && ModuleShortName.ToLower().Equals("home")) ? true : false;
                if(_homeIconVisibility)
                {
                    NewsIconVisibility = false;
                    FooterIconVisibilityExceptHomeAndNews = false;
                }
                return _homeIconVisibility;
            }
            set
            {
                _homeIconVisibility = value;
                OnPropertyChanged("HomeIconVisibility");
            }
        }
        private bool _footerIconVisibilityExceptHomeAndNews=true;
        public bool FooterIconVisibilityExceptHomeAndNews
        {
            get
            {
                if(_footerIconVisibilityExceptHomeAndNews)
                {
                    HomeIconVisibility = false;
                    NewsIconVisibility = false;
                }
                return _footerIconVisibilityExceptHomeAndNews;
            }
            set
            {
                _footerIconVisibilityExceptHomeAndNews = value;
                OnPropertyChanged("FooterIconVisibilityExceptHomeAndNews");
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