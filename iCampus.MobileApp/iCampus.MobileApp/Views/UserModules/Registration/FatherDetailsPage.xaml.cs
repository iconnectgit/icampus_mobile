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

public partial class FatherDetailsPage :  ContentPage , INotifyPropertyChanged
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

        public FatherDetailsPage ()
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
            var fatherDetails = new ObservableCollection<BindableFormFieldsView>(
                AppSettings.Current.FamilyDetails.Where(x => x.CategoryId == 8));

            foreach (var fatherDetail in fatherDetails)
            {
                string labelText = fatherDetail.LabelResourceText;
                string fieldValue = fatherDetail.FieldValue as string;
                bool isRequired = !string.IsNullOrEmpty(fatherDetail.Validators) && fatherDetail.Validators.Contains("required");

                var label = new Label
                {
                    Text = labelText,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                };

                var horizontalLayout = new StackLayout
                {
                    IsVisible = fatherDetail.IsVisible,
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

                switch (fatherDetail.EditorType)
                {
                    case EditorTypes.Textbox:
                        var entry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            //Text = fieldValue,
                            AutomationId = fatherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40,
                            BindingContext = fatherDetail
                        };
                        entry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: fatherDetail));

                        var errorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(fatherDetail.Validators))
                            registrationService.ApplyValidators(entry, fatherDetail.Validators, errorLabel);

                        var frame = new Frame
                        {
                            Padding = new Thickness(5),
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 5),
                            IsVisible = true,
                            BorderColor = Colors.Transparent,
                            Content = entry
                        };
                        var stackLayoutWithError = new StackLayout
                        {
                            IsVisible = fatherDetail.IsVisible,
                            Children = { frame, errorLabel }
                        };

                        if(fatherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = fatherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutWithError);
                        break;

                    case EditorTypes.LanguageEditorTextBox:
                        var languageEntry = new NoUnderlineEntry
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            //Text = fieldValue,
                            AutomationId = fatherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["EntryFontStyle"],
                            Margin = new Thickness(0),
                            HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40,
                            BindingContext = fatherDetail
                        };
                        languageEntry.SetBinding(NoUnderlineEntry.TextProperty, new Binding("FieldValue", BindingMode.TwoWay, source: fatherDetail));

                        var languageErrorLabel = new Label
                        {
                            TextColor = Colors.Red,
                            IsVisible = false,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                        };
                        if (!string.IsNullOrEmpty(fatherDetail.Validators))
                            registrationService.ApplyValidators(languageEntry, fatherDetail.Validators, languageErrorLabel);
                        var languageFrame = new Frame
                        {
                            Padding = new Thickness(5),
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 5),
                            IsVisible = true,
                            BorderColor = Colors.Transparent,
                            Content = languageEntry
                        };
                        var languageStackLayoutWithError = new StackLayout
                        {
                            IsVisible = fatherDetail.IsVisible,
                            Children = { languageFrame, languageErrorLabel }
                        };

                        if (fatherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = fatherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
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
                            AutomationId = fatherDetail.DataFieldName,
                            Style = (Style)Application.Current.Resources["DatePickerFontStyle"]
                        };

                        if (DateTime.TryParse(fieldValue, out DateTime parsedDate))
                        {
                            customDatePicker.Date = parsedDate;
                        }

                        customDatePicker.DateSelected += (sender, args) =>
                        {
                            fatherDetail.FieldValue = args.NewDate.ToString("dd-MMM-yyyy");
                        };

                        var frameDatePicker = new Frame
                        {
                            HasShadow = false,
                            Padding = new Thickness(0),
                            HeightRequest = 40,
                            IsClippedToBounds = true,
                            CornerRadius = 5,
                            Margin = new Thickness(0, 5, 0, 5),
                            BorderColor = Colors.Transparent,
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
                            IsVisible = fatherDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(0),
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };

                        stackLayout.Children.Add(frameDatePicker);
                        stackLayout.Children.Add(calendarIcon);

                        if (fatherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = fatherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayout);
                        break;

                    case EditorTypes.YesOrNo:
                        
                        var stackLayoutYesOrNo = new StackLayout
                        {
                            IsVisible = fatherDetail.IsVisible,
                            Orientation = StackOrientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 0),
                            HeightRequest = 40
                        };

                        if (fatherDetail.FieldValue.ToString().ToLower() == "true")
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
                            BindingContext = fatherDetail
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
                            BindingContext = fatherDetail
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

                        if (fatherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = fatherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));
                            foreach (var field in parentFields)
                            {
                                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
                            }
                        }

                        DynamicLayout.Children.Add(stackLayoutYesOrNo);
                        break;
                    case EditorTypes.AutoComplete:
                        {
                            var frameSelectbox = new Frame
                            {
                                IsVisible = fatherDetail.IsVisible,
                                HasShadow = false,
                                Margin = new Thickness(0, 5, 0, 0),
                                Padding = new Thickness(0),
                                IsClippedToBounds = true,
                                HeightRequest = 40,
                                BorderColor = Colors.Transparent,
                                CornerRadius = 5,
                                BindingContext = fatherDetail
                            };

                            var grid = new Grid();

                            var noUnderlineEntry = new NoUnderlineEntry
                            {
                                Text = Convert.ToString(fatherDetail.FieldText),
                                Placeholder = "Select",
                                Style = (Style)Application.Current.Resources["EntryFontStyle"],
                                TextColor = Colors.Gray,
                                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(5,0,0,0) : new Thickness(0),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 40,
                                //IsReadOnly = true
                            };
                            noUnderlineEntry.TextChanged += (s, e) => SearchEntryTextChanged(s, e, fatherDetail);

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
                                BindingContext = fatherDetail
                            };

                            listView.SetBinding(ListView.IsVisibleProperty, new Binding("ListViewVisible", source: fatherDetail));
                            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GeneralSelectList", source: fatherDetail));
                            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", source: fatherDetail));
                            listView.SetBinding(ListView.HeightRequestProperty, new Binding("ListViewHeightRequest", source: fatherDetail));

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

                            if (fatherDetail.HasLinkedChildren > 0)
                            {
                                var parentFields = fatherDetails.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));
                                foreach (var field in parentFields)
                                {
                                    HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
                                }
                            }

                            DynamicLayout.Children.Add(listView);
                            break;
                        }

                    case EditorTypes.Selectbox:
                    {
                        var frameSelectbox = new Frame
                        {
                            IsVisible = fatherDetail.IsVisible,
                            HasShadow = false,
                            Margin = new Thickness(0, 5, 0, 0),
                            Padding = new Thickness(5,0,0,0),
                            IsClippedToBounds = true,
                            HeightRequest = 40,
                            BorderColor = Colors.Transparent,
                            CornerRadius = 5,
                            BindingContext = fatherDetail
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
                            Text = Convert.ToString(fatherDetail.FieldText),
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

                        var listView = new CollectionView
                        {
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            Margin = new Thickness(0, 5, 0, 0),
                            SelectionMode = SelectionMode.Single,
                            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                            {
                                ItemSpacing = 5
                            }
                        };

                        listView.SetBinding(IsVisibleProperty, new Binding("ListViewVisible", source: fatherDetail));
                        listView.SetBinding(CollectionView.ItemsSourceProperty, new Binding("GeneralSelectList", source: fatherDetail));
                        listView.SetBinding(CollectionView.SelectionChangedCommandProperty, new Binding("SelectedItem", source: fatherDetail));
                        listView.SetBinding(HeightRequestProperty, new Binding("ListViewHeightRequest", source: fatherDetail));

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

                        if (fatherDetail.HasLinkedChildren > 0)
                        {
                            var parentFields = fatherDetails
                                .Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                                            f.VisibilityLinkedField.StartsWith(fatherDetail.FieldName + "-"));

                            foreach (var field in parentFields)
                                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail,
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
            var fatherDetail = image?.BindingContext as BindableFormFieldsView; 
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            fatherDetail.FieldValue = true;
            HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void NoRadioButtonTapped(object sender, EventArgs e)
        {
            var image = sender as CachedImage;
            var fatherDetail = image?.BindingContext as BindableFormFieldsView; 
            OnPropertyChanged(nameof(YesRadioButtonImage));
            OnPropertyChanged(nameof(NoRadioButtonImage));
            fatherDetail.FieldValue = false;
            HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
            PopulateDetails();
        }

        private void SearchEntryTextChanged(object sender, TextChangedEventArgs e, BindableFormFieldsView fatherDetail)
        {
            if (e.NewTextValue.Length >= 3)
            {
                var filteredList = fatherDetail.MasterList
                    .Where(item => item.ItemName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                fatherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(filteredList);
            }
            else
            {
                fatherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(fatherDetail.MasterList);
            }
            fatherDetail.ListViewVisible = true;
            fatherDetail.ListViewHeightRequest = fatherDetail.GeneralSelectList.Count * 30;
            if (fatherDetail.ListViewHeightRequest > 300)
                fatherDetail.ListViewHeightRequest = 300;
        }

        private async void FrameTapped(object sender, EventArgs e)
        {
            try
            {
                BindableFormFieldsView fatherDetail = null;
                if (sender is BoxView boxView)
                {
                    fatherDetail = boxView.BindingContext as BindableFormFieldsView;
                }
                else if (sender is Frame frame)
                {
                    fatherDetail = frame.BindingContext as BindableFormFieldsView;
                }

                if (fatherDetail == null)
                {
                    return; 
                }
                string listUrl = fatherDetail.ListUrl;
                if (listUrl.StartsWith("/"))
                {
                    listUrl = listUrl.TrimStart('/');
                }
                string portalUrl = AppSettings.Current.PortalUrl;
                string cacheKeyPrefix = fatherDetail.DataFieldName;
                fatherDetail.MasterList = fatherDetail.GeneralSelectList = new ObservableCollection<SelectionListView>(await ApiHelper.GetObjectList<SelectionListView>(listUrl,
                        apiUrl: portalUrl, cacheType: ApiHelper.CacheTypeParam.LoadFromCache, cacheKeyPrefix: cacheKeyPrefix));
                
                fatherDetail.ListViewVisible = !fatherDetail.ListViewVisible; 
                fatherDetail.ListViewHeightRequest = fatherDetail.GeneralSelectList.Count * 30; 
                if (fatherDetail.ListViewHeightRequest > 300) 
                    fatherDetail.ListViewHeightRequest = 300;

                //ForceNativeTableUpdate(listView);
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
            var fatherDetail = (label?.Parent?.Parent?.Parent?.BindingContext as BindableFormFieldsView);

            if (fatherDetail != null && tappedItem != null)
            {
                var itemIdProperty = tappedItem.GetType().GetProperty("ItemId");
                var itemNameProperty = tappedItem.GetType().GetProperty("ItemName");
                if (itemIdProperty != null && itemNameProperty != null)
                {
                    var itemId = (int)itemIdProperty.GetValue(tappedItem);
                    var itemName = itemNameProperty.GetValue(tappedItem) as string;

                    fatherDetail.FieldValue = itemId.ToString();
                    fatherDetail.FieldText = itemName;
                }
                fatherDetail.ListViewVisible = false;
                HelperMethods.ShowHideVisibilityLinkedFields(fatherDetail, AppSettings.Current.FamilyDetails);
                PopulateDetails();
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var filteredFamilyDetails = new ObservableCollection<BindableFormFieldsView>(AppSettings.Current.FamilyDetails.Where(f => f.CategoryId == 8));
            registrationService.SaveFamilyDetails(filteredFamilyDetails);
        }
        public void ForceNativeTableUpdate(ListView listView)
        {
            if (listView.Handler != null)
            {
#if ANDROID
                var nativeListView = listView.Handler.PlatformView as AndroidX.RecyclerView.Widget.RecyclerView;
                nativeListView?.GetAdapter()?.NotifyDataSetChanged();
#elif IOS || MACCATALYST
            var nativeListView = listView.Handler.PlatformView as UIKit.UITableView;
            nativeListView?.ReloadData();
#endif
            }
        }

        //private void ApplyValidators(NoUnderlineEntry entry, string validators, Label errorLabel)
        //{
        //    string[] validatorList = !string.IsNullOrEmpty(validators) ? validators.Replace(" ", "").Split(',') : new string[] { };

        //    foreach (var validator in validatorList)
        //    {
        //        if (validator.Equals("required"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (string.IsNullOrEmpty(e.NewTextValue))
        //                {
        //                    errorLabel.Text = "This field is required.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.Equals("email"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (!Regex.IsMatch(e.NewTextValue, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        //                {
        //                    errorLabel.Text = "Please enter a valid email address.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.Equals("phone-uae"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (!Regex.IsMatch(e.NewTextValue, @"^((05)[0-9]{8})$"))
        //                {
        //                    errorLabel.Text = "Please enter a valid phone number.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.Equals("phone-oman"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (!Regex.IsMatch(e.NewTextValue, @"^([0-9]{8})$"))
        //                {
        //                    errorLabel.Text = "Please enter a valid phone number.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.Equals("phone"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (!Regex.IsMatch(e.NewTextValue, @"^([0-9]{10})$"))
        //                {
        //                    errorLabel.Text = "Please enter a valid phone number.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.Equals("number"))
        //        {
        //            entry.TextChanged += (s, e) =>
        //            {
        //                if (!Regex.IsMatch(e.NewTextValue, @"^[0-9]*$"))
        //                {
        //                    errorLabel.Text = "Please enter a valid number.";
        //                    errorLabel.IsVisible = true;
        //                }
        //                else
        //                {
        //                    errorLabel.IsVisible = false;
        //                }
        //            };
        //        }
        //        else if (validator.StartsWith("number-"))
        //        {
        //            var range = validator.Replace("number-", "").Split('-');
        //            if (range.Length == 2 && int.TryParse(range[0], out int min) && int.TryParse(range[1], out int max))
        //            {
        //                entry.TextChanged += (s, e) =>
        //                {
        //                    if (int.TryParse(e.NewTextValue, out int value))
        //                    {
        //                        if (value < min || value > max)
        //                        {
        //                            errorLabel.Text = $"Please enter a number between {min} and {max}.";
        //                            errorLabel.IsVisible = true;
        //                        }
        //                        else
        //                        {
        //                            errorLabel.IsVisible = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        errorLabel.Text = $"Please enter a number between {min} and {max}.";
        //                        errorLabel.IsVisible = true;
        //                    }
        //                };
        //            }
        //        }
        //    }
        //}
        
    }