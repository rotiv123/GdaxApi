namespace GdaxApi
{
    using GdaxApi.Pagination;

    public static class Paginator
    {
        public static ApiRequestBuilder<Page<T, U>> Before<T, U>(this ApiRequestBuilder<Page<T, U>> builder, object before)
        {
            return builder.AddQueryParam("before", before);
        }

        public static ApiRequestBuilder<Page<T, U>> After<T, U>(this ApiRequestBuilder<Page<T, U>> builder, object after)
        {
            return builder.AddQueryParam("after", after);
        }

        public static ApiRequestBuilder<Page<T, U>> Limit<T, U>(this ApiRequestBuilder<Page<T, U>> builder, int limit)
        {
            return builder.AddQueryParam("limit", limit);
        }
    }
}
