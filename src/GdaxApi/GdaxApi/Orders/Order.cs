namespace GdaxApi.Orders
{
    using System;

    public class Order
    {
        public Guid Id { get; set; }

        public decimal? Price { get; set; }

        public decimal? Size { get; set; }
        
        public string ProductId { get; set; }

        public OrderSide Side { get; set; }

        public OrerType Type { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Status { get; set; }

        public bool Settled { get; set; }

        public Order()
        {
        }

        public Order(dynamic data)
        {
            this.Id = data.id;
            this.Price = data.price;
            this.Size = data.size;
            this.ProductId = data.product_id;
            this.Side = Enum.Parse(typeof(OrderSide), data.side.ToString(), true);
            this.Type = Enum.Parse(typeof(OrerType), data.type.ToString(), true);
            this.CreatedAt = data.created_at;
            this.Status = data.status;
            this.Settled = data.settled;
        }
    }
}
