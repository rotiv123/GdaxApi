namespace GdaxApi.Orders
{
    using System;

    public class OrderResponse
    {
        public Guid Id { get; set; }

        public decimal? Price { get; set; }

        public decimal? Size { get; set; }
        
        public string ProductId { get; set; }

        public string Side { get; set; }

        public string Type { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Status { get; set; }

        public bool Settled { get; set; }

        public OrderResponse()
        {
        }

        public OrderResponse(dynamic data)
        {
            this.Id = data.id;
            this.Price = data.price;
            this.Size = data.size;
            this.ProductId = data.product_id;
            this.Side = data.side;
            this.Type = data.type;
            this.CreatedAt = data.created_at;
            this.Status = data.status;
            this.Settled = data.settled;
        }
    }
}
