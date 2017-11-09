namespace GdaxApi.Products
{
    using System;
    using System.Linq;
    using GdaxApi.Utils;
    using Newtonsoft.Json.Linq;

    public class CandleResponse
    {
        public Bucket[] Buckets { get; set; }

        public CandleResponse()
        {
            this.Buckets = new Bucket[0];
        }

        public CandleResponse(JArray data)
        {
            this.Buckets = data.Select(
                x => new Bucket
                {
                    Time = DateTimeExtensions.FromUnixTimestamp(x[0].Value<double>()),
                    Low = x[1].Value<decimal>(),
                    High = x[2].Value<decimal>(),
                    Open = x[3].Value<decimal>(),
                    Close = x[4].Value<decimal>(),
                    Volume = x[5].Value<decimal>()
                }).ToArray();
        }

        public class Bucket
        {
            public DateTimeOffset Time { get; set; }

            public decimal Low { get; set; }

            public decimal High { get; set; }

            public decimal Open { get; set; }

            public decimal Close { get; set; }

            public decimal Volume { get; set; }
        }
    }
}
