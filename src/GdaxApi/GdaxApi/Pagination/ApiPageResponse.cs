namespace GdaxApi
{
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using GdaxApi.Pagination;
    using GdaxApi.Utils;

    public class ApiPageResponse<T, U> : ApiResponse<Page<T, U>>
    {
        private static readonly MethodInfo ParseMethod =
            typeof(U).GetMethods(BindingFlags.Public | BindingFlags.Static)
                     .Where(x => x.Name == "Parse" && x.GetParameters().Length == 1)
                     .Single();

        private const string BeforeHeader = "cb-before";
        private const string AfterHeader = "cb-after";

        public ApiPageResponse(HttpResponseMessage httpReponse, ISerializer serializer)
            : base(httpReponse, serializer)
        {
        }

        protected override async Task<Page<T, U>> ReadAsync()
        {
            var str = await this.HttpReponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var page = new Page<T, U> { Items = this.Serializer.Deserialize<T[]>(str) };

            if (this.HttpReponse.Headers.TryGetValues(BeforeHeader, out var beforeValues))
            {
                page.Before = (U)ParseMethod.Invoke(null, new object[] { beforeValues.First() });
            }

            if (this.HttpReponse.Headers.TryGetValues(AfterHeader, out var afterValues))
            {
                page.After = (U)ParseMethod.Invoke(null, new object[] { afterValues.First() });
            }

            return page;
        }
    }

    public static class ApiResponsePageExtensions
    {
        public static async Task<Page<T, U>> SendAsync<T, U>(this ApiRequestBuilder<Page<T, U>> builder)
        {
            var request = builder.Build();
            return await builder.Api.SendAsync(request, httpResponse => new ApiPageResponse<T, U>(httpResponse, builder.Api.Serializer)).ConfigureAwait(false);
        }
    }
}
