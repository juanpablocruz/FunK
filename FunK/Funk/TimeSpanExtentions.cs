using System;

namespace FunK
{
    public static class TimeSpanExtentions
    {
        public static TimeSpan Days(this int value) => TimeSpan.FromDays(value);

        public static TimeSpan Hours(this int value) => TimeSpan.FromHours(value);

        public static TimeSpan Minutes(this int value) => TimeSpan.FromMinutes(value);

        public static TimeSpan Seconds(this int value) => TimeSpan.FromSeconds(value);

        public static TimeSpan Milliseconds(this int value) => TimeSpan.FromMilliseconds(value);

        public static TimeSpan Ticks(this int value) => TimeSpan.FromTicks(value);

        public static DateTime Ago(this TimeSpan @this) => DateTime.UtcNow - @this;
    }
}
