using System.Collections;
using System.Reactive.Linq;
using System.Reflection;
using Akavache;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Helpers;

public class ICCacheManager
    {
        public static async Task<DateTimeOffset?> GetCreatedAt(string key)
        {
            try
            {
                return await BlobCache.UserAccount.GetCreatedAt(key);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return null;
            }
        }
        public static async Task<(bool, T)> GetObjectData<T>(string key, bool isInternetConnected)
        {
            try
            {
                if (isInternetConnected)
                {
                    if (await CheckIfCacheExpired(key))
                    {
                        return (false, Activator.CreateInstance<T>());
                    }
                }
                var result = await BlobCache.UserAccount.GetObject<T>(key);
                if (result != null)
                {
                    Type modelType = result.GetType();
                    PropertyInfo[] properties = modelType.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            var dateTimeValue = property.GetValue(result) as DateTime?;
                            if (dateTimeValue.HasValue)
                            {
                                // Ensure DateTime is converted to Local time
                                property.SetValue(result, dateTimeValue.Value.ToLocalTime());
                            }
                        }
                    }
                }
                return (true, result);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return (false, Activator.CreateInstance<T>());
            }
        }
        public static async Task<(bool, T)> GetObjectData<T>(string key)
        {
            try
            {
                var result = await BlobCache.UserAccount.GetObject<T>(key);
                return (true, result);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return (false, Activator.CreateInstance<T>());
            }
        }
        public static async Task<T> GetObject<T>(string key)
        {
            try
            {
                return await BlobCache.UserAccount.GetObject<T>(key);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return Activator.CreateInstance<T>();
            }
        }
        public static async Task<T> GetSecureObject<T>(string key)
        {
            try
            {
                return await BlobCache.Secure.GetObject<T>(key);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return Activator.CreateInstance<T>();
            }
        }
        public static async Task<(bool, List<T>)> GetObjectList<T>(string key, bool isInternetConnected)
        {
            List<T> result = new List<T>();
            try
            {
                if (isInternetConnected)
                {
                    if (await CheckIfCacheExpired(key))
                    {
                        return (false, result);
                    }
                }
                result = await BlobCache.UserAccount.GetObject<List<T>>(key);
                return (true, result);
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return (false, result);
            }
        }
        public static async void SaveObjectWithCacheLimit<T>(string key, T value)
        {
            try
            {
                T data = value.CloneJson();
                Type modelType = data.GetType();
                PropertyInfo[] sourceValue = modelType.GetProperties();
                foreach (PropertyInfo property in sourceValue)
                {
                    if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        var dateTimeValue = property.GetValue(data) as DateTime?;
                        if (dateTimeValue.HasValue)
                        {
                            property.SetValue(data, DateTime.SpecifyKind(dateTimeValue.Value, DateTimeKind.Local));
                        }
                    }
                    if (property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) || property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        var result = property.GetValue(data);
                        var genericType = property.PropertyType.GetGenericArguments().First();
                        var instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));
                        if (result != null)
                        {
                            var enumerator = ((IEnumerable)result).GetEnumerator();
                            var count = 0;
                            while (enumerator.MoveNext())
                            {
                                if (count < AppSettings.Current.CacheRecordSize)
                                {
                                    instance.GetType().GetMethod("Add").Invoke(instance, new[] { enumerator.Current });
                                    count++;
                                }
                                else
                                    break;
                            }
                            property.SetValue(data, instance);
                        }
                    }
                }
                await BlobCache.UserAccount.InsertObject<T>(key, data);
                BlobCache.UserAccount.Flush().Wait();
            }
            catch(Exception ex)
            {
                //Crashes.TrackError(ex);
            }    
        }

        public static async void SaveObject<T>(string key, T value)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject<T>(key, value);
                BlobCache.UserAccount.Flush().Wait();
            }
            catch(Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public static async Task SaveSecureObject<T>(string key, T value)
        {
            try
            {
                await BlobCache.Secure.InsertObject<T>(key, value);
                BlobCache.Secure.Flush().Wait();
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }
        public static async void SaveObjectList<T>(string key, List<T> value,bool isCacheAllRecords = false)
        {
            var cacheRecordData = isCacheAllRecords ? value : value.Take(AppSettings.Current.CacheRecordSize).ToList();
            await BlobCache.UserAccount.InsertObject<List<T>>(key, cacheRecordData);
            BlobCache.UserAccount.Flush().Wait();
        }
        public static async Task<IEnumerable<string>> GetAllKeys()
        {
            return await BlobCache.UserAccount.GetAllKeys();
        }
        public static async Task InvalidateObject<T>(string key)
        {
            await BlobCache.UserAccount.InvalidateObject<List<T>>(key);
        }

        public static async Task InvalidateObjects<T>(IEnumerable<string> keys)
        {
            await BlobCache.UserAccount.InvalidateObjects<List<T>>(keys);
        }

        public static async Task ClearCache()
        {
            var appSettingsCached = AppSettings.Current;
            await BlobCache.UserAccount.InvalidateAll();
            if (appSettingsCached != null)
            {
                AppSettings.Current = appSettingsCached;
                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
            }
        }
        public static void CloseCache()
        {
            BlobCache.Shutdown().Wait();
        }
        public static async Task<bool> CheckIfCacheExpired(string key)
        {
            try
            {
                int cacheValidityPeriodInMins = Convert.ToInt32(TextResource.CacheValidityPeriodInMins);
                var createdAt = await GetCreatedAt(key);
                int cachedDataDurationInMinutes = 0;
                if (createdAt.HasValue)
                {
                    cachedDataDurationInMinutes = (int)DateTime.Now.Subtract(createdAt.Value.LocalDateTime).TotalMinutes;
                }
                if (cachedDataDurationInMinutes >= cacheValidityPeriodInMins)
                {
                    return true;
                }
            }
            catch (KeyNotFoundException ex)
            {
                //Crashes.TrackError(ex);
                return false;
            }
            return false;
        }

    }