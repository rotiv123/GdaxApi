namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GdaxApi.Exceptions;
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

            if (this.httpReponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GdaxApiException($"{this.httpReponse.StatusCode} {str}");
            }

            return this.serializer.Deserialize<T>(str);
        }
    }

    public static class ApiResponseExtensions
    {
        public static Task<T> SendAsync<T>(this ApiRequestBuilder<T> builder)
        {
            var request = builder.Build();
            return builder.Api.SendAsync<T>(request, httpResponse => new ApiResponse<T>(httpResponse, builder.Api.Serializer));
        }
    }
}
