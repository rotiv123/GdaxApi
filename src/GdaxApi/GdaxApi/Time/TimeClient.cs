namespace GdaxApi
{
    using GdaxApi.Time;

    public interface ITimeClient
    {
        ApiRequestBuilder<TimeResponse> GetTime();
    }

    public class TimeClient : ITimeClient
    {
        private readonly GdaxApiClient api;

        public TimeClient(GdaxApiClient api)
        {
            this.api = api;
        }

        public ApiRequestBuilder<TimeResponse> GetTime()
        {
            return this.api.GetTime();
        }
    }

    public static class TimeClientExtensions
    {
        public static ApiRequestBuilder<TimeResponse> GetTime(this GdaxApiClient api)
        {
            return api.Get<TimeResponse>("time");
        }
    }
}
