namespace GdaxApi
{
    using System;
    using GdaxApi.Pagination;
    using GdaxApi.Products;

    public interface IProductClient
    {
        ApiRequestBuilder<CandleResponse> GetCandles(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity);

        ApiRequestBuilder<Page<TradeResponse, long>> GetTrades(string productId);
    }

    public class ProductClient : IProductClient
    {
        private readonly GdaxApiClient api;

        public ProductClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<CandleResponse> GetCandles(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity)
        {
            return this.api.GetCandles(productId, start, end, granularity);
        }

        public ApiRequestBuilder<Page<TradeResponse, long>> GetTrades(string productId)
        {
            return this.api.GetTrades(productId);
        }
    }

    public static class ProductClientExtensions
    {
        public static ApiRequestBuilder<CandleResponse> GetCandles(this GdaxApiClient api, string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity)
        {
            return api.Get<CandleResponse>($"products/{productId}/candles")
                      .AddQueryParam("start", start.ToString("u"))
                      .AddQueryParam("end", end.ToString("u"))
                      .AddQueryParam("granularity", (int)granularity.TotalSeconds);
        }

        public static ApiRequestBuilder<Page<TradeResponse, long>> GetTrades(this GdaxApiClient api, string productId)
        {
            return api.Get<Page<TradeResponse, long>>($"products/{productId}/trades");
        }
    }
}
