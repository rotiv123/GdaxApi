namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GdaxApi.Utils;

    public class ApiResponse<T> : IDisposable
    {
        private readonly HttpResponseMessage httpReponse;
        private readonly ISerializer serializer;

        public ApiResponse(HttpResponseMessage httpReponse, ISerializer serializer)
        {
            this.httpReponse = httpReponse;
            this.serializer = serializer;
        }

        public HttpResponseMessage HttpReponse => this.httpReponse;

        public Task<T> Content => ReadAsync();

        protected ISerializer Serializer => this.serializer;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.httpReponse.Dispose();
            }
        }

        protected virtual async Task<T> ReadAsync()
        {
            var str = await this.httpReponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            return this.serializer.Deserialize<T>(str);
        }
    }

    public static class ApiResponseExtensions
    {
        public static async Task<T> SendAsync<T>(this ApiRequestBuilder<T> builder)
        {
            var request = builder.Build();
            var httpResponse = await request.Api.HttpClient.SendAsync(request).ConfigureAwait(false);
            using (var response = new ApiResponse<T>(httpResponse, request.Api.Serializer))
            {
                return await response.Content.ConfigureAwait(false);
            }
        }
    }
}
