namespace GdaxApi.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using GdaxApi.Authentication;
    using Xunit;

    public class TimeClientTests
    {
        private static readonly GdaxCredentials FakeCredentials = new GdaxCredentials
        {
            ApiKey = "123",
            Passphrase = "321",
            Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("secret"))
        };

        [Fact]
        public async Task GetTime_response_is_not_null()
        {
            using (var api = new GdaxApiClient(FakeCredentials, sandbox: true))
            {
                var timeClient = new TimeClient(api);

                var result = await timeClient.GetTime().SendAsync();

                Assert.NotNull(result);
            }
        }
    }
}
