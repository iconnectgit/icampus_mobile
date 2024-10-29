using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Web;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.Helpers;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class RegistrationServices : ViewModelBase
	{
		public RegistrationServices() : base(null, null, null)
		{
		}


        public async void SaveStudentDetails(ProgressBar progressBar, Label progressMessage)
        {
            try
            {
                if (await IsValid(AppSettings.Current.StudentDetails))
                {
                    List<AttachmentFileView> attachmentFiles = new List<AttachmentFileView>();
                    string imageDataString = "";
                    var existingFileNames = AppSettings.Current.ExistingAttachmentDetail.Select(x => x.UploadedFileName).ToList();
                    foreach (var item in AppSettings.Current.StudentDetails.Where(f => f.CategoryId == 7))
                    {
                        if (item.AttachmentFiles != null && item.AttachmentFiles.Count > 0)
                        {
                            var newFiles = item.AttachmentFiles.Where(a => !existingFileNames.Contains(a.FileName)).ToList();
                            attachmentFiles.AddRange(newFiles);
                            if (item.EditorTypeCode.ToLower().Trim() == "i")
                            {
                                imageDataString = JsonConvert.SerializeObject(item.ImageData);
                            }
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

                    string jsonString = "[{\"Id\":\"\",\"FamilyId\":\"\",\"RegistrationBookingId\":\"0\",";
                    bool isFirst = true;

                    foreach (var field in AppSettings.Current.StudentDetails.Where(f => f.CategoryId != 7))
                    {
                        if (!string.IsNullOrEmpty(field.ExternalFieldGroupCode))
                        {
                            continue;
                        }
                        if (!isFirst)
                        {
                            jsonString += ",";
                        }

                        jsonString += $"\"{field.DataFieldName}\":\"{field.FieldValue}\"";
                        isFirst = false;
                    }
                    jsonString += "}]";

                    var externalFieldValues = new List<ExternalFieldValue>();
                    foreach (var field in AppSettings.Current.StudentDetails.Where(f => f.CategoryId != 7))
                    {
                        if (!string.IsNullOrEmpty(field.ExternalFieldGroupCode))
                        {
                            externalFieldValues.Add(new ExternalFieldValue()
                            {
                                FieldGroupCode = field.ExternalFieldGroupCode,
                                FieldGroupId = (int)field.ExternalFieldGroupId,
                                FieldValue = Convert.ToString(field.FieldValue)
                            });
                        }
                    }

                    string extFieldValuesJsonString = JsonConvert.SerializeObject(externalFieldValues);

                    OperationDetails operationDetails = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveStudentRegistrationFormDetailsApi, AppSettings.Current.RegistrationId, HttpUtility.UrlEncode(jsonString), HttpUtility.UrlEncode(extFieldValuesJsonString)), AppSettings.Current.ApiUrl);
                    if (attachmentFiles.Count <= 0 && String.IsNullOrEmpty(AppSettings.Current.DeletedAttachmentFiles))
                    {
                        await HelperMethods.ShowAlertWithAction("", operationDetails.Message, async () =>
                        {
                            MessagingCenter.Send<string>("", "UpdateReregistration");
                            await Application.Current.MainPage.Navigation.PopAsync();
                            await Application.Current.MainPage.Navigation.PopAsync();
                        });
                    }

                    var registrationId = operationDetails.Output;

                    if (operationDetails.Success)
                    {
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
                                string.Format(TextResource.UploadRegistrationStudentAttachmentFileApi, registrationId, HttpUtility.UrlEncode(imageDataString)),
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
                        if (attachmentFiles.Count > 0 || !String.IsNullOrEmpty(AppSettings.Current.DeletedAttachmentFiles))
                        {
                            OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveRegistrationStudentAttachmentDataApi, registrationId, HttpUtility.UrlEncode(attachmentDetailStr), HttpUtility.UrlEncode(existingAttachmentDetailStr), AppSettings.Current.DeletedAttachmentFiles), AppSettings.Current.ApiUrl);
                            await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                            {
                                MessagingCenter.Send<string>("", "UpdateReregistration");
                                await Application.Current.MainPage.Navigation.PopAsync();
                                await Application.Current.MainPage.Navigation.PopAsync();
                            });
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }
        }

        public async void SaveFamilyDetails(ObservableCollection<BindableFormFieldsView> studentDetails)
        {
            try
            {
                if (await IsValid(studentDetails))
                {
                    long primaryKeyValue = 0;
                    var fieldsToInclude = new List<string> { "IsNewParent", "IsPending", "Latitude", "Longitude" };

                    if (AppSettings.Current.FormData.Any())
                    {
                        primaryKeyValue = AppSettings.Current.FormData[0].FirstOrDefault(x => x.Key == "RegistrationFamilyDataId").Value.ToLongInteger();
                    }
                    string jsonString = "[{\"Id\":\"\",\"FamilyId\":\"\",";
                    bool isFirst = true;
                    foreach (var fieldDict in AppSettings.Current.FormData)
                    {
                        foreach (var field in fieldsToInclude)
                        {
                            if (fieldDict.ContainsKey(field))
                            {
                                if (!isFirst)
                                {
                                    jsonString += ",";
                                }

                                string serializedValue = System.Text.Json.JsonSerializer.Serialize(fieldDict[field]);
                                jsonString += $"\"{field}\":{serializedValue}";
                                isFirst = false;
                            }
                        }
                    }
                    foreach (var field in studentDetails)
                    {
                        if (field.DataFieldName == "MapLocation")
                        {
                            continue;
                        }
                        if (!isFirst)
                        {
                            jsonString += ",";
                        }

                        jsonString += $"\"{field.DataFieldName}\":\"{field.FieldValue}\"";
                        isFirst = false;
                    }
                    jsonString += "}]";

                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveRegistrationFormFamilyDetailsApi, HttpUtility.UrlEncode(jsonString), primaryKeyValue, AppSettings.Current.FamilyId), AppSettings.Current.ApiUrl);
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

        public async void SaveKGInformationDetails()
        {
            try
            {
                if (await IsValid(AppSettings.Current.KGStudentDetails))
                {
                    string jsonString = "[{\"Id\":\"\",\"FamilyId\":\"\",";
                    bool isFirst = true;

                    foreach (var field in AppSettings.Current.KGStudentDetails)
                    {
                        if (field.EditorTypeCode.ToLower().Trim() == "rt")
                        {
                            continue;
                        }
                        if (!isFirst)
                        {
                            jsonString += ",";
                        }

                        jsonString += $"\"{field.DataFieldName}\":\"{field.FieldValue}\"";
                        isFirst = false;
                    }
                    jsonString += "}]";

                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveKgStudentRegistrationFormDetailsApi, AppSettings.Current.RegistrationId, HttpUtility.UrlEncode(jsonString)), AppSettings.Current.ApiUrl);
                    await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                    {
                        MessagingCenter.Send<string>("", "UpdateReregistration");
                        await Application.Current.MainPage.Navigation.PopAsync();
                        await Application.Current.MainPage.Navigation.PopAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, "");
            }
        }

        public async Task<bool> IsValid(ObservableCollection<BindableFormFieldsView> studentDetails)
        {
            foreach (var field in studentDetails)
            {
                if (field.EditorTypeCode.ToLower().Trim() == "h")
                {
                    continue;
                }
                bool isRequired = !string.IsNullOrEmpty(field.Validators) && field.Validators.Contains("required");
                if (isRequired)
                {
                    if (field.EditorTypeCode.ToLower().Trim() == "i" || field.EditorTypeCode.ToLower().Trim() == "mf" && field.AttachmentFiles.Count <= 0)
                    {
                        await HelperMethods.ShowAlert("Validation Error", $"You missed \"{field.LabelResourceText}\" required field in {field.CategoryName} details.");
                        return false;
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(field.FieldValue)))
                    {
                        await HelperMethods.ShowAlert("Validation Error", $"You missed \"{field.LabelResourceText}\" required field in {field.CategoryName} details.");
                        return false;
                    }
                }
            }
            return true;
        }

        public void ApplyValidators(NoUnderlineEntry entry, string validators, Label errorLabel)
        {
            string[] validatorList = !string.IsNullOrEmpty(validators) ? validators.Replace(" ", "").Split(',') : new string[] { };

            foreach (var validator in validatorList)
            {
                if (validator.Equals("required"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (string.IsNullOrEmpty(e.NewTextValue))
                        {
                            errorLabel.Text = "This field is required.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.Equals("email"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (!Regex.IsMatch(e.NewTextValue, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                            errorLabel.Text = "Please enter a valid email address.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.Equals("phone-uae"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (!Regex.IsMatch(e.NewTextValue, @"^((05)[0-9]{8})$"))
                        {
                            errorLabel.Text = "Please enter a valid phone number.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.Equals("phone-oman"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (!Regex.IsMatch(e.NewTextValue, @"^([0-9]{8})$"))
                        {
                            errorLabel.Text = "Please enter a valid phone number.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.Equals("phone"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (!Regex.IsMatch(e.NewTextValue, @"^([0-9]{10})$"))
                        {
                            errorLabel.Text = "Please enter a valid phone number.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.Equals("number"))
                {
                    entry.TextChanged += (s, e) =>
                    {
                        if (!Regex.IsMatch(e.NewTextValue, @"^[0-9]*$"))
                        {
                            errorLabel.Text = "Please enter a valid number.";
                            errorLabel.IsVisible = true;
                        }
                        else
                        {
                            errorLabel.IsVisible = false;
                        }
                    };
                }
                else if (validator.StartsWith("number-"))
                {
                    var range = validator.Replace("number-", "").Split('-');
                    if (range.Length == 2 && int.TryParse(range[0], out int min) && int.TryParse(range[1], out int max))
                    {
                        entry.TextChanged += (s, e) =>
                        {
                            if (int.TryParse(e.NewTextValue, out int value))
                            {
                                if (value < min || value > max)
                                {
                                    errorLabel.Text = $"Please enter a number between {min} and {max}.";
                                    errorLabel.IsVisible = true;
                                }
                                else
                                {
                                    errorLabel.IsVisible = false;
                                }
                            }
                            else
                            {
                                errorLabel.Text = $"Please enter a number between {min} and {max}.";
                                errorLabel.IsVisible = true;
                            }
                        };
                    }
                }
            }
        }

    }