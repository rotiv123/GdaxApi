namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using GdaxApi.Authentication;
    using GdaxApi.Utils;

    public class GdaxApiClient : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly ISerializer serializer;
        private readonly bool hasOwnershipOfHttpClient;

        public GdaxApiClient(ISerializer serializer, GdaxCredentials credentials)
        {
            this.serializer = serializer;
            this.httpClient = new HttpClient(new GdaxAuthenticationHandler(credentials));
            this.hasOwnershipOfHttpClient = true;
        }

        public GdaxApiClient(ISerializer serializer, HttpClient httpClient)
        {
            this.serializer = serializer;
            this.httpClient = httpClient;
            this.hasOwnershipOfHttpClient = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal ISerializer Serializer => this.serializer;

        internal HttpClient HttpClient => this.httpClient;

        internal ApiGetRequestBuilder<T> Get<T>(string path)
        {
            return new ApiGetRequestBuilder<T>(this, path);
        }

        internal ApiPostRequestBuilder<T> Post<T>(string path)
        {
            return new ApiPostRequestBuilder<T>(this, path);
        }

        internal ApiDeleteRequestBuilder<T> Delete<T>(string path)
        {
            return new ApiDeleteRequestBuilder<T>(this, path);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.hasOwnershipOfHttpClient)
                {
                    this.httpClient.Dispose();
                }
            }
        }
    }
}
