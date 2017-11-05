namespace GdaxApi.Orders
{
    public abstract class OrderRequest
    {
        public OrderSide Side { get; set; }

        public string ProductId { get; set; }

        public OrerType Type { get; }

        protected OrderRequest(OrerType type)
        {
            this.Type = type;
        }

        public enum OrerType
        {
            Limit,
            Stop
        }

        public enum OrderSide
        {
            Buy,
            Sell
        }
    }

    public class LimitOrderRequest : OrderRequest
    {
        public LimitOrderRequest()
            : base(OrerType.Limit)
        {
        }
        
        public decimal Size { get; set; }

        public decimal Price { get; set; }
    }

    public class StopOrderRequest : OrderRequest
    {
        public StopOrderRequest()
            : base(OrerType.Stop)
        {
        }

        /// <summary>
        /// Desired price at which the stop order triggers.
        /// </summary>
        public decimal Price { get; set; }

        public decimal? Size { get; set; }

        /// <summary>
        ///  Desired amount of quote currency to use.
        /// </summary>
        public decimal? Funds { get; set; }
    }
}
