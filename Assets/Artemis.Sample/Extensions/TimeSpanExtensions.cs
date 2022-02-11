using System;
using System.Collections.Generic;

namespace Artemis.Sample.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadableString(this TimeSpan timeSpan)
        {
            var components = new List<string>();

            if (Math.Abs(timeSpan.Days) > 0)
            {
                components.Add($"{timeSpan.Days}d");
            }

            if (Math.Abs(timeSpan.Hours) > 0)
            {
                components.Add($"{timeSpan.Hours}h");
            }

            if (Math.Abs(timeSpan.Minutes) > 0)
            {
                components.Add($"{timeSpan.Minutes}m");
            }

            if (Math.Abs(timeSpan.Seconds) > 0)
            {
                components.Add($"{timeSpan.Seconds}s");
            }

            if (Math.Abs(timeSpan.Milliseconds) > 0)
            {
                components.Add($"{timeSpan.Milliseconds}ms");
            }

            return string.Join(" ", components);
        }
    }
}