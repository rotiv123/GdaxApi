namespace GdaxApi.Products
{
    using System;
    using System.Linq;
    using GdaxApi.Utils;
    using Newtonsoft.Json.Linq;

    public class CandleSet
    {
        public Candle[] Buckets { get; set; }

        public CandleSet()
        {
            this.Buckets = new Candle[0];
        }

        internal CandleSet(JArray data)
        {
            this.Buckets = data.Select(
                x => new Candle
                {
                    Time = DateTimeExtensions.FromUnixTimestamp(x[0].Value<double>()),
                    Low = x[1].Value<decimal>(),
                    High = x[2].Value<decimal>(),
                    Open = x[3].Value<decimal>(),
                    Close = x[4].Value<decimal>(),
                    Volume = x[5].Value<decimal>()
                }).ToArray();
        }

        public class Candle
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
