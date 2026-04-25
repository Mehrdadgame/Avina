using System.Globalization;

namespace Avina.Helpers;

public static class PersianDateHelper
{
    private static readonly PersianCalendar Calendar = new();

    public static string ToShamsiDate(this DateTime value)
    {
        var local = ToTehranTime(value);
        var year = Calendar.GetYear(local);
        var month = Calendar.GetMonth(local);
        var day = Calendar.GetDayOfMonth(local);
        return $"{year:0000}/{month:00}/{day:00}";
    }

    public static string ToShamsiDateTime(this DateTime value)
    {
        var local = ToTehranTime(value);
        var date = local.ToShamsiDate();
        return $"{date} {local:HH:mm}";
    }

    private static DateTime ToTehranTime(DateTime value)
    {
        if (value.Kind == DateTimeKind.Unspecified)
        {
            return value;
        }

        try
        {
            var tehran = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            var utc = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
            return TimeZoneInfo.ConvertTimeFromUtc(utc, tehran);
        }
        catch
        {
            return value.ToLocalTime();
        }
    }
}
