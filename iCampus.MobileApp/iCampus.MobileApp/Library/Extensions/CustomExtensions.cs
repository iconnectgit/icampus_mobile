namespace iCampus.MobileApp.Library.Extensions;

public static class CustomExtensions
{
    public static string FormatPath(this string source)
    {
        return source = source.Replace("\\", "/");
    }

    public static bool IsCacheExpired(this DateTime lastRefreshedDateTime, short cachePeriodInMins)
    {
        return lastRefreshedDateTime != null && (DateTime.Now.Subtract(lastRefreshedDateTime).TotalMinutes > cachePeriodInMins);
    }
}