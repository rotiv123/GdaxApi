namespace GdaxApi
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GdaxApi.Authentication;
    using GdaxApi.Exceptions;
    using GdaxApi.Utils;

    public class GdaxApiClient : IDisposable
    {
        private static readonly Uri BaseUriPublic = new Uri("https://api.gdax.com/");
        private static readonly Uri BaseUriSandbox = new Uri("https://api-public.sandbox.gdax.com/");
        private readonly HttpClient httpClient;
        private readonly ISerializer serializer;
        private readonly bool hasOwnershipOfHttpClient;
        private readonly Uri baseUri;

        public GdaxApiClient(GdaxCredentials credentials, ISerializer serializer = null, bool sandbox = false)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            this.serializer = serializer ?? new Serializer();
            this.httpClient = new HttpClient(new GdaxAuthenticationHandler(credentials) { InnerHandler = new HttpClientHandler() });
            this.hasOwnershipOfHttpClient = true;
            this.baseUri = sandbox ? BaseUriSandbox : BaseUriPublic;
        }

        public GdaxApiClient(HttpClient httpClient, ISerializer serializer = null, bool sandbox = false)
        {
            this.serializer = serializer ?? new Serializer();
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.hasOwnershipOfHttpClient = false;
            this.baseUri = sandbox ? BaseUriSandbox : BaseUriPublic;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal Uri BaseUri => this.baseUri;

        internal ISerializer Serializer => this.serializer;

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
