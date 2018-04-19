namespace GdaxApi.Authentication
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GdaxApi.Utils;

    public class GdaxAuthenticationHandler: DelegatingHandler
    {
        private readonly GdaxCredentials credentials;
        private readonly IDateProvider dateProvider;
        private readonly GdaxTimeOffset timeOffset;
        private readonly HMACSHA256 hmac;

        public GdaxAuthenticationHandler(GdaxCredentials credentials)
            : this(credentials, new DefaultDateProvider())
        {
        }

        public GdaxAuthenticationHandler(GdaxCredentials credentials, IDateProvider dateProvider)
            : base()
        {
            this.credentials = credentials;
            this.dateProvider = dateProvider;
            this.timeOffset = new GdaxTimeOffset();
            var key = Convert.FromBase64String(this.credentials.Secret);
            this.hmac = new HMACSHA256(key);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timestamp = (int)(this.dateProvider.UnixTimestamp - timeOffset.Offset);
            var signature = await this.ComputeSignature(request, timestamp).ConfigureAwait(false);

            SetHttpRequestHeaders(request, timestamp, signature);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        internal double GetGdaxTimeOffset { get { return this.timeOffset.Offset; } }

        internal async Task<double> GdaxTimeOffset(GdaxApiClient api, bool refresh)
        {
            try
            {
                return await this.timeOffset.GetOffset(api, this.dateProvider, refresh);
            }
            catch { throw; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.hmac.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SetHttpRequestHeaders(HttpRequestMessage requestMessage, int timestamp, string signature)
        {
            requestMessage.Headers.Add("CB-ACCESS-KEY", this.credentials.ApiKey);
            requestMessage.Headers.Add("CB-ACCESS-SIGN", signature);
            requestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timestamp.ToString(CultureInfo.InvariantCulture));
            requestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", this.credentials.Passphrase);
        }

        private async Task<string> ComputeSignature(HttpRequestMessage request, int timestamp)
        {
            var content = request.Content == null ? null : (await request.Content.ReadAsStringAsync().ConfigureAwait(false));
            var prehash = $"{timestamp}{request.Method.ToString().ToUpper()}{request.RequestUri.PathAndQuery}{content}";
            var bytes = Encoding.UTF8.GetBytes(prehash);
            var hash = this.hmac.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
