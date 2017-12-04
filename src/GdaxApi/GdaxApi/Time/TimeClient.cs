namespace GdaxApi
{
    using GdaxApi.Time;

    public interface ITimeClient
    {
        ApiRequestBuilder<ApiTime> GetTime();
    }

    public class TimeClient : ITimeClient
    {
        private readonly GdaxApiClient api;

        public TimeClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<ApiTime> GetTime()
        {
            return this.api.GetTime();
        }
    }

    public static class TimeClientExtensions
    {
        public static ApiRequestBuilder<ApiTime> GetTime(this GdaxApiClient api)
        {
            return api.Get<ApiTime>("time");
        }
    }
}
