using System.Globalization;

namespace Blog.Infrastructure.Services;

public class DateTimeFormatter
{
    public static string FormatCustomDate(DateTime dateTime)
    {
        return dateTime.ToString("dd MMM (HH:mm)", CultureInfo.InvariantCulture);
    }
}