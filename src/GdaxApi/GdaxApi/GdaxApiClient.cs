namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GdaxApi.Authentication;
    using GdaxApi.Exceptions;
    using GdaxApi.Utils;

    public class GdaxApiClient: IDisposable
    {
        private static readonly Uri BaseUriPublic = new Uri("https://api.gdax.com/");
        private static readonly Uri BaseUriSandbox = new Uri("https://api-public.sandbox.gdax.com/");
        private readonly HttpClient httpClient;
        private readonly bool hasOwnershipOfHttpClient;

        public GdaxApiClient(GdaxCredentials credentials, ISerializer serializer = null, bool sandbox = false)
                : this(new GdaxAuthenticationHandler(credentials) { InnerHandler = new HttpClientHandler() }, serializer, sandbox)
        {
        }

        public GdaxApiClient(GdaxAuthenticationHandler authenticationHandler, ISerializer serializer = null, bool sandbox = false)
        {
            if (authenticationHandler == null)
            {
                throw new ArgumentNullException(nameof(authenticationHandler));
            }

            this.Serializer = serializer ?? new Serializer();
            this.httpClient = new HttpClient(authenticationHandler);
            this.hasOwnershipOfHttpClient = true;
            this.BaseUri = sandbox ? BaseUriSandbox : BaseUriPublic;
        }

        public GdaxApiClient(HttpClient httpClient, ISerializer serializer = null, bool sandbox = false)
        {
            this.Serializer = serializer ?? new Serializer();
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.hasOwnershipOfHttpClient = false;
            this.BaseUri = sandbox ? BaseUriSandbox : BaseUriPublic;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal Uri BaseUri { get; }

        internal ISerializer Serializer { get; }

        internal ApiRequestBuilder<T> Get<T>(string path)
        {
            return new ApiRequestBuilder<T>(this, HttpMethod.Get, path);
        }

        internal ApiRequestBuilder<T> Post<T>(string path)
        {
            return new ApiRequestBuilder<T>(this, HttpMethod.Post, path);
        }

        internal ApiRequestBuilder<T> Delete<T>(string path)
        {
            return new ApiRequestBuilder<T>(this, HttpMethod.Delete, path);
        }

        internal async Task<T> SendAsync<T>(HttpRequestMessage request, Func<HttpResponseMessage, ApiResponse<T>> responseBuilder)
        {
            try
            {
                var httpResponse = await this.httpClient.SendAsync(request).ConfigureAwait(false);
                using (var response = responseBuilder(httpResponse))
                {
                    return await response.Content.ConfigureAwait(false);
                }
            }
            catch (GdaxApiException)
            {
                throw;
            }
            catch (AggregateException aex)
            {
                throw aex.Wrap("GdaxApi call failed");
            }
            catch (Exception ex)
            {
                throw ex.Wrap("GdaxApi call failed");
            }
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
