namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;

    public static class DateAndTimeUtils
    {
        public static string ToFormattedString(this TimeSpan span)
        {
            // Handle the case where the timespan is zero
            if (span == TimeSpan.Zero)
            {
                return "0 seconds";
            }

            IList<string> stringParts = new List<string>();

            // Days
            if (span.Days != 0)
            {
                string daysLabel = span.Days == 1 ? "day" : "days";
                stringParts.Add($"{span.Days} {daysLabel}");
            }

            // Hours
            if (span.Hours != 0)
            {
                string hoursLabel = span.Hours == 1 ? "hour" : "hours";
                stringParts.Add($"{span.Hours} {hoursLabel}");
            }

            // Minutes
            if (span.Minutes != 0)
            {
                string minutesLabel = span.Minutes == 1 ? "minute" : "minutes";
                stringParts.Add($"{span.Minutes} {minutesLabel}");
            }

            // Seconds and milliseconds
            if (span.Seconds != 0 || span.Milliseconds != 0)
            {
                string millisecondsPart = span.Milliseconds == 0 ? string.Empty : $".{span.Milliseconds}";
                string secondsLabel = span.Seconds == 1 && span.Milliseconds == 0 ? "second" : "seconds";
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
