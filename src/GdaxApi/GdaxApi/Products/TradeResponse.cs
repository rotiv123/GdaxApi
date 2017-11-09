namespace GdaxApi.Products
{
    using System;

    public class TradeResponse
    {
        public TradeResponse()
        {
        }

        public TradeResponse(dynamic data)
        {
            this.Time = DateTimeOffset.Parse(data.time.ToString());
            this.TradeId = data.trade_id;
            this.Price = data.price;
            this.Size = data.size;
            this.Side = data.side;
        }

        public DateTimeOffset Time { get; set; }

        public long TradeId { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public string Side { get; set; }
    }
}
