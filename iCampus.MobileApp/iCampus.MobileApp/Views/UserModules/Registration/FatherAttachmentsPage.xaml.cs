using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Android.Text;
using FFImageLoading.Maui;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Behaviours;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Helpers;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Views.UserModules.Registration;

public partial class FatherAttachmentsPage : ContentPage
	{
        #region Properties
        
        #endregion
        public FatherAttachmentsPage ()
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
            var fatherAttachments = new ObservableCollection<BindableFormFieldsView>(
                AppSettings.Current.FamilyDetails.Where(x => x.CategoryId == 11));

            foreach (var fatherAttachment in fatherAttachments)
            {
                string labelText = fatherAttachment.LabelResourceText;
                string fieldValue = fatherAttachment.FieldValue as string;
                bool isRequired = !string.IsNullOrEmpty(fatherAttachment.Validators) && fatherAttachment.Validators.Contains("required");

                var label = new Label
                {
                    Text = labelText,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)Application.Current.Resources["TitleLabelStyle"],
                };

                var horizontalLayout = new StackLayout
                {
                    IsVisible = fatherAttachment.IsVisible,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5, 0, 5),
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

                switch (fatherAttachment.EditorType)
                {
                    case EditorTypes.MultipleFileUploader:
                        {
                            var frameAttachments = new Frame
                            {
                                HasShadow = false,
                                Padding = new Thickness(5),
                                BackgroundColor = Colors.White,
                                HorizontalOptions = LayoutOptions.Start,
                                BorderColor = Color.FromArgb("#E8E8E8"),
                            };

                            var horizontalStackLayout = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Spacing = 0,
                                IsClippedToBounds = true,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.CenterAndExpand
                            };

                            var cachedImageAttachments = new CachedImage
                            {
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                WidthRequest = 15,
                                HeightRequest = 15,
                                Aspect = Aspect.AspectFit,
                                DownsampleToViewSize = true,
                                Source = "attachment_icon.png"
                            };

                            var labelAttachments = new Label
                            {
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                Margin = new Thickness(5, 0, 0, 0),
                                Text = "Select File",
                                Style = (Style)Application.Current.Resources["DescriptionLabelStyle"] 
                            };

                            var tapGestureRecognizer = new TapGestureRecognizer();
                            tapGestureRecognizer.Tapped += (sender, e) => OnAddAttachmentClickAsync(fatherAttachment);
                            horizontalStackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                            horizontalStackLayout.Children.Add(cachedImageAttachments);
                            horizontalStackLayout.Children.Add(labelAttachments);

                            frameAttachments.Content = horizontalStackLayout;
                            DynamicLayout.Children.Add(frameAttachments);

                            var attachmentListView = new ListView
                            {
                                HasUnevenRows = true,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                SelectionMode = ListViewSelectionMode.None,
                                VerticalOptions = LayoutOptions.Start,
                                BackgroundColor = Colors.Transparent,
                                SeparatorVisibility = SeparatorVisibility.None,
                                Margin = new Thickness(0),
                                BindingContext = fatherAttachment
                            };
                            attachmentListView.SetBinding(ListView.ItemsSourceProperty, new Binding("AttachmentFiles", BindingMode.TwoWay, source: fatherAttachment));
                            attachmentListView.SetBinding(ListView.HeightRequestProperty, new Binding("AttachmentListViewHeight", source: fatherAttachment));
                            attachmentListView.SetBinding(IsVisibleProperty, new Binding("AttachmentFiles.Count", BindingMode.OneWay, new IntToBoolValueConverter(), source: fatherAttachment));

                            var itemTemplate = new DataTemplate(() =>
                            {
                                var mainStackLayout = new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                };

                                var grid = new Grid
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Margin = new Thickness(0, 8, 0, 0),
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                                    ColumnDefinitions =
                                    {
                                        new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) },
                                        new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) }
                                    }
                                };

                                var fileDetailsStackLayout = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    Spacing = 0
                                };

                                var fileIconImage = new CachedImage
                                {
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    WidthRequest = 20,
                                    HeightRequest = 20,
                                    Aspect = Aspect.AspectFit,
                                    Margin = new Thickness(10, 0, 0, 0),
                                    DownsampleToViewSize = true
                                };
                                fileIconImage.SetBinding(CachedImage.SourceProperty, new Binding("FileName", BindingMode.OneWay, new AttachmentFileToImageSourceConverter(), null));

                                // Create the Label for file name
                                var fileNameLabel = new Label
                                {
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    Margin = new Thickness(5, 0, 0, 0),
                                    LineBreakMode = LineBreakMode.TailTruncation,
                                    MaxLines = 1,
                                    TextColor = (Color)Application.Current.Resources["GrayTextColor"],
                                    Style = (Style)Application.Current.Resources["AttachmentTextStyle"]
                                };
                                fileNameLabel.SetBinding(Label.TextProperty, "FileName");

                                var tapGestureRecognizerFile = new TapGestureRecognizer();
                                tapGestureRecognizerFile.Tapped += (sender, e) => PreviewIconClicked(fatherAttachment, fileNameLabel.Text);
                                fileNameLabel.GestureRecognizers.Add(tapGestureRecognizerFile);

                                fileDetailsStackLayout.Children.Add(fileIconImage);
                                fileDetailsStackLayout.Children.Add(fileNameLabel);

                                //grid.Children.Add(fileDetailsStackLayout, 0, 0);
                                
                                grid.Children.Add(fileDetailsStackLayout);
                                Grid.SetRow(fileDetailsStackLayout, 0); 
                                Grid.SetColumn(fileDetailsStackLayout, 0); 

                                var deleteIconImage = new CachedImage
                                {
                                    HorizontalOptions = LayoutOptions.EndAndExpand,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    WidthRequest = 24,
                                    HeightRequest = 24,
                                    Aspect = Aspect.AspectFit,
                                    Source = "delete_bin_icon.png",
                                    DownsampleToViewSize = true
                                };
                                
                                var tapGestureRecognizerListView = new TapGestureRecognizer();
                                tapGestureRecognizerListView.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding("."));
                                tapGestureRecognizerListView.Tapped += OnDeleteAttachmentClick;
                                deleteIconImage.GestureRecognizers.Add(tapGestureRecognizerListView);

                                //grid.Children.Add(deleteIconImage, 1, 0);
                                grid.Children.Add(deleteIconImage);
                                Grid.SetRow(deleteIconImage, 0); 
                                Grid.SetColumn(deleteIconImage, 1); 

                                mainStackLayout.Children.Add(grid);

                                return new ViewCell { View = mainStackLayout };
                            });

                            attachmentListView.ItemTemplate = itemTemplate;
                            
                            DynamicLayout.Children.Add(attachmentListView);
                            break;
                        }
                }
            }
        }

        private async Task OnAddAttachmentClickAsync(BindableFormFieldsView fatherAttachment)
        {
            AttachmentFileView fileData = await HelperMethods.PickFileFromDevice();
            if (fileData == null)
            {
                return;
            }
            string foundSpecialChars = FindSpecialCharacters(fileData.FileName);
            if (!string.IsNullOrEmpty(foundSpecialChars))
            {
                await HelperMethods.ShowAlert(TextResource.AlertsPageTitle, "File name can't contain any of the following character(s): " + foundSpecialChars);
                return;
            }

            fatherAttachment.AttachmentFiles.AddFileToList(fileData);
            fatherAttachment.AttachmentListViewHeight = fatherAttachment.AttachmentFiles.Count * 40;
            var newAttachment = new FileDataView
            {
                CategoryId = fatherAttachment.ExternalCategoryId,
                IsProcessed = false,
                UploadedFileName = fileData.FileName
            };

            AppSettings.Current.AttachmentDetail.Add(newAttachment);
        }

        private async void OnDeleteAttachmentClick(object sender, EventArgs e)
        {
            var tappedEventArgs = e as TappedEventArgs;
            var attachmentFile = tappedEventArgs.Parameter as AttachmentFileView;
            if (attachmentFile != null)
            {
                var fatherAttachments = new ObservableCollection<BindableFormFieldsView>(
                    AppSettings.Current.FamilyDetails.Where(x => x.CategoryId == 11));

                async Task DeleteAttachment()
                {
                    foreach (var item in fatherAttachments)
                    {
                        var itemToRemove = item.AttachmentFiles.FirstOrDefault(x => x.FileName == attachmentFile.FileName);
                        if (itemToRemove != null)
                        {
                            item.AttachmentFiles.Remove(itemToRemove);
                            item.AttachmentListViewHeight = item.AttachmentFiles.Count * 40;
                        }
                    }

                    var existingItemToRemove = AppSettings.Current.ExistingAttachmentDetail.FirstOrDefault(x => x.UploadedFileName == attachmentFile.FileName);
                    if (existingItemToRemove != null)
                    {
                        AppSettings.Current.ExistingAttachmentDetail.Remove(existingItemToRemove);

                        if (string.IsNullOrEmpty(AppSettings.Current.DeletedAttachmentFiles))
                        {
                            AppSettings.Current.DeletedAttachmentFiles = attachmentFile.FileName;
                        }
                        else
                        {
                            AppSettings.Current.DeletedAttachmentFiles += ":" + attachmentFile.FileName;
                        }
                    }

                    var newItemToRemove = AppSettings.Current.AttachmentDetail.FirstOrDefault(x => x.UploadedFileName == attachmentFile.FileName);
                    if (newItemToRemove != null)
                    {
                        AppSettings.Current.AttachmentDetail.Remove(newItemToRemove);
                    }
                }

                var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    await DeleteAttachment();
                }
            }
        }
       
        private string FindSpecialCharacters(string fileName)
        {
            string specialCharacters = "!@#$%^&*()+[]{}|;:'\"<>,?/~`";
            var foundSpecialCharacters = fileName.Where(c => specialCharacters.Contains(c)).Distinct();
            string specialCharactersString = string.Join(", ", foundSpecialCharacters);

            return specialCharactersString;
        }

        private async Task PreviewIconClicked(BindableFormFieldsView fatherAttachments, string fileName)
        {
            try
            {
                string downloadUrl = fatherAttachments.DownloadUrl;
                string portalUrl = AppSettings.Current.PortalUrl;
                string filePath = portalUrl + downloadUrl + "?filePath=" + fatherAttachments.DownloadFilePath + "/" + fileName;

                //await HelperMethods.OpenFileForPreview(filePath);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (await IsValid())
                {
                    List<AttachmentFileView> attachmentFiles = new List<AttachmentFileView>();
                    var existingFileNames = AppSettings.Current.ExistingAttachmentDetail.Select(x => x.UploadedFileName).ToList();
                    foreach (var item in AppSettings.Current.FamilyDetails.Where(f => f.CategoryId == 11 || f.CategoryId == 12))
                    {
                        if (item.AttachmentFiles != null && item.AttachmentFiles.Count > 0)
                        {
                            var newFiles = item.AttachmentFiles.Where(a => !existingFileNames.Contains(a.FileName)).ToList();
                            attachmentFiles.AddRange(newFiles);
                        }
                    }
                    var existingAttachmentDetail = AppSettings.Current.ExistingAttachmentDetail.Select(x => new
                    {
                        x.AttachmentId,
                        x.CategoryId,
                        x.IsProcessed,
                        x.UploadedFileName
                    }).ToList();
                    var existingAttachmentDetailStr = JsonConvert.SerializeObject(existingAttachmentDetail);

                    var attachmentsDetail = AppSettings.Current.AttachmentDetail.Select(x => new
                    {
                        x.CategoryId,
                        x.IsProcessed,
                        x.UploadedFileName
                    }).ToList();
                    var attachmentDetailStr = JsonConvert.SerializeObject(attachmentsDetail);
                    var data = AppSettings.Current.DeletedAttachmentFiles;

                    

                    int totalFiles = attachmentFiles.Count;
                    int filesUploaded = 0;

                    foreach (var file in attachmentFiles)
                    {
                        progressBar.IsVisible = true;
                        progressBar.Progress = 0;
                        progressMessage.IsVisible = true;
                        filesUploaded++;
                        progressBar.Progress = (double)filesUploaded / totalFiles;
                        progressMessage.Text = $"Uploading file {filesUploaded} of {totalFiles} files";
                        var fileList = new List<AttachmentFileView> { file };
                        OperationDetails operation = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(
                            string.Format(TextResource.UploadRegistrationFamilyAttachmentFileApi, AppSettings.Current.FamilyId),
                            AppSettings.Current.ApiUrl, null, fileList, isLoader: false);

                        if (!operation.Success)
                        {
                            progressBar.IsVisible = false;
                            progressMessage.IsVisible = false;
                            await HelperMethods.ShowAlert("", operation.Message);
                            return;
                        }
                    }
                    progressBar.IsVisible = false;
                    progressMessage.IsVisible = false;
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveRegistrationFamilyAttachmentDataApi, AppSettings.Current.FamilyId, HttpUtility.UrlEncode(attachmentDetailStr), HttpUtility.UrlEncode(existingAttachmentDetailStr), AppSettings.Current.DeletedAttachmentFiles), AppSettings.Current.ApiUrl);
                    await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }
        }

        public async Task<bool> IsValid()
        {
            foreach (var field in AppSettings.Current.FamilyDetails.Where(f => f.CategoryId == 11))
            {
                bool isRequired = !string.IsNullOrEmpty(field.Validators) && field.Validators.Contains("required");
                if (isRequired)
                {
                    if (field.AttachmentFiles.Count <= 0)
                    {
                        await HelperMethods.ShowAlert("Validation Error", $"You missed {field.LabelResourceText} required field.");
                        return false;
                    }
                }
            }
            return true;
        }
    }