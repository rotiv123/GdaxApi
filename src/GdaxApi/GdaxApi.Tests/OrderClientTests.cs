namespace GdaxApi.Tests
{
    using System.Threading.Tasks;
    using GdaxApi.Authentication;
    using GdaxApi.Orders;
    using Xunit;

    public class OrderClientTests
    {
        private static readonly GdaxCredentials FakeCredentials = new GdaxCredentials
        {
            ApiKey = "a7d42723c1a430ac7c302b5f5f6ff9b5",
            Passphrase = "z1yi8yngtkty",
            Secret = "xgYizakX7B7wT8VtcT6Rr/bQ+kJKLoOXlicd1kpoFMwzQZEyvrNkFKxMACjhFVi6oklApXHiY6LkMLQ4dI+Rpg=="
        };

        [Fact]
        public async Task PostMarketOrder_response_is_not_null()
        {
            using (var api = new GdaxApiClient(FakeCredentials, sandbox: true))
            {
                var orderClient = new OrderClient(api);
                var order = new MarketOrderRequest(OrderRequest.OrderSide.Buy, "BTC-EUR") { Funds = 1 };

                var result = await orderClient.PostMarketOrder(order).SendAsync();

                Assert.NotNull(result);
            }
        }
    }
}
