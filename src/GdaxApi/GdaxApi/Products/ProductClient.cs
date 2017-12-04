namespace GdaxApi
{
    using System;
    using GdaxApi.Pagination;
    using GdaxApi.Products;

    public interface IProductClient
    {
        ApiRequestBuilder<CandleSet> GetCandles(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity);

        ApiRequestBuilder<Page<Trade, long>> GetTrades(string productId);
    }

    public class ProductClient : IProductClient
    {
        private readonly GdaxApiClient api;

        public ProductClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<CandleSet> GetCandles(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity)
        {
            return this.api.GetCandles(productId, start, end, granularity);
        }

        public ApiRequestBuilder<Page<Trade, long>> GetTrades(string productId)
        {
            return this.api.GetTrades(productId);
        }

        public ApiRequestBuilder<AggregatedBook> GetBookLevel2(string productId)
        {
            return this.api.GetBookLevel2(productId);
        }

        public ApiRequestBuilder<AggregatedBook> GetBookLevel1(string productId)
        {
            return this.api.GetBookLevel2(productId);
        }
    }

    public static class ProductClientExtensions
    {
        public static ApiRequestBuilder<CandleSet> GetCandles(this GdaxApiClient api, string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity)
        {
            return api.Get<CandleSet>($"products/{productId}/candles")
                      .AddQueryParam("start", start.ToString("u"))
                      .AddQueryParam("end", end.ToString("u"))
                      .AddQueryParam("granularity", (int)granularity.TotalSeconds);
        }

        public static ApiRequestBuilder<Page<Trade, long>> GetTrades(this GdaxApiClient api, string productId)
        {
            return api.Get<Page<Trade, long>>($"products/{productId}/trades");
        }

        public static ApiRequestBuilder<AggregatedBook> GetBookLevel2(this GdaxApiClient api, string productId)
        {
            return api.Get<AggregatedBook>($"products/{productId}/book")
                      .AddQueryParam("level", 2);
        }

        public static ApiRequestBuilder<AggregatedBook> GetBookLevel1(this GdaxApiClient api, string productId)
        {
            return api.Get<AggregatedBook>($"products/{productId}/book")
                      .AddQueryParam("level", 1);
        }
    }
}
