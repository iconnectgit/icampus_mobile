using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using iCampus.MobileApp.Forms;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;
using System.Resources;
using iCampus.Common.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.PopUpViews;
using Microsoft.Maui.Networking;
using JsonSerializer = System.Text.Json.JsonSerializer;
using NetworkAccess = Microsoft.Maui.Networking.NetworkAccess;
using Mopups.Services;
namespace iCampus.MobileApp.Helpers;

public class ApiHelper
    {
        static ResourceManager resourceManager = new ResourceManager("iCampus.MobileApp.TextResource", Assembly.GetExecutingAssembly());
        string welcomeMessage = resourceManager.GetString("PublicKey");
        
        private static string publicKey = TextResource.PublicKey;
        private static string secretKey = TextResource.SecretKey;
        private static ProcessingIndicatorPopup _processingPopup;
        private static bool _isPopupVisible = false;
        private static int _popupRequestCount = 0;
        private static readonly object _lock = new object();


        #region Caching

        public static async Task<(bool, T)> LoadObjectFromCache<T>(string methodName, bool isInternetConnected = false)
        {
            var result = await ICCacheManager.GetObjectData<T>(methodName, isInternetConnected);
            return result;
        }
        public static async Task<(bool, List<T>)> LoadObjectListFromCache<T>(string methodName, bool isInternetConnected = false)
        {
            var result = await ICCacheManager.GetObjectList<T>(methodName, isInternetConnected);
            return result;
        }
        public static string GetCacheKey(string methodName, bool attachStudentIdIfParent = true)
        {
            string methodNameWithoutParameter;
            if (methodName.Contains("?"))
            {
                var position = methodName.IndexOf("?");
                methodNameWithoutParameter = methodName.Substring(0, position);
            }
            else
            {
                methodNameWithoutParameter = methodName;
            }
            if (AppSettings.Current.SelectedStudent.ItemId != null && AppSettings.Current.IsParent && attachStudentIdIfParent)
                return string.Concat(methodNameWithoutParameter, "_", AppSettings.Current.UserId, "_", AppSettings.Current.SchoolCampusId, "_", AppSettings.Current.SelectedStudent.ItemId);
            else
                return string.Concat(methodNameWithoutParameter, "_", AppSettings.Current.UserId, "_", AppSettings.Current.SchoolCampusId);
        }

        #endregion

        public enum CacheTypeParam
        {
            CachingDisabled = 0,
            LoadFromCache = 1,
            LoadFromServerAndCache = 2
        }

        public static async Task<T> GetObject<T>(string methodName, bool loadFromCacheWhenNoInternetConnection = false, bool isLoader = true, bool attachStudentIdIfParent = true, CacheTypeParam cacheType = CacheTypeParam.CachingDisabled, string cacheKeyPrefix = null)
        {
            return await GetObject<T>(methodName, AppSettings.Current.ApiUrl, loadFromCacheWhenNoInternetConnection, isLoader, attachStudentIdIfParent, cacheType, cacheKeyPrefix);
        }
        public static async Task<T> GetObject<T>(string methodName, string apiUrl, bool loadFromCacheWhenNoInternetConnection, bool isLoader, bool attachStudentIdIfParent = true, CacheTypeParam cacheType = CacheTypeParam.CachingDisabled, string cacheKeyPrefix = null)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = ValidateInternetConnectivity();
            string finalCacheKey = GetCacheKey(string.IsNullOrEmpty(cacheKeyPrefix) ? methodName : cacheKeyPrefix, attachStudentIdIfParent);

            
            if (!isInternetConnected && loadFromCacheWhenNoInternetConnection)
            {
                cacheType = CacheTypeParam.LoadFromCache;
            }
            if (cacheType == CacheTypeParam.LoadFromCache)
            {
                var cacheResult = await LoadObjectFromCache<T>(finalCacheKey, isInternetConnected);
                if (cacheResult.Item1)
                {
                    return cacheResult.Item2;
                }
            }

            if (!isInternetConnected)
            {
                await DisplayNoConnectionMessage();
            }
            else
            {
                using (var client = new HttpClient())
                {
                    if (isLoader)
                    {
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = true);
                        await ShowProcessingIndicatorPopup();
                    }
                    var response = await GetAsyncApiResponse(client, methodName, apiUrl);
                    if (response.StatusCode == HttpStatusCode.Unauthorized && !AppSettings.Current.IsSessionExpiredAlert)
                    {
                        HelperMethods.LogEvent("API_Calling", $"Date - {DateTime.Now.ToString()} - MethodName - {cacheKeyPrefix} - ResponseStatus - {response.StatusCode.ToString()}");
                        
                        await HelperMethods.PerformAutoLogout(apiUrl);
                        return result;
                    }
                    
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<T>(x.Result);
                    });
                    if (isLoader)
                    {
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
                        await HideProcessingIndicatorPopup();
                    }
                }
                if (cacheType == CacheTypeParam.LoadFromCache || cacheType == CacheTypeParam.LoadFromServerAndCache)
                {
                    ICCacheManager.SaveObjectWithCacheLimit(finalCacheKey, result);
                }
            }
            return result;
        }
        public static async Task<List<T>> GetObjectList<T>(string methodName, bool loadFromCacheWhenNoInternetConnection = false, bool isLoader = true, bool attachStudentIdIfParent = true, CacheTypeParam cacheType = CacheTypeParam.CachingDisabled, string cacheKeyPrefix = null, string apiUrl = null)
        {
            List<T> result = new List<T>();
            if(string.IsNullOrEmpty(apiUrl))
                apiUrl = AppSettings.Current.ApiUrl;
            bool isInternetConnected = ValidateInternetConnectivity();
            string finalCacheKey = GetCacheKey(string.IsNullOrEmpty(cacheKeyPrefix) ? methodName : cacheKeyPrefix, attachStudentIdIfParent);

            if (!isInternetConnected && loadFromCacheWhenNoInternetConnection)
            {
                cacheType = CacheTypeParam.LoadFromCache;
            }
            if (cacheType == CacheTypeParam.LoadFromCache)
            {
                var cacheResult = await LoadObjectListFromCache<T>(finalCacheKey, isInternetConnected);
                if (cacheResult.Item1 == true)
                {
                    return cacheResult.Item2;
                }
            }

            if (!isInternetConnected)
            {
                await DisplayNoConnectionMessage();
            }
            else
            {
                using (var client = new HttpClient())
                {
                    if(isLoader)
                    {
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = true);
                        await ShowProcessingIndicatorPopup();
                    }
                    var response = await GetAsyncApiResponse(client, methodName, apiUrl);
                    if (response.StatusCode == HttpStatusCode.Unauthorized && !AppSettings.Current.IsSessionExpiredAlert)
                    {
                        // Analytics.TrackEvent(name: response.StatusCode.ToString(),
                        //     new Dictionary<string, string> {
                        //         { "DateTime", DateTime.Now.ToString() },
                        //         { "methodName", methodName}
                        //     });
                        await HelperMethods.PerformAutoLogout(apiUrl);
                        return result;
                    }
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<List<T>>(x.Result);
                    });
                    if(isLoader)
                    {
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
                        await HideProcessingIndicatorPopup();
                    }
                }
                if (cacheType == CacheTypeParam.LoadFromCache || cacheType == CacheTypeParam.LoadFromServerAndCache)
                {
                    ICCacheManager.SaveObjectList(finalCacheKey, result);
                }
            }
            return result;
        }

        public static async Task<T> PostRequest<T>(string methodName)
        {
            return await PostRequest<T>(methodName, AppSettings.Current.ApiUrl);
        }

        public static async Task<T> PostRequest<T>(string methodName, string apiUrl, object postObject = null, bool isJsonMediaType = true)
        {
            try
            {
                T result = Activator.CreateInstance<T>();
                bool isInternetConnected = await ValidateInternetConnectivityAndDisplayMessage();
                if (isInternetConnected)
                {
                    using (var client = new HttpClient())
                    {
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = true);
                        await ShowProcessingIndicatorPopup();
                        AuthorizeRequest(client, "POST", postObject);

                        string json = string.Empty;
                        if (postObject != null)
                            json = JsonConvert.SerializeObject(postObject, (Newtonsoft.Json.Formatting)Formatting.Indented);
                        StringContent httpContent;
                        if (isJsonMediaType)
                        {
                            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            httpContent = new StringContent(json);
                        }
                        apiUrl = Path.Combine(apiUrl, methodName);

                        var response = await client.PostAsync(apiUrl, httpContent).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();

                        await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                        {
                            if (x.IsFaulted)
                                throw x.Exception;

                            result = JsonConvert.DeserializeObject<T>(x.Result);


                        });
                        MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
                        await HideProcessingIndicatorPopup();
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task PutRequest(string methodName, object putObject)
        {
            bool isInternetConnected = await ValidateInternetConnectivityAndDisplayMessage();
            if (isInternetConnected)
            {
                using (var client = new HttpClient())
                {
                    await ShowProcessingIndicatorPopup();
                    AuthorizeRequest(client, "PUT", putObject);

                    string json = JsonConvert.SerializeObject(putObject, (Newtonsoft.Json.Formatting)Formatting.Indented);
                    var httpContent = new StringContent(json);
                    string apiUrl = Path.Combine(AppSettings.Current.ApiUrl, methodName);
                    var response = await client.PostAsync(apiUrl, httpContent).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();
                    await HideProcessingIndicatorPopup();
                }
            }
        }

        public static async Task<T> DeleteRequest<T>(string methodName)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = await ValidateInternetConnectivityAndDisplayMessage();
            if (isInternetConnected)
            {
                using (var client = new HttpClient())
                {
                    await ShowProcessingIndicatorPopup();
                    AuthorizeRequest(client, "DELETE");

                    string apiUrl = Path.Combine(AppSettings.Current.ApiUrl, methodName);
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

        private static void AuthorizeRequest(HttpClient client, string methodType, object content = null)
        {
            try
            {
                string requestContentBase64String = string.Empty;

                //Calculate UNIX time
                DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

                //create random nonce for each request
                string nonce = Guid.NewGuid().ToString("N");

                //Checking if the request contains body, usually will be null with HTTP GET and DELETE
                if (content != null)
                {
                    byte[] contentBytes;
                    using (var ms = new MemoryStream())
                    {
                        //new BinaryFormatter().Serialize(ms, content);
                        JsonSerializer.SerializeAsync(ms, content);
                        contentBytes = ms.ToArray();
                    }
                    MD5 md5 = MD5.Create();
                    byte[] requestContentHash = md5.ComputeHash(contentBytes);
                    requestContentBase64String = Convert.ToBase64String(requestContentHash);
                }

                //Creating the raw signature string
                string signatureRawData = requestContentBase64String;
                var secretKeyByteArray = Encoding.ASCII.GetBytes(secretKey);

                byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

                using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
                {
                    byte[] signatureBytes = hmac.ComputeHash(signature);
                    string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("amx",
                        string.Format("{0}:{1}:{2}:{3}:{4}",
                        publicKey, requestSignatureBase64String, nonce, requestTimeStamp, AppSettings.Current.UserSessionUid));
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }

        }

        private static async Task<HttpResponseMessage> GetAsyncApiResponse(HttpClient client, string methodName, string apiUrl)
        {
            try
            {
                AuthorizeRequest(client, "GET");

                apiUrl = Path.Combine(apiUrl, methodName);
                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                    response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<T> PostMultiDataRequestAsync<T>(string methodName, string apiUrl, object postObject = null, List<AttachmentFileView> files = null, bool isLoader = true)
        {
            T result = Activator.CreateInstance<T>();
            bool isInternetConnected = await ValidateInternetConnectivityAndDisplayMessage();
            if (isInternetConnected)
            {
                using (var client = new HttpClient())
                {
                    if(isLoader)
                        await ShowProcessingIndicatorPopup();
                    AuthorizeRequest(client, "POST", postObject);
                    string boundary = "---8d0f01e6b3b5dafaaadaada";
                    string json = string.Empty;
                    using (var multipartContent = new MultipartFormDataContent(boundary))
                    {
                        foreach (var file in files)
                        {
                            if (file.FileData != null)
                            {
                                string contentType = file.FileType switch
                                {
                                    iCampus.Common.Enums.FileTypes.Pdf => "application/pdf",
                                    iCampus.Common.Enums.FileTypes.Jpg => "image/jpeg",
                                    iCampus.Common.Enums.FileTypes.Png => "image/png",
                                    iCampus.Common.Enums.FileTypes.Doc => "application/msword", 
                                    iCampus.Common.Enums.FileTypes.Docx => "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                                    iCampus.Common.Enums.FileTypes.Xls => "application/vnd.ms-excel", 
                                    iCampus.Common.Enums.FileTypes.Xlsx => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    _ => "application/octet-stream" // Fallback for unknown file types
                                };
                                //multipartContent.Add(CreateFileContent(file.FileData, file.FileName, Convert.ToString(file.FileType) ));
                                multipartContent.Add(CreateFileContent(file.FileData, file.FileName, contentType));

                            }
                        }
                        if (postObject != null)
                            json = JsonConvert.SerializeObject(postObject, (Newtonsoft.Json.Formatting)Formatting.Indented);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        multipartContent.Add(content, "postObject");
                        apiUrl = Path.Combine(apiUrl, methodName);
                        var response = await client.PostAsync(apiUrl, multipartContent).ConfigureAwait(false);

                        response.EnsureSuccessStatusCode();

                        await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                        {
                            if (x.IsFaulted)
                                throw x.Exception;

                            result = JsonConvert.DeserializeObject<T>(x.Result);

                        });
                    }
                    if(isLoader)
                        await HideProcessingIndicatorPopup();
                }

            }
            return result;
        }

        public static async Task<bool> ValidateInternetConnectivityAndDisplayMessage()
        {
            bool isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (!isInternetConnected)
            {
                await DisplayNoConnectionMessage();
            }
            return isInternetConnected;
        }

        public static bool ValidateInternetConnectivity()
        {
            bool isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            return isInternetConnected;
        }

        public static async Task DisplayNoConnectionMessage()
        {
            Application.Current.MainPage.ShowPopup(new NetworkAlertPopup());
        }

        public static async Task ShowProcessingIndicatorPopup()
        {
            try
            {
                _processingPopup = new ProcessingIndicatorPopup();
                if (MopupService.Instance.PopupStack.Any(p => p is ProcessingIndicatorPopup))
                {
                    return;
                }
                await MopupService.Instance.PushAsync(_processingPopup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }

        public static async Task HideProcessingIndicatorPopup()
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

        private static ByteArrayContent CreateFileContent(byte[] content, string fileName, string contentType)
        {
            var fileContent = new ByteArrayContent(content);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = fileName,
                FileName = HttpUtility.UrlEncode(fileName)
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
    }