using System.Threading.Tasks;
using System.Diagnostics;

namespace GdaxApi.Authentication
{
    using GdaxApi.Utils;
    using GdaxApi.Time;

    public class GdaxTimeOffset
    {
        internal double Offset { get; private set; } = 0d;

        internal async Task<double> GetOffset(GdaxApiClient api, IDateProvider dateProvider, bool refresh = false)
        {
            ApiTime exchangetime;

            try
            {
                exchangetime = await api.GetTime().SendAsync();
            }
            catch { throw; }

            double offset = dateProvider.UnixTimestamp - exchangetime.UnixTimestamp;
            if (refresh)
                this.Offset = offset;

            Debug.WriteLine(string.Format("GdaxTimeOffset: Local UTC Time   {0}", dateProvider.Now.ToUniversalTime()));
            Debug.WriteLine(string.Format("GdaxTimeOffset: GDAX Server Time {0}", exchangetime.Timestamp));
            Debug.WriteLine(string.Format("GdaxTimeOffset: Timestamp offset {0}", offset));

            return offset;
        }
    }
}
