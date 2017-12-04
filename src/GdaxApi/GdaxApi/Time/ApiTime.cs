namespace GdaxApi.Time
{
    using System;

    public class ApiTime
    {
        public DateTimeOffset Timestamp { get; set; }

        public double UnixTimestamp { get; set; }

        public ApiTime()
        {
        }

        internal ApiTime(dynamic data)
        {
            this.Timestamp = data.iso;
            this.UnixTimestamp = data.epoch;
        }
    }
}
