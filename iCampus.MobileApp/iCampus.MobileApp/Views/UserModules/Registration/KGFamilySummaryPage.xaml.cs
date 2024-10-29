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
using CheckBox = InputKit.Shared.Controls.CheckBox;


namespace iCampus.MobileApp.Views.UserModules.Registration;

public partial class KGFamilySummaryPage : ContentPage, INotifyPropertyChanged
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
        get => _yesRadioButtonImage;
        set
        {
            _yesRadioButtonImage = value;
            OnPropertyChanged("YesRadioButtonImage");
        }
    }

    private string _noRadioButtonImage;

    public string NoRadioButtonImage
    {
        get => _noRadioButtonImage;
        set
        {
            _noRadioButtonImage = value;
            OnPropertyChanged("NoRadioButtonImage");
        }
    }

    private string _genderId;

    public string GenderId
    {
        get => _genderId;
        set
        {
            _genderId = value;
            OnPropertyChanged("GenderId");
        }
    }

    private string _birthDate;

    public string BirthDate
    {
        get => _birthDate;
        set
        {
            _birthDate = value;
            OnPropertyChanged("BirthDate");
        }
    }

    #endregion

    private readonly RegistrationServices registrationService = new();

    public KGFamilySummaryPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PopulateDetails();
    }

    private void PopulateDetails()
    {
        DynamicLayout.Children.Clear();
        var studentDetails = new ObservableCollection<BindableFormFieldsView>(
            AppSettings.Current.KGStudentDetails.Where(x => x.CategoryId == 21));

        foreach (var studentDetail in studentDetails)
        {
            if (studentDetail.EditorTypeCode.Trim() == "H") continue;
            var labelText = studentDetail.LabelResourceText;
            var fieldValue = studentDetail.FieldValue as string;
            var isRequired = !string.IsNullOrEmpty(studentDetail.Validators) &&
                             studentDetail.Validators.Contains("required");

            var label = new Label
            {
                Text = labelText,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 0),
                Style = (Style)Application.Current.Resources["TitleLabelStyle"]
            };

            var horizontalLayout = new StackLayout
            {
                IsVisible = studentDetail.IsVisible,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 5, 0, 5)
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

            switch (studentDetail.EditorType)
            {
                case EditorTypes.Textbox:
                    var entry = new NoUnderlineEntry
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        //Text = fieldValue,
                        AutomationId = studentDetail.DataFieldName,
                        Style = (Style)Application.Current.Resources["EntryFontStyle"],
                        Margin = new Thickness(0),
                        HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40,
                        BindingContext = studentDetail
                    };
                    entry.SetBinding(Entry.TextProperty,
                        new Binding("FieldValue", BindingMode.TwoWay, source: studentDetail));

                    var errorLabel = new Label
                    {
                        TextColor = Colors.Red,
                        IsVisible = false,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                    };
                    if (!string.IsNullOrEmpty(studentDetail.Validators))
                        registrationService.ApplyValidators(entry, studentDetail.Validators, errorLabel);

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
                        IsVisible = studentDetail.IsVisible,
                        Children = { frame, errorLabel }
                    };

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(stackLayoutWithError);
                    break;

                case EditorTypes.LanguageEditorTextBox:
                case EditorTypes.TelephoneNumber:
                case EditorTypes.Textarea:
                    var languageEntry = new NoUnderlineEntry
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        //Text = fieldValue,
                        AutomationId = studentDetail.DataFieldName,
                        Style = (Style)Application.Current.Resources["EntryFontStyle"],
                        Margin = new Thickness(0),
                        HeightRequest = Device.RuntimePlatform == Device.iOS ? 30 : 40,
                        BindingContext = studentDetail
                    };
                    languageEntry.SetBinding(Entry.TextProperty,
                        new Binding("FieldValue", BindingMode.TwoWay, source: studentDetail));

                    var languageErrorLabel = new Label
                    {
                        TextColor = Colors.Red,
                        IsVisible = false,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                    };
                    if (!string.IsNullOrEmpty(studentDetail.Validators))
                        registrationService.ApplyValidators(languageEntry, studentDetail.Validators,
                            languageErrorLabel);
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
                        IsVisible = studentDetail.IsVisible,
                        Children = { languageFrame, languageErrorLabel }
                    };

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(languageStackLayoutWithError);
                    break;

                case EditorTypes.Datepicker:
                    var customDatePicker = new DatePicker
                    {
                        BackgroundColor = Colors.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(DatePicker)),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        AutomationId = studentDetail.DataFieldName,
                        Style = (Style)Application.Current.Resources["DatePickerFontStyle"]
                    };

                    if (DateTime.TryParse(fieldValue, out var parsedDate)) customDatePicker.Date = parsedDate;

                    customDatePicker.DateSelected += (sender, args) =>
                    {
                        studentDetail.FieldValue = args.NewDate.ToString("dd-MMM-yyyy");
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
                        IsVisible = studentDetail.IsVisible,
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(0),
                        Margin = new Thickness(0, 5, 0, 0),
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    stackLayout.Children.Add(frameDatePicker);
                    stackLayout.Children.Add(calendarIcon);

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(stackLayout);
                    break;

                case EditorTypes.YesOrNo:

                    var stackLayoutYesOrNo = new StackLayout
                    {
                        IsVisible = studentDetail.IsVisible,
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 0),
                        HeightRequest = 40
                    };

                    if (studentDetail.FieldValue?.ToString().ToLower() == "true")
                    {
                        YesRadioButtonImage = "selected_radio_button.png";
                        NoRadioButtonImage = "unselected_radio_button.png";
                    }
                    else if (studentDetail.FieldValue?.ToString().ToLower() == "false")
                    {
                        YesRadioButtonImage = "unselected_radio_button.png";
                        NoRadioButtonImage = "selected_radio_button.png";
                    }
                    else
                    {
                        YesRadioButtonImage = "unselected_radio_button.png";
                        NoRadioButtonImage = "unselected_radio_button.png";
                    }

                    var yesRadioButtonImage = new CachedImage
                    {
                        Source = YesRadioButtonImage,
                        Scale = 0.4,
                        BindingContext = studentDetail
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
                        BindingContext = studentDetail
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

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(stackLayoutYesOrNo);
                    break;

                case EditorTypes.CheckBox:
                {
                    var checkboxStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        IsVisible = studentDetail.IsVisible
                    };

                    var checkBox = new CheckBox
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Scale = 0.8,
                        Margin = new Thickness(5, 0, 0, 0),
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        IsEnabled = true,
                        BindingContext = studentDetail
                    };
                    checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("FieldValue", source: studentDetail));
                    checkBox.CheckChanged += OnCheckBoxCheckedChanged;
                    checkboxStackLayout.Children.Add(checkBox);

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(checkboxStackLayout);
                    break;
                }

                case EditorTypes.AutoComplete:
                {
                    var frameSelectbox = new Frame
                    {
                        IsVisible = studentDetail.IsVisible,
                        HasShadow = false,
                        Margin = new Thickness(0, 5, 0, 0),
                        Padding = new Thickness(0),
                        IsClippedToBounds = true,
                        BorderColor = Colors.Transparent,
                        HeightRequest = 40,
                        CornerRadius = 5,
                        BindingContext = studentDetail
                    };

                    var grid = new Grid();

                    var noUnderlineEntry = new NoUnderlineEntry
                    {
                        Text = Convert.ToString(studentDetail.FieldText),
                        Placeholder = "Select",
                        Style = (Style)Application.Current.Resources["EntryFontStyle"],
                        TextColor = Colors.Gray,
                        Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(5, 0, 0, 0) : new Thickness(0),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 40
                        //IsReadOnly = true
                    };
                    noUnderlineEntry.TextChanged += (s, e) => SearchEntryTextChanged(s, e, studentDetail);

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
                        BindingContext = studentDetail
                    };

                    listView.SetBinding(IsVisibleProperty, new Binding("ListViewVisible", source: studentDetail));
                    listView.SetBinding(ListView.ItemsSourceProperty,
                        new Binding("GeneralSelectList", source: studentDetail));
                    listView.SetBinding(ListView.SelectedItemProperty,
                        new Binding("SelectedItem", source: studentDetail));
                    listView.SetBinding(HeightRequestProperty,
                        new Binding("ListViewHeightRequest", source: studentDetail));

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

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
                    }

                    DynamicLayout.Children.Add(listView);
                    break;
                }

                case EditorTypes.Selectbox:
                {
                    var frameSelectbox = new Frame
                    {
                        IsVisible = studentDetail.IsVisible,
                        HasShadow = false,
                        Margin = new Thickness(0, 5, 0, 0),
                        Padding = new Thickness(0),
                        IsClippedToBounds = true,
                        BorderColor = Colors.Transparent,
                        HeightRequest = 40,
                        CornerRadius = 5,
                        BindingContext = studentDetail
                    };

                    var grid = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    var noUnderlineEntry = new NoUnderlineEntry
                    {
                        Text = Convert.ToString(studentDetail.FieldText),
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

                    var boxView = new BoxView
                    {
                        Color = Colors.Transparent,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BindingContext = studentDetail
                    };

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += FrameTapped;
                    frameSelectbox.GestureRecognizers.Add(tapGestureRecognizer);

                    var absoluteLayout = new AbsoluteLayout();


                    frameSelectbox.Content = grid;

                    DynamicLayout.Children.Add(frameSelectbox);
                    var listView = new SwipeGestureListview
                    {
                        HasUnevenRows = true,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        SeparatorVisibility = SeparatorVisibility.Default,
                        Margin = new Thickness(0, 5, 0, 0),
                        BindingContext = studentDetail
                    };

                    listView.SetBinding(IsVisibleProperty, new Binding("ListViewVisible", source: studentDetail));
                    listView.SetBinding(ListView.ItemsSourceProperty,
                        new Binding("GeneralSelectList", source: studentDetail));
                    listView.SetBinding(ListView.SelectedItemProperty,
                        new Binding("SelectedItem", source: studentDetail));
                    listView.SetBinding(HeightRequestProperty,
                        new Binding("ListViewHeightRequest", source: studentDetail));

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

                    if (studentDetail.HasLinkedChildren > 0)
                    {
                        var parentFields = studentDetails.Where(f =>
                            !string.IsNullOrEmpty(f.VisibilityLinkedField) &&
                            f.VisibilityLinkedField.StartsWith(studentDetail.FieldName + "-"));
                        foreach (var field in parentFields)
                            HelperMethods.ShowHideVisibilityLinkedFields(studentDetail,
                                AppSettings.Current.KGStudentDetails);
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
        var detail = image?.BindingContext as BindableFormFieldsView;
        OnPropertyChanged(nameof(YesRadioButtonImage));
        OnPropertyChanged(nameof(NoRadioButtonImage));
        detail.FieldValue = true;
        HelperMethods.ShowHideVisibilityLinkedFields(detail, AppSettings.Current.KGStudentDetails);
        PopulateDetails();
    }

    private void NoRadioButtonTapped(object sender, EventArgs e)
    {
        var image = sender as CachedImage;
        var detail = image?.BindingContext as BindableFormFieldsView;
        OnPropertyChanged(nameof(YesRadioButtonImage));
        OnPropertyChanged(nameof(NoRadioButtonImage));
        detail.FieldValue = false;
        HelperMethods.ShowHideVisibilityLinkedFields(detail, AppSettings.Current.KGStudentDetails);
        PopulateDetails();
    }

    private void SearchEntryTextChanged(object sender, TextChangedEventArgs e, BindableFormFieldsView detail)
    {
        if (e.NewTextValue.Length >= 3)
        {
            var filteredList = detail.MasterList
                .Where(item => item.ItemName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
            detail.GeneralSelectList = new ObservableCollection<SelectionListView>(filteredList);
        }
        else
        {
            detail.GeneralSelectList = new ObservableCollection<SelectionListView>(detail.MasterList);
        }

        detail.ListViewVisible = true;
        detail.ListViewHeightRequest = detail.GeneralSelectList.Count * 30;
        if (detail.ListViewHeightRequest > 300)
            detail.ListViewHeightRequest = 300;
    }

    private async void FrameTapped(object sender, EventArgs e)
    {
        try
        {
            BindableFormFieldsView detail = null;
            if (sender is BoxView boxView)
                detail = boxView.BindingContext as BindableFormFieldsView;
            else if (sender is Frame frame) detail = frame.BindingContext as BindableFormFieldsView;

            if (detail == null) return;
            var listUrl = detail.ListUrl;
            if (listUrl.StartsWith("/")) listUrl = listUrl.TrimStart('/');
            var portalUrl = AppSettings.Current.PortalUrl;
            var cacheKeyPrefix = detail.DataFieldName;
            detail.MasterList = detail.GeneralSelectList = new ObservableCollection<SelectionListView>(
                await ApiHelper.GetObjectList<SelectionListView>(listUrl,
                    apiUrl: portalUrl, cacheType: ApiHelper.CacheTypeParam.LoadFromCache,
                    cacheKeyPrefix: cacheKeyPrefix));
            detail.ListViewVisible = !detail.ListViewVisible;
            detail.ListViewHeightRequest = detail.GeneralSelectList.Count * 30;
            if (detail.ListViewHeightRequest > 300)
                detail.ListViewHeightRequest = 300;
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
        var detail = label?.Parent?.Parent?.Parent?.BindingContext as BindableFormFieldsView;

        if (detail != null && tappedItem != null)
        {
            var itemIdProperty = tappedItem.GetType().GetProperty("ItemId");
            var itemNameProperty = tappedItem.GetType().GetProperty("ItemName");
            if (itemIdProperty != null && itemNameProperty != null)
            {
                var itemId = (int)itemIdProperty.GetValue(tappedItem);
                var itemName = itemNameProperty.GetValue(tappedItem) as string;

                detail.FieldValue = itemId.ToString();
                detail.FieldText = itemName;
            }

            detail.ListViewVisible = false;
            HelperMethods.ShowHideVisibilityLinkedFields(detail, AppSettings.Current.KGStudentDetails);
            if (detail.FieldName.ToLower() == "gender")
            {
                GenderId = Convert.ToString(detail.FieldValue);
                AppSettings.Current.GradeUrl = "Users/Registration/GetRegistrationGradesByAge?birthDate=" + BirthDate +
                                               "&genderId=" + GenderId + "&academicYear=" +
                                               AppSettings.Current.AcademicYear;
            }

            PopulateDetails();
        }
    }

    private void OnCheckBoxCheckedChanged(object? sender, EventArgs eventArgs)
    {
        var checkBox = sender as CheckBox;
        var detail = checkBox?.BindingContext as BindableFormFieldsView;

        if (detail != null) detail.FieldValue = checkBox.IsChecked;
        PopulateDetails();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        registrationService.SaveKGInformationDetails();
    }
}