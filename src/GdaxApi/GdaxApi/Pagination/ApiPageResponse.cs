namespace GdaxApi
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GdaxApi.Pagination;
    using GdaxApi.Utils;

    public class ApiPageResponse<T> : ApiResponse<Page<T>>
    {
        private const string BeforeHeader = "cb-before";
        private const string AfterHeader = "cb-after";

        public ApiPageResponse(HttpResponseMessage httpReponse, ISerializer serializer)
            : base(httpReponse, serializer)
        {
        }

        protected override async Task<Page<T>> ReadAsync()
        {
            var str = await this.HttpReponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var page = new Page<T> { Items = this.Serializer.Deserialize<T[]>(str) };

            if (this.HttpReponse.Headers.TryGetValues(BeforeHeader, out var beforeValues))
            {
                page.Before = DateTimeOffset.Parse(beforeValues.First());
            }

            if (this.HttpReponse.Headers.TryGetValues(AfterHeader, out var afterValues))
            {
                page.After = DateTimeOffset.Parse(afterValues.First());
            }

            return page;
        }
    }

    public static class ApiResponsePageExtensions
    {
        public static async Task<Page<T>> SendAsync<T>(this ApiRequestBuilder<Page<T>> builder)
        {
            var request = builder.Build();
            var httpResponse = await request.Api.HttpClient.SendAsync(request).ConfigureAwait(false);
            using (var response = new ApiPageResponse<T>(httpResponse, request.Api.Serializer))
            {
                return await response.Content.ConfigureAwait(false);
            }
        }
    }
}
