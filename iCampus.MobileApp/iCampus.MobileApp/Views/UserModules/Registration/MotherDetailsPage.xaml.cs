using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Maui;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Layouts;

namespace iCampus.MobileApp.Views.UserModules.Registration;

public partial class MotherDetailsPage : ContentPage, INotifyPropertyChanged
	{
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _yesRadioButtonImage;
        public string YesRadioButtonImage
        {
            get
            {
                return _yesRadioButtonImage;
            }
            set
            {
                _yesRadioButtonImage = value;
                OnPropertyChanged("YesRadioButtonImage");
            }
        }
        private string _noRadioButtonImage;
        public string NoRadioButtonImage
        {
            get
            {
                return _noRadioButtonImage;
            }
            set
            {
                _noRadioButtonImage = value;
                OnPropertyChanged("NoRadioButtonImage");
            }
        }

        #endregion
        readonly RegistrationServices registrationService = new RegistrationServices();

        public MotherDetailsPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateDetails();

        }

        private void PopulateDetails()
        {
            DynamicLayout.Children.Clear();
            var motherDetails = new ObservableCollection<BindableFormFieldsView>(
                AppSettings.Current.FamilyDetails.Where(x => x.CategoryId == 9));

            foreach (var motherDetail in motherDetails)
            {
                string labelText = motherDetail.LabelResourceText;
                string fieldValue = motherDetail.FieldValue as string;
                bool isRequired = !string.IsNullOrEmpty(motherDetail.Validators) && motherDetail.Validators.Contains("required");

                var label = new Label
                {
                    Text = labelText,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                };

                var horizontalLayout = new StackLayout
                {
                    IsVisible = motherDetail.IsVisible,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5, 0, 0),
                };

                horizontalLayout.Children.Add(label);

                if (isRequired)
                {
                    var requiredLabel = new Label
                    {
                        Text = "* Required",
                        TextColor = Colors.Red,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(5, 0, 0, 0),
                        FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                    };
                    horizontalLayout.Children.Add(requiredLabel);
                }

                DynamicLayout.Children.Add(horizontalLayout);

                switch (motherDetail.EditorType)
                {
                    case EditorTypes.Textbox:
                        var entry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            //Text = fieldValue,
                            AutomationId = motherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40
                        };
                        entry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: motherDetail));

                        var errorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(motherDetail.Validators))
                            registrationService.ApplyValidators(entry, motherDetail.Validators, errorLabel);

                        var frame = new Frame
                        {
                            Padding = new Thickness(5),
                            HasShadow = false,
                            BorderColor = Colors.Transparent,
                            Margin = new Thickness(0, 5, 0, 5),
                            IsVisible = true,
                            Content = entry
                        };
                        var stackLayoutWithError = new StackLayout
                        {
                            IsVisible = motherDetail.IsVisible,
                            Children = { frame, errorLabel }
                        };

                        if (motherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = motherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutWithError);
                        break;

                    case EditorTypes.LanguageEditorTextBox:
                        var languageEntry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            //Text = fieldValue,
                            AutomationId = motherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40
                        };
                        languageEntry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: motherDetail));

                        var languageErrorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(motherDetail.Validators))
                            registrationService.ApplyValidators(languageEntry, motherDetail.Validators, languageErrorLabel);
                        var languageFrame = new Frame
                        {
                            Padding = new Thickness(5),
                            HasShadow = false,
                            BorderColor = Colors.Transparent,
                            Margin = new Thickness(0, 5, 0, 5),
                            IsVisible = true,
                            Content = languageEntry
                        };
                        var languageStackLayoutWithError = new StackLayout
                        {
                            IsVisible = motherDetail.IsVisible,
                            Children = { languageFrame, languageErrorLabel }
                        };

                        if (motherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = motherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(languageStackLayoutWithError);
                        break;

                    case EditorTypes.Datepicker:
                        var customDatePicker = new DatePicker()
                        {
                            BackgroundColor = Colors.White,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(DatePicker)),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            AutomationId = motherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["DatePickerFontStyle"]
                        };

                        if (DateTime.TryParse(fieldValue, out DateTime parsedDate))
                        {
                            customDatePicker.Date = parsedDate;
                        }
                        customDatePicker.DateSelected += (sender, args) =>
                        {
                            motherDetail.FieldValue = args.NewDate.ToString("dd-MMM-yyyy");
                        };
                        var frameDatePicker = new Frame
                        {
                            HasShadow = false,
                            Padding = new Thickness(0),
                            HeightRequest = 40,
                            BorderColor = Colors.Transparent,
                            IsClippedToBounds = true,
                            CornerRadius = 5,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Content = customDatePicker
                        };

                        var calendarIcon = new CachedImage
                        {
                            Source = "calendar_icon.png",
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            Aspect = Aspect.AspectFit,
                            WidthRequest = 30,
                            HeightRequest = 30
                        };

                        var stackLayout = new StackLayout
                        {
                            IsVisible = motherDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(0),
                            Margin = new Thickness(0, 5, 0, 0),
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };

                        stackLayout.Children.Add(frameDatePicker);
                        stackLayout.Children.Add(calendarIcon);

                        if (motherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = motherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayout);
                        break;

                    case EditorTypes.YesOrNo:

                        var stackLayoutYesOrNo = new StackLayout
                        {
                            IsVisible = motherDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 0),
                            HeightRequest = 40
                        };

                        if (motherDetail.FieldValue.ToString().ToLower() == "true")
                        {
                            YesRadioButtonImage = "selected_radio_button.png";
                            NoRadioButtonImage = "unselected_radio_button.png";
                        }
                        else
                        {
                            YesRadioButtonImage = "unselected_radio_button.png";
                            NoRadioButtonImage = "selected_radio_button.png";
                        }

                        var yesRadioButtonImage = new CachedImage
                        {
                            Source = YesRadioButtonImage,
                            Scale = 0.4,
                            BindingContext = motherDetail
                        };
                        var yesTapGesture = new TapGestureRecognizer();
                        yesTapGesture.Tapped += YesRadioButtonTapped;
                        yesRadioButtonImage.GestureRecognizers.Add(yesTapGesture);

                        var yesLabel = new Label
                        {
                            Text = "Yes",
                            Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                            Margin = new Thickness(0, 5, 0, 0),
                            VerticalOptions = LayoutOptions.Center
                        };

                        var noRadioButtonImage = new CachedImage
                        {
                            Source = NoRadioButtonImage,
                            Scale = 0.4,
                            BindingContext = motherDetail
                        };
                        var noTapGesture = new TapGestureRecognizer();
                        noTapGesture.Tapped += NoRadioButtonTapped;
                        noRadioButtonImage.GestureRecognizers.Add(noTapGesture);

                        var noLabel = new Label
                        {
                            Text = "No",
                            Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                            Margin = new Thickness(0, 5, 0, 0),
                            VerticalOptions = LayoutOptions.Center
                        };

                        stackLayoutYesOrNo.Children.Add(yesRadioButtonImage);
                        stackLayoutYesOrNo.Children.Add(yesLabel);

                        stackLayoutYesOrNo.Children.Add(noRadioButtonImage);
                        stackLayoutYesOrNo.Children.Add(noLabel);

                        if (motherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = motherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutYesOrNo);
                        break;
                    case EditorTypes.AutoComplete:
                        {
                            var frameSelectbox = new Frame
                            {
                                IsVisible = motherDetail.IsVisible,
                                HasShadow = false,
                                Margin = new Thickness(0, 5, 0, 0),
                                Padding = new Thickness(0),
                                IsClippedToBounds = true,
                                BorderColor = Colors.Transparent,
                                HeightRequest = 40,
                                CornerRadius = 5,
                                BindingContext = motherDetail
                            };

                            var grid = new Grid();

                            var noUnderlineEntry = new NoUnderlineEntry
                            {
                                Text = Convert.ToString(motherDetail.FieldText),
                                Placeholder = "Select",
                                Style = (Style)Application.Current.Resources["EntryFontStyle"],
                                TextColor = Colors.Gray,
                                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(5, 0, 0, 0) : new Thickness(0),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 40,
                                //IsReadOnly = true
                            };
                            noUnderlineEntry.TextChanged += (s, e) => SearchEntryTextChanged(s, e, motherDetail);

                            var cachedImage = new CachedImage
                            {
                                Source = "search.png",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Aspect = Aspect.AspectFit,
                                Margin = new Thickness(0, 0, 5, 0),
                                HeightRequest = 20,
                                WidthRequest = 20
                            };

                            grid.Children.Add(noUnderlineEntry);
                            grid.Children.Add(cachedImage);

                            var tapGestureRecognizer = new TapGestureRecognizer();
                            tapGestureRecognizer.Tapped += FrameTapped;
                            frameSelectbox.GestureRecognizers.Add(tapGestureRecognizer);

                            frameSelectbox.Content = grid;
                            DynamicLayout.Children.Add(frameSelectbox);
                            var listView = new SwipeGestureListview
                            {
                                HasUnevenRows = true,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                SeparatorVisibility = SeparatorVisibility.Default,
                                Margin = new Thickness(5, 0, 5, 0),
                                BindingContext = motherDetail
                            };

                            listView.SetBinding(ListView.IsVisibleProperty, new Binding("ListViewVisible", source: motherDetail));
                            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GeneralSelectList", source: motherDetail));
                            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", source: motherDetail));
                            listView.SetBinding(ListView.HeightRequestProperty, new Binding("ListViewHeightRequest", source: motherDetail));

                            listView.ItemTemplate = new DataTemplate(() =>
                            {
                                var listViewStackLayout = new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Padding = new Thickness(5)
                                };

                                var listViewLabel = new Label
                                {
                                    Style = (Style)Application.Current.Resources["DescriptionLabelStyle"],
                                    TextColor = (Color)Application.Current.Resources["GrayTextColor"]
                                };
                                listViewLabel.SetBinding(Label.TextProperty, new Binding("ItemName"));

                                var tapGestureRecognizerList = new TapGestureRecognizer();
                                tapGestureRecognizerList.Tapped += ListItemTapped;
                                listViewLabel.GestureRecognizers.Add(tapGestureRecognizerList);

                                listViewStackLayout.Children.Add(listViewLabel);
                                return new ViewCell { View = listViewStackLayout };
                            });

                            if (motherDetail.HasLinkedChildren > 0)
                            {
                                var parentFields = motherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));
                                foreach (var field in parentFields)
                                {
                                    HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                                }
                            }

                            DynamicLayout.Children.Add(listView);
                            break;
                        }

                    case EditorTypes.Selectbox:
                    {
                        var frameSelectbox = new Frame
                        {
                            IsVisible = motherDetail.IsVisible,
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 0),
                            Padding = new Thickness(5,0,0,0),
                            IsClippedToBounds = true,
                            HeightRequest = 40,
                            BorderColor = Colors.Transparent,
                            CornerRadius = 5,
                            BindingContext = motherDetail
                        };

                        var grid = new Grid
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                            ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = GridLength.Star },
                                new ColumnDefinition { Width = GridLength.Auto }
                            }
                        };

                        var noUnderlineEntry = new NoUnderlineEntry
                        {
                            Text = Convert.ToString(motherDetail.FieldText),
                            Placeholder = "Select",
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            TextColor = Colors.Gray,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HeightRequest = 40,
                            IsReadOnly = true
                        };
                        
                        var cachedImage = new CachedImage
                        {
                            Source = "dropdown_gray_picker.png",
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.EndAndExpand,
                            Aspect = Aspect.AspectFit,
                            HeightRequest = 20,
                            WidthRequest = 20
                        };
                        grid.Children.Add(noUnderlineEntry);
                        Grid.SetRow(noUnderlineEntry, 0); 
                        Grid.SetColumn(noUnderlineEntry, 0); 

                        grid.Children.Add(cachedImage);
                        Grid.SetRow(cachedImage, 0); 
                        Grid.SetColumn(cachedImage, 1); 
                       

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += FrameTapped; 
                        frameSelectbox.GestureRecognizers.Add(tapGestureRecognizer);

                        frameSelectbox.Content = grid; 

                        DynamicLayout.Children.Add(frameSelectbox);

                       var listView = new ListView
                        {
                            HasUnevenRows = true,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            SeparatorVisibility = SeparatorVisibility.Default,
                            Margin = new Thickness(0, 5, 0, 0),
                            BindingContext = motherDetail,
                        };

                        listView.SetBinding(IsVisibleProperty, new Binding("ListViewVisible", source: motherDetail));
                        listView.SetBinding(ListView.ItemsSourceProperty,
                            new Binding("GeneralSelectList", source: motherDetail));
                        listView.SetBinding(ListView.SelectedItemProperty,
                            new Binding("SelectedItem", source: motherDetail));
                        listView.SetBinding(HeightRequestProperty,
                            new Binding("ListViewHeightRequest", source: motherDetail));

                        listView.ItemTemplate = new DataTemplate(() =>
                        {
                            var listViewStackLayout = new StackLayout
                            {
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Padding = new Thickness(5)
                            };

                            var listViewLabel = new Label
                            {
                                Style = (Style)Application.Current.Resources["DescriptionLabelStyle"],
                                TextColor = (Color)Application.Current.Resources["GrayTextColor"]
                            };
                            listViewLabel.SetBinding(Label.TextProperty, new Binding("ItemName"));

                            var tapGestureRecognizerList = new TapGestureRecognizer();
                            tapGestureRecognizerList.Tapped += ListItemTapped; 
                            listViewLabel.GestureRecognizers.Add(tapGestureRecognizerList);

                            listViewStackLayout.Children.Add(listViewLabel);
                            return new ViewCell { View = listViewStackLayout };
                        });

                        if (motherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = motherDetails
                                .Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                                            f.VisibilityLinkedField.StartsWith(motherDetail.FieldName + "-"));

                            foreach (var field in parentFields)
                                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail,
                                    AppSettings.Current.FamilyDetails);
                        }
                        DynamicLayout.Children.Add(listView);
                        break;
                    }
                }
            }
        }

        private void YesRadioButtonTapped(object sender, EventArgs e)
        {
            var image = sender as CachedImage;
            var motherDetail = image?.BindingContext as BindableFormFieldsView;
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            motherDetail.FieldValue = true;
            HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void NoRadioButtonTapped(object sender, EventArgs e)
        {
            var image = sender as CachedImage;
            var motherDetail = image?.BindingContext as BindableFormFieldsView;
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            motherDetail.FieldValue = false;
            HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void SearchEntryTextChanged(object sender, TextChangedEventArgs e, BindableFormFieldsView motherDetail)
        {
            if (e.NewTextValue.Length >= 3)
            {
                var filteredList = motherDetail.MasterList
                    .Where(item => item.ItemName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                motherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(filteredList);
            }
            else
            {
                motherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(motherDetail.MasterList);
            }
            motherDetail.ListViewVisible = true;
            motherDetail.ListViewHeightRequest = motherDetail.GeneralSelectList.Count * 30;
            if (motherDetail.ListViewHeightRequest > 300)
                motherDetail.ListViewHeightRequest = 300;
        }

        private async void FrameTapped(object sender, EventArgs e)
        {
            try
            {
                BindableFormFieldsView motherDetail = null;
                if (sender is BoxView boxView)
                {
                    motherDetail = boxView.BindingContext as BindableFormFieldsView;
                }
                else if (sender is Frame frame)
                {
                    motherDetail = frame.BindingContext as BindableFormFieldsView;
                }

                if (motherDetail == null)
                {
                    return;
                }
                string listUrl = motherDetail.ListUrl;
                if (listUrl.StartsWith("/"))
                {
                    listUrl = listUrl.TrimStart('/');
                }
                string portalUrl = AppSettings.Current.PortalUrl;
                string cacheKeyPrefix = motherDetail.DataFieldName;
                motherDetail.MasterList = motherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(await ApiHelper.GetObjectList<SelectionListView>(listUrl,
                        apiUrl: portalUrl, cacheType: ApiHelper.CacheTypeParam.LoadFromCache, cacheKeyPrefix: cacheKeyPrefix));
                motherDetail.ListViewVisible = !motherDetail.ListViewVisible;
                motherDetail.ListViewHeightRequest = motherDetail.GeneralSelectList.Count * 30;
                if (motherDetail.ListViewHeightRequest > 300)
                    motherDetail.ListViewHeightRequest = 300;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }

        }

        private void ListItemTapped(object sender, EventArgs e)
        {
            var label = sender as Label;
            var tappedItem = label?.BindingContext;
            var motherDetail = (label?.Parent?.Parent?.Parent?.BindingContext as BindableFormFieldsView);

            if (motherDetail != null && tappedItem != null)
            {
                var itemIdProperty = tappedItem.GetType().GetProperty("ItemId");
                var itemNameProperty = tappedItem.GetType().GetProperty("ItemName");
                if (itemIdProperty != null && itemNameProperty != null)
                {
                    var itemId = (int)itemIdProperty.GetValue(tappedItem);
                    var itemName = itemNameProperty.GetValue(tappedItem) as string;

                    motherDetail.FieldValue = itemId.ToString();
                    motherDetail.FieldText = itemName;
                }
                motherDetail.ListViewVisible = false;
                HelperMethods.ShowHideVisibilityLinkedFields(motherDetail, AppSettings.Current.FamilyDetails);
                PopulateDetails();
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var registrationService = new RegistrationServices();
            var filteredFamilyDetails = new ObservableCollection<BindableFormFieldsView>(AppSettings.Current.FamilyDetails.Where(f => f.CategoryId == 9));
            registrationService.SaveFamilyDetails(filteredFamilyDetails);
        }
    }