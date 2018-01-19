namespace GdaxApi.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using GdaxApi.Authentication;
    using Xunit;

    public class CurrenciesClientTests
    {
        private static readonly GdaxCredentials FakeCredentials = new GdaxCredentials
        {
            ApiKey = "123",
            Passphrase = "321",
            Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("secret"))
        };

        [Fact]
        public async Task GetCurrencies_response_is_not_null()
        {
            using (var api = new GdaxApiClient(FakeCredentials, sandbox: true))
            {
                var currenciesClient = new CurrenciesClient(api);

                var result = await currenciesClient.GetCurrencies().SendAsync();

                Assert.NotNull(result);
            }
        }
    }
}
