namespace GdaxApi
{
    using GdaxApi.Currencies;

    public interface ICurrenciesClient
    {
        ApiRequestBuilder<Currency[]> GetCurrencies();
    }

    public class CurrenciesClient : ICurrenciesClient
    {
        private readonly GdaxApiClient api;

        public CurrenciesClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<Currency[]> GetCurrencies()
        {
            return this.api.GetCurrencies();
        }
    }

    public static class CurrenciesClientExtensions
    {
        public static ApiRequestBuilder<Currency[]> GetCurrencies(this GdaxApiClient api)
        {
            return api.Get<Currency[]>("currencies");
        }
    }
}
