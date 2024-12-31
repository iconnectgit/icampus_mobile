using System.Text;
using CommunityToolkit.Maui.Views;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Helpers.CustomCalendar;
using iCampus.MobileApp.Views.PopUpViews;
using Mopups.Services;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Helpers;

public class SISApiHelper
    {
        public static async Task<T> GetObject<T>(string methodName)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = await ValidateInternetConnectivity();
            if (isInternetConnected)
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);
                using (var client = new HttpClient(customDelegatingHandler))
                {
                    await ShowProcessingIndicatorPopup();
                    var response = await GetAsyncApiResponse(client, methodName, apiConfig.ApiUrl);
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<T>(x.Result);
                    });
                    await HideProcessingIndicatorPopup();
                }
            }
            return result;
        }

        public static async Task<List<T>> GetObjectList<T>(string methodName)
        {
            List<T> result = new List<T>();
            bool isInternetConnected = await ValidateInternetConnectivity();
            if (isInternetConnected)
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);
                using (var client = new HttpClient(customDelegatingHandler))
                {
                    await ShowProcessingIndicatorPopup();
                    var response = await GetAsyncApiResponse(client, methodName, apiConfig.ApiUrl);
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<List<T>>(x.Result);
                    });
                    await HideProcessingIndicatorPopup();
                }
            }
            return result;
        }


        public static async Task<T> PostRequest<T>(string methodName, object postObject = null)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = await ValidateInternetConnectivity();
            
            if (isInternetConnected)
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);
                using (var client = new HttpClient(customDelegatingHandler))
                {
                    await ShowProcessingIndicatorPopup();

                    string json = string.Empty;
                    var httpContent = new StringContent(json);
                    if (postObject != null)
                    {
                        json = JsonConvert.SerializeObject(postObject, Formatting.Indented);
                        httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    var apiUrl = Path.Combine(apiConfig.ApiUrl, methodName);
                    var response = await client.PostAsync(apiUrl, httpContent).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<T>(x.Result);

                    });
                    await HideProcessingIndicatorPopup();
                }
            }

            return result;
        }

        public static async Task PutRequest(string methodName, object putObject)
        {
            bool isInternetConnected = await ValidateInternetConnectivity();
            if (isInternetConnected)
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);
                using (var client = new HttpClient(customDelegatingHandler))
                {
                    await ShowProcessingIndicatorPopup();
                    string json = JsonConvert.SerializeObject(putObject, Formatting.Indented);
                    var httpContent = new StringContent(json);
                    string apiUrl = Path.Combine(apiConfig.ApiUrl, methodName);
                    var response = await client.PostAsync(apiUrl, httpContent).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();
                    await HideProcessingIndicatorPopup();
                }
            }
        }

        public static async Task<T> DeleteRequest<T>(string methodName)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = await ValidateInternetConnectivity();
            if (isInternetConnected)
            {
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);
                using (var client = new HttpClient(customDelegatingHandler))
                {
                    await ShowProcessingIndicatorPopup();

                    string apiUrl = Path.Combine(apiConfig.ApiUrl, methodName);
                    var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;
                        result = JsonConvert.DeserializeObject<T>(x.Result);
                    });
                    await HideProcessingIndicatorPopup();
                }
            }
            return result;
        }


        private static async Task<HttpResponseMessage> GetAsyncApiResponse(HttpClient client, string methodName, string apiUrl)
        {
            apiUrl = Path.Combine(apiUrl, methodName);
            var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return response;
        }

        public static async Task<bool> ValidateInternetConnectivity()
        {
            bool isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (!isInternetConnected)
            {
                Application.Current.MainPage.ShowPopup(new NetworkAlertPopup());
            }
            return isInternetConnected;
        }

        private static async Task ShowProcessingIndicatorPopup()
        {
            try
            {
                var processingPopup = new ProcessingIndicatorPopup();
                await MopupService.Instance.PushAsync(processingPopup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task HideProcessingIndicatorPopup()
        {
            try
            {
                if (MopupService.Instance.PopupStack.Any())
                {
                    await MopupService.Instance.RemovePageAsync(MopupService.Instance.PopupStack.Last());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }