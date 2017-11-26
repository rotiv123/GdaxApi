namespace GdaxApi.Products
{
    using System;

    public class Trade
    {
        public Trade()
        {
        }

        internal Trade(dynamic data)
        {
            this.Time = DateTimeOffset.Parse(data.time.ToString());
            this.TradeId = data.trade_id;
            this.Price = data.price;
            this.Size = data.size;
            this.Side = Enum.Parse(typeof(TradeSide), data.side.ToString(), true);
        }

        public DateTimeOffset Time { get; set; }

        public long TradeId { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public TradeSide Side { get; set; }
    }
}
