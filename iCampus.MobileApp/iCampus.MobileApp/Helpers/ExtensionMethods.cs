using Newtonsoft.Json;

namespace iCampus.MobileApp.Helpers;

public static class ExtensionMethods
{
    public static T CloneJson<T>(this T source)
    {
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }
        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
    }
}