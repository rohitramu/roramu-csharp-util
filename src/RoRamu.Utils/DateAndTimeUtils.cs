namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Utility methods for working with dates and times.
    /// </summary>
    public static class DateAndTimeUtils
    {
        /// <summary>
        /// Formats a TimeSpan into a human readable string.
        /// </summary>
        /// <param name="span">The time span.</param>
        /// <param name="forceIncludeDays">Include the number of days even if it is zero.</param>
        /// <param name="forceIncludeHours">Include the number of hours even if it is zero.</param>
        /// <param name="forceIncludeMinutes">Include the number of minutes even if it is zero.</param>
        /// <param name="forceIncludeSecondsAndMilliseconds">Include the number of seconds and milliseconds even if they are zero.</param>
        /// <returns>The formatted string.</returns>
        public static string ToFormattedString(this TimeSpan span,
            bool forceIncludeDays = false,
            bool forceIncludeHours = false,
            bool forceIncludeMinutes = false,
            bool forceIncludeSecondsAndMilliseconds = false)
        {
            // Handle the case where the timespan is zero
            if (span == TimeSpan.Zero)
            {
                forceIncludeSecondsAndMilliseconds = true;
            }

            IList<string> stringParts = new List<string>();

            // Days
            if (forceIncludeDays || span.Days != 0)
            {
                string daysLabel = span.Days == 1 ? "day" : "days";
                stringParts.Add($"{span.Days} {daysLabel}");
            }

            // Hours
            if (forceIncludeHours || span.Hours != 0)
            {
                string hoursLabel = span.Hours == 1 ? "hour" : "hours";
                stringParts.Add($"{span.Hours} {hoursLabel}");
            }

            // Minutes
            if (forceIncludeMinutes || span.Minutes != 0)
            {
                string minutesLabel = span.Minutes == 1 ? "minute" : "minutes";
                stringParts.Add($"{span.Minutes} {minutesLabel}");
            }

            // Seconds and milliseconds
            if (forceIncludeSecondsAndMilliseconds || span.Seconds != 0 || span.Milliseconds != 0)
            {
                string millisecondsPart = span.Milliseconds == 0 ? string.Empty : $".{span.Milliseconds}";
                string secondsLabel = span.Seconds == 1 && string.IsNullOrEmpty(millisecondsPart) ? "second" : "seconds";
                stringParts.Add($"{span.Seconds}{millisecondsPart} {secondsLabel}");
            }

            // Include the word "and" before the last part
            if (stringParts.Count > 1)
            {
                stringParts.Insert(stringParts.Count - 1, "and");
            }

            // Handle negative timespan
            string result = span < TimeSpan.Zero ? "-" : string.Empty;
            result += string.Join(" ", stringParts);

            return result;
        }
    }
}
