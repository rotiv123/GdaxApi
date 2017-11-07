namespace GdaxApi
{
    using System;
    using System.Globalization;
    using GdaxApi.Orders;
    using GdaxApi.Pagination;

    public interface IOrderClient
    {
        ApiRequestBuilder<Page<OrderResponse>> GetAllOrders();

        ApiRequestBuilder<OrderResponse> PostLimitOrder(LimitOrderRequest order);

        ApiRequestBuilder<OrderResponse> PostMarketOrder(MarketOrderRequest order);

        ApiRequestBuilder<GenericResponse> PostCancelOrder(Guid orderId);
    }

    public class OrderClient : IOrderClient
    {
        private readonly GdaxApiClient api;

        public OrderClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<Page<OrderResponse>> GetAllOrders()
        {
            return this.api.GetAllOrders();
        }

        public ApiRequestBuilder<OrderResponse> PostLimitOrder(LimitOrderRequest order)
        {
            return this.api.PostLimitOrder(order);
        }

        public ApiRequestBuilder<GenericResponse> PostCancelOrder(Guid orderId)
        {
            return this.api.PostCancelOrder(orderId);
        }

        public ApiRequestBuilder<OrderResponse> PostMarketOrder(MarketOrderRequest order)
        {
            return this.api.PostMarketOrder(order);
        }
    }

    public static class OrderClientExtensions
    {
        public static ApiRequestBuilder<Page<OrderResponse>> GetAllOrders(this GdaxApiClient api)
        {
            return api.Get<Page<OrderResponse>>("orders")
                      .AddQueryParam("status", "all");
        }

        public static ApiRequestBuilder<OrderResponse> PostLimitOrder(this GdaxApiClient api, LimitOrderRequest order)
        {
            return api.Post<OrderResponse>("orders")
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

        public static ApiRequestBuilder<OrderResponse> PostMarketOrder(this GdaxApiClient api, MarketOrderRequest order)
        {
            return api.Post<OrderResponse>("orders")
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
