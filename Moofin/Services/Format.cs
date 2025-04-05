using System;
using System.Globalization;

namespace Moofin.Services
{
    public static class Format
    {
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        public static string FormatPercentage(double value)
        {
            return $"{value:0.0}%";
        }
        public static string FormatDate(DateTime dateTime, string? format = null)
        {
            return dateTime.ToString(format ?? "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public static string FormatShortDate(DateTime dateTime)
        {
            return dateTime.ToString("d MMM yyyy", CultureInfo.InvariantCulture); // e.g. 5 Apr 2025
        }

        public static string FormatDurationMinutes(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalMinutes} min";
        }

        public static string FormatDurationHours(TimeSpan timeSpan)
        {
            return $"{timeSpan.TotalHours:0.0} h";
        }

        public static string FormatWithSign(double value, string? suffix = null)
        {
            string sign = value >= 0 ? "+" : "-";
            return $"{sign}{Math.Abs(value):0.0}{suffix}";
        }

        public static string FormatTitleCase(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }
    }
}