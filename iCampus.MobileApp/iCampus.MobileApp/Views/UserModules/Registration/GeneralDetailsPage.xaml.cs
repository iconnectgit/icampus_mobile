using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Provider;
using FFImageLoading.Maui;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Maps;

namespace iCampus.MobileApp.Views.UserModules.Registration;

public partial class GeneralDetailsPage : ContentPage, INotifyPropertyChanged
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

        public GeneralDetailsPage ()
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
            var generalDetails = new ObservableCollection<BindableFormFieldsView>(
                AppSettings.Current.FamilyDetails.Where(x => x.CategoryId == 10));

            foreach (var generalDetail in generalDetails)
            {
                string labelText = generalDetail.LabelResourceText;
                string fieldValue = generalDetail.FieldValue as string;
                bool isRequired = !string.IsNullOrEmpty(generalDetail.Validators) && generalDetail.Validators.Contains("required");

                var label = new Label
                {
                    Text = labelText,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                };

                var horizontalLayout = new StackLayout
                {
                    IsVisible = generalDetail.IsVisible,
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

                switch (generalDetail.EditorType)
                {
                    case EditorTypes.Textbox:
                    case EditorTypes.Textarea:
                        var entry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            //Text = fieldValue,
                            AutomationId = generalDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40
                        };
                        entry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: generalDetail));

                        var errorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(generalDetail.Validators))
                            registrationService.ApplyValidators(entry, generalDetail.Validators, errorLabel);

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
                            IsVisible = generalDetail.IsVisible,
                            Children = { frame, errorLabel }
                        };

                        if (generalDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutWithError);
                        break;

                    case EditorTypes.LanguageEditorTextBox:
                        var languageEntry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Text = fieldValue,
                            AutomationId = generalDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40
                        };
                        languageEntry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: generalDetail));

                        var languageErrorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(generalDetail.Validators))
                            registrationService.ApplyValidators(languageEntry, generalDetail.Validators, languageErrorLabel);
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
                            IsVisible = generalDetail.IsVisible,
                            Children = { languageFrame, languageErrorLabel }
                        };

                        if (generalDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
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
                            AutomationId = generalDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["DatePickerFontStyle"]
                        };

                        if (DateTime.TryParse(fieldValue, out DateTime parsedDate))
                        {
                            customDatePicker.Date = parsedDate;
                        }
                        customDatePicker.DateSelected += (sender, args) =>
                        {
                            generalDetail.FieldValue = args.NewDate.ToString("dd-MMM-yyyy");
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
                            IsVisible = generalDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(0),
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };

                        stackLayout.Children.Add(frameDatePicker);
                        stackLayout.Children.Add(calendarIcon);

                        if (generalDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayout);
                        break;

                    case EditorTypes.YesOrNo:

                        var stackLayoutYesOrNo = new StackLayout
                        {
                            IsVisible = generalDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 0),
                            HeightRequest = 40
                        };

                        if (generalDetail.FieldValue.ToString().ToLower() == "true")
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
                            BindingContext = generalDetail
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
                            BindingContext = generalDetail
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

                        if (generalDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutYesOrNo);
                        break;
                    case EditorTypes.AutoComplete:
                        {
                            var frameSelectbox = new Frame
                            {
                                IsVisible = generalDetail.IsVisible,
                                HasShadow = false,
                                Margin = new Thickness(0, 5, 0, 0),
                                Padding = new Thickness(0),
                                IsClippedToBounds = true,
                                HeightRequest = 40,
                                BorderColor = Colors.Transparent,
                                CornerRadius = 5,
                                BindingContext = generalDetail
                            };

                            var grid = new Grid();

                            var noUnderlineEntry = new NoUnderlineEntry
                            {
                                Text = Convert.ToString(generalDetail.FieldText),
                                Placeholder = "Select",
                                Style = (Style)Application.Current.Resources["EntryFontStyle"],
                                TextColor = Colors.Gray,
                                Margin = new Thickness(0),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 40,
                                //IsReadOnly = true
                            };
                            noUnderlineEntry.TextChanged += (s, e) => SearchEntryTextChanged(s, e, generalDetail);

                            var cachedImage = new CachedImage
                            {
                                Source = "search.png",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Aspect = Aspect.AspectFit,
                                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(5, 0, 0, 0) : new Thickness(0),
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
                                BindingContext = generalDetail
                            };

                            listView.SetBinding(ListView.IsVisibleProperty, new Binding("ListViewVisible", source: generalDetail));
                            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GeneralSelectList", source: generalDetail));
                            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", source: generalDetail));
                            listView.SetBinding(ListView.HeightRequestProperty, new Binding("ListViewHeightRequest", source: generalDetail));

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

                            if (generalDetail.HasLinkedChildren > 0)
                            {
                                var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                                foreach (var field in parentFields)
                                {
                                    HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                                }
                            }

                            DynamicLayout.Children.Add(listView);
                            break;
                        }

                    case EditorTypes.Selectbox:
                        {
                            var frameSelectbox = new Frame
                            {
                                IsVisible = generalDetail.IsVisible,
                                HasShadow = false,
                                BorderColor = Colors.Transparent,
                                Margin = new Thickness(0, 5, 0, 0),
                                Padding = new Thickness(0),
                                IsClippedToBounds = true,
                                HeightRequest = 40,
                                CornerRadius = 5,
                                BindingContext = generalDetail
                            };

                            var grid = new Grid
                            {
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            };

                            var noUnderlineEntry = new NoUnderlineEntry
                            {
                                Text = Convert.ToString(generalDetail.FieldText),
                                Placeholder = "Select",
                                Style = (Style)Application.Current.Resources["EntryFontStyle"],
                                TextColor = Colors.Gray,
                                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(5, 0, 0, 0) : new Thickness(0),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 40,
                                IsReadOnly = true
                            };

                            var cachedImage = new CachedImage
                            {
                                Source = "dropdown_gray_picker.png",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Aspect = Aspect.AspectFit,
                                HeightRequest = 18,
                                WidthRequest = 18
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
                            var listView = new SwipeGestureListview
                            {
                                HasUnevenRows = true,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                SeparatorVisibility = SeparatorVisibility.Default,
                                Margin = new Thickness(0, 5, 0, 0),
                                BindingContext = generalDetail
                            };

                            listView.SetBinding(ListView.IsVisibleProperty, new Binding("ListViewVisible", source: generalDetail));
                            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GeneralSelectList", source: generalDetail));
                            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", source: generalDetail));
                            listView.SetBinding(ListView.HeightRequestProperty, new Binding("ListViewHeightRequest", source: generalDetail));

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
                                tapGestureRecognizerList.Tapped += ListItemTapped; // Use event handler instead of command
                                listViewLabel.GestureRecognizers.Add(tapGestureRecognizerList);

                                listViewStackLayout.Children.Add(listViewLabel);
                                return new ViewCell { View = listViewStackLayout };
                            });

                            if (generalDetail.HasLinkedChildren > 0)
                            {
                                var parentFields = generalDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(generalDetail.FieldName + "-"));
                                foreach (var field in parentFields)
                                {
                                    HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                                }
                            }

                            DynamicLayout.Children.Add(listView);
                            break;
                        }
                    case EditorTypes.GoogleMap:
                        {
                            var entryGoogleMap = new NoUnderlineEntry
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Text = fieldValue,
                                AutomationId = generalDetail.DataFieldName,
                                Style = (Style)Application.Current.Resources["EntryFontStyle"],
                                Margin = new Thickness(0),
                                HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40
                            };
                            
                            var frameGoogleMap = new Frame
                            {
                                Padding = new Thickness(5),
                                HasShadow = false,
                                BorderColor = Colors.Transparent,
                                Margin = new Thickness(0, 5, 0, 5),
                                IsVisible = true,
                                Content = entryGoogleMap
                            };

                            //frameGoogleMap.SetBinding(IsVisibleProperty, new Binding("AttachmentFiles.Count", BindingMode.OneWay, new IntToBoolValueConverter(), source: fatherAttachment));


                            DynamicLayout.Children.Add(frameGoogleMap);
                            var themeColor = Color.FromHex(AppSettings.Current.Settings.ThemeColor);
                            var buttonStackLayout = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Spacing = 10
                            };
                            // Create the first button
                            var currentLocationButton = new Button
                            {
                                Text = "Current Location",
                                FontSize = 14,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.White,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                BackgroundColor = themeColor,
                                BorderColor = themeColor,
                                HeightRequest = 40,
                                CornerRadius = 6
                            };
                            currentLocationButton.Clicked += (sender, e) => OnCurrentLocationButtonClickedAsync(generalDetail);

                            // Create the second button
                            var resetButton = new Button
                            {
                                Text = "Reset",
                                FontSize = 14,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.White,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                BackgroundColor = themeColor,
                                BorderColor = themeColor,
                                HeightRequest = 40,
                                CornerRadius = 6
                            };
                            resetButton.Clicked += (sender, e) => OnResetButtonClicked(generalDetail);

                            // Add the buttons to the horizontal stack layout
                            buttonStackLayout.Children.Add(currentLocationButton);
                            buttonStackLayout.Children.Add(resetButton);

                            DynamicLayout.Children.Add(buttonStackLayout);
                            break;
                        }
                }
            }
        }

        private void YesRadioButtonTapped(object sender, EventArgs e)
        {
            var image = sender as CachedImage;
            var generalDetail = image?.BindingContext as BindableFormFieldsView;
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            generalDetail.FieldValue = true;
            HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void NoRadioButtonTapped(object sender, EventArgs e)
        {
            var image = sender as CachedImage;
            var generalDetail = image?.BindingContext as BindableFormFieldsView;
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            generalDetail.FieldValue = false;
            HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void SearchEntryTextChanged(object sender, TextChangedEventArgs e, BindableFormFieldsView generalDetail)
        {
            if (e.NewTextValue.Length >= 3)
            {
                var filteredList = generalDetail.MasterList
                    .Where(item => item.ItemName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                generalDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(filteredList);
            }
            else
            {
                generalDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(generalDetail.MasterList);
            }
            generalDetail.ListViewVisible = true;
            generalDetail.ListViewHeightRequest = generalDetail.GeneralSelectList.Count * 30;
            if (generalDetail.ListViewHeightRequest > 300)
                generalDetail.ListViewHeightRequest = 300;
        }

        private async void FrameTapped(object sender, EventArgs e)
        {
            try
            {
                BindableFormFieldsView generalDetail = null;
                if (sender is BoxView boxView)
                {
                    generalDetail = boxView.BindingContext as BindableFormFieldsView;
                }
                else if (sender is Frame frame)
                {
                    generalDetail = frame.BindingContext as BindableFormFieldsView;
                }

                if (generalDetail == null)
                {
                    return;
                }
                string listUrl = generalDetail.ListUrl;
                if (listUrl.StartsWith("/"))
                {
                    listUrl = listUrl.TrimStart('/');
                }
                string portalUrl = AppSettings.Current.PortalUrl;
                string cacheKeyPrefix = generalDetail.DataFieldName;
                generalDetail.MasterList = generalDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(await ApiHelper.GetObjectList<SelectionListView>(listUrl,
                        apiUrl: portalUrl, cacheType: ApiHelper.CacheTypeParam.LoadFromCache, cacheKeyPrefix: cacheKeyPrefix));
                generalDetail.ListViewVisible = !generalDetail.ListViewVisible;
                generalDetail.ListViewHeightRequest = generalDetail.GeneralSelectList.Count * 30;
                if (generalDetail.ListViewHeightRequest > 300)
                    generalDetail.ListViewHeightRequest = 300;
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
            var generalDetail = (label?.Parent?.Parent?.Parent?.BindingContext as BindableFormFieldsView);

            if (generalDetail != null && tappedItem != null)
            {
                var itemIdProperty = tappedItem.GetType().GetProperty("ItemId");
                var itemNameProperty = tappedItem.GetType().GetProperty("ItemName");
                if (itemIdProperty != null && itemNameProperty != null)
                {
                    var itemId = (int)itemIdProperty.GetValue(tappedItem);
                    var itemName = itemNameProperty.GetValue(tappedItem) as string;

                    generalDetail.FieldValue = itemId.ToString();
                    generalDetail.FieldText = itemName;
                }
                generalDetail.ListViewVisible = false;
                HelperMethods.ShowHideVisibilityLinkedFields(generalDetail, AppSettings.Current.FamilyDetails);
                PopulateDetails();
            }
        }

        private async Task OnCurrentLocationButtonClickedAsync(BindableFormFieldsView generalDetails)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    var data = AppSettings.Current.FormData;

                    // If using Position from Maps, ensure the namespace is correct
                    var position = new Location(location.Latitude, location.Longitude); // Make sure Position is from Microsoft.Maui.Controls.Maps
                    generalDetails.FieldValue = $"{location.Latitude} , {location.Longitude}";

                    foreach (var fieldDict in AppSettings.Current.FormData)
                    {
                        if (fieldDict.ContainsKey("Latitude"))
                        {
                            fieldDict["Latitude"] = location.Latitude;
                        }

                        if (fieldDict.ContainsKey("Longitude"))
                        {
                            fieldDict["Longitude"] = location.Longitude;
                        }
                    }
                    PopulateDetails();

                    await OpenGoogleMapsAsync(location.Latitude, location.Longitude);
                }
                else
                {
                    await DisplayAlert("Error", "Unable to get location", "OK");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }
        }

        private async Task OpenGoogleMapsAsync(double latitude, double longitude)
        {
            var locationUri = new Uri($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}");
            await Launcher.OpenAsync(locationUri);
        }

        private void OnResetButtonClicked(BindableFormFieldsView generalDetails)
        {
            generalDetails.FieldValue = "0-0";
            PopulateDetails();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var registrationService = new RegistrationServices();
            var filteredFamilyDetails = new ObservableCollection<BindableFormFieldsView>(AppSettings.Current.FamilyDetails.Where(f => f.CategoryId == 10));
            registrationService.SaveFamilyDetails(filteredFamilyDetails);
        }

    }