namespace GdaxApi.Utils
{
    using System;

    public interface IDateProvider
    {
        DateTimeOffset Now { get; }

        double UnixTimestamp { get; }
    }

    public class DefaultDateProvider : IDateProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public double UnixTimestamp => DateTime.UtcNow.ToUnixTimestamp();
    }

    internal static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);

        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime - UnixEpoch).TotalSeconds;
        }

        public static DateTimeOffset FromUnixTimestamp(double timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }
    }
}
