namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using System.Text;

    public class ApiRequestBuilder<T>
    {
        private readonly GdaxApiClient api;
        private readonly UriBuilder uriBuilder;
        private readonly HttpMethod method;
        private string content;

        public ApiRequestBuilder(GdaxApiClient api, HttpMethod method, string path)
        {
            this.api = api;
            this.method = method;
            this.uriBuilder = new UriBuilder(api.BaseUri)
            {
                Path = path
            };
        }

        internal GdaxApiClient Api => this.api;

        public ApiRequestBuilder<T> Content(object content)
        {
            this.content = this.api.Serializer.Serialize(content);
            return this;
        }

        public ApiRequestBuilder<T> AddQueryParam(string name, object value)
        {
            if (this.uriBuilder.Query != null)
            {
                this.uriBuilder.Query += $"&{name}={value}";
            }
            else
            {
                this.uriBuilder.Query = $"?{name}={value}";
            }

            return this;
        }

        public HttpRequestMessage Build()
        {
            var req = Create(this.uriBuilder.Uri);
            if (this.content != null)
            {
                req.Content = new StringContent(this.content, Encoding.UTF8, "application/json");
            }

            return req;
        }

        protected virtual HttpRequestMessage Create(Uri uri)
        {
            var req = new HttpRequestMessage(this.method, uri);
            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("User-Agent", "GdaxApiClient");
            return req;
        }
    }
}
