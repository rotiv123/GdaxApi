namespace GdaxApi.Time
{
    using System;

    public class TimeResponse
    {
        public DateTimeOffset Timestamp { get; set; }

        public double UnixTimestamp { get; set; }

        public TimeResponse()
        {
        }

        public TimeResponse(dynamic data)
        {
            this.Timestamp = data.iso;
            this.UnixTimestamp = data.epoch;
        }
    }
}
