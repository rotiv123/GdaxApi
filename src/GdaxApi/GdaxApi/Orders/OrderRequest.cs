namespace GdaxApi.Orders
{
    public abstract class OrderRequest
    {
        public OrderSide Side { get; }

        public string ProductId { get; }

        public OrderType Type { get; }

        protected OrderRequest(OrderType type, OrderSide side, string productId)
        {
            this.Type = type;
            this.Side = side;
            this.ProductId = productId;
        }
    }

    public class LimitOrderRequest: OrderRequest
    {
        public LimitOrderRequest(OrderSide side, string productId)
            : base(OrderType.Limit, side, productId)
        {
        }

        public decimal Size { get; set; }

        public decimal Price { get; set; }
    }

    public class MarketOrderRequest: OrderRequest
    {
        public MarketOrderRequest(OrderSide side, string productId)
            : base(OrderType.Market, side, productId)
        {
        }

        public decimal? Size { get; set; }

        /// <summary>
        ///  Desired amount of quote currency to use.
        /// </summary>
        public decimal? Funds { get; set; }
    }
}
