namespace GdaxApi
{
    using System;
    using System.Net.Http;

    public class ApiRequest<T> : HttpRequestMessage
    {
        private readonly GdaxApiClient api;

        public ApiRequest(GdaxApiClient api, HttpMethod method, Uri requestUri)
            : base(method, requestUri)
        {
            this.api = api;
        }

        internal GdaxApiClient Api => this.api;
    }
}
