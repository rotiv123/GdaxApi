namespace GdaxApi
{
    using System;
    using System.Globalization;
    using GdaxApi.Orders;
    using GdaxApi.Pagination;

    public interface IOrderClient
    {
        ApiRequestBuilder<Page<Order, DateTimeOffset>> GetAllOrders();

        ApiRequestBuilder<Order> PostLimitOrder(LimitOrderRequest order);

        ApiRequestBuilder<Order> PostMarketOrder(MarketOrderRequest order);

        ApiRequestBuilder<GenericResponse> PostCancelOrder(Guid orderId);
    }

    public class OrderClient : IOrderClient
    {
        private readonly GdaxApiClient api;

        public OrderClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<Page<Order, DateTimeOffset>> GetAllOrders()
        {
            return this.api.GetAllOrders();
        }

        public ApiRequestBuilder<Order> PostLimitOrder(LimitOrderRequest order)
        {
            return this.api.PostLimitOrder(order);
        }

        public ApiRequestBuilder<GenericResponse> PostCancelOrder(Guid orderId)
        {
            return this.api.PostCancelOrder(orderId);
        }

        public ApiRequestBuilder<Order> PostMarketOrder(MarketOrderRequest order)
        {
            return this.api.PostMarketOrder(order);
        }
    }

    public static class OrderClientExtensions
    {
        public static ApiRequestBuilder<Page<Order, DateTimeOffset>> GetAllOrders(this GdaxApiClient api)
        {
            return api.Get<Page<Order, DateTimeOffset>>("orders")
                      .AddQueryParam("status", "all");
        }

        public static ApiRequestBuilder<Order> PostLimitOrder(this GdaxApiClient api, LimitOrderRequest order)
        {
            return api.Post<Order>("orders")
                      .Content(
                        new
                        {
                            type = "limit",
                            side = order.Side.ToString().ToLower(),
                            product_id = order.ProductId,
                            price = order.Price.ToString(CultureInfo.InvariantCulture),
                            size = order.Size.ToString(CultureInfo.InvariantCulture),
                        });
        }

        public static ApiRequestBuilder<Order> PostMarketOrder(this GdaxApiClient api, MarketOrderRequest order)
        {
            return api.Post<Order>("orders")
                      .Content(
                        new
                        {
                            type = "market",
                            side = order.Side.ToString().ToLower(),
                            product_id = order.ProductId,
                            funds = order.Funds?.ToString(CultureInfo.InvariantCulture),
                            size = order.Size?.ToString(CultureInfo.InvariantCulture),
                        });
        }

        public static ApiRequestBuilder<GenericResponse> PostCancelOrder(this GdaxApiClient api, Guid orderId)
        {
            return api.Delete<GenericResponse>($"orders/{orderId}");
        }
    }
}
