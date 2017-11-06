namespace GdaxApi
{
    using GdaxApi.Pagination;

    public static class Paginator
    {
        public static ApiRequestBuilder<Page<T>> Before<T>(this ApiRequestBuilder<Page<T>> builder, int before)
        {
            return builder.AddQueryParam("before", before);
        }

        public static ApiRequestBuilder<Page<T>> After<T>(this ApiRequestBuilder<Page<T>> builder, int after)
        {
            return builder.AddQueryParam("after", after);
        }

        public static ApiRequestBuilder<Page<T>> Limit<T>(this ApiRequestBuilder<Page<T>> builder, int limit)
        {
            return builder.AddQueryParam("limit", limit);
        }
    }
}
