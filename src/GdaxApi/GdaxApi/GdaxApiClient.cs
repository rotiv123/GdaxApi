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
        private readonly GdaxAuthenticationHandler authenticationHandler;
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
            this.authenticationHandler = authenticationHandler;
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

        /// <summary>
        /// Gets the stored time offset between the GDAX API web servers time and the clients local time in seconds.
        /// <para>Note that the stored vs real offset can change over time, periodically check that the stored vs real value does not exceed 30 seconds
        /// or you will start to recieve "request timestamp expired" exceptions.</para>
        /// <para>Use the <see cref="GdaxTimeOffset"/> method with the optional parameter 'refresh' set to true to refresh the stored offset.</para>
        /// </summary>
        public double GetGdaxTimeOffset { get { return this.authenticationHandler.GetGdaxTimeOffset; } }

        /// <summary>
        /// Returns the real time offset between the GDAX API web servers time and the clients local time in seconds.
        /// <para>To allow the client time to more closely match the GDAX API web servers time reducing the likelyhood of a "request timestamp expired" exception,
        /// you can refresh the stored time offset by using the optional parameter 'refresh'.</para>
        /// <para>Note that the real vs stored offset can change over time, periodically check that the real vs stored value does not exceed
        /// 30 seconds and refresh when it does, or you will start to recieve "request timestamp expired" exceptions.</para>
        /// </summary>
        public async Task<double> GdaxTimeOffset(bool refresh = false)
        {
            try
            {
                return await this.authenticationHandler.GdaxTimeOffset(this, refresh);
            }
            catch { throw; }
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
