namespace GdaxApi.Products
{
    public class Last24HourStat
    {
        public Last24HourStat()
        {
        }

        internal Last24HourStat(dynamic data)
        {
            this.Low = data.low;
            this.High = data.high;
            this.Open = data.open;
            this.Volume = data.volume;
        }

        public decimal Low { get; set; }

        public decimal High { get; set; }

        public decimal Open { get; set; }

        public decimal Volume { get; set; }
    }
}
