using System;
using System.Threading.Tasks;
using System.Diagnostics;
using GdaxApi.Utils;
using GdaxApi.Time;

namespace GdaxApi.Authentication
{
    /// <summary>
    /// IDateProvider with Offset
    /// </summary>
    public class OffsetDateProvider: IDateProvider
    {
        /// <summary>
        /// Gets the stored time offset between the GDAX API web servers time and the clients local time in seconds.
        /// <para>Note that the stored vs real offset can change over time, periodically check that the stored vs real value does not exceed 30 seconds
        /// or you will start to recieve "request timestamp expired" exceptions.</para>
        /// <para>Use the <see cref="RealOffset"/> method with the optional parameter 'refresh' set to true to refresh the stored offset.</para>
        /// </summary>
        public double StoredOffset { get; private set; } = 0d;

        public DateTimeOffset Now => DateTimeOffset.Now;

        public double UnixTimestamp => DateTime.UtcNow.ToUnixTimestamp() - StoredOffset;

        /// <summary>
        /// Returns the real time offset between the GDAX API web servers time and the clients local time in seconds.
        /// <para>To allow the client time to more closely match the GDAX API web servers time reducing the likelyhood of a "request timestamp expired" exception,
        /// you can refresh the stored time offset by using the optional parameter 'refresh'.</para>
        /// <para>Note that the real vs stored offset can change over time, periodically check that the real vs stored value does not exceed
        /// 30 seconds and refresh when it does, or you will start to recieve "request timestamp expired" exceptions.</para>
        /// </summary>
        public async Task<double> RealOffset(GdaxApiClient api, bool refresh = false)
        {
            ApiTime exchangetime;

            try
            {
                exchangetime = await api.GetTime().SendAsync();
            }
            catch { throw; }

            double offset = DateTime.UtcNow.ToUnixTimestamp() - exchangetime.UnixTimestamp;
            if (refresh)
                this.StoredOffset = offset;

            Debug.WriteLine(string.Format("GdaxTimeOffset: Local UTC Time   {0}", DateTimeOffset.Now.ToUniversalTime()));
            Debug.WriteLine(string.Format("GdaxTimeOffset: GDAX Server Time {0}", exchangetime.Timestamp));
            Debug.WriteLine(string.Format("GdaxTimeOffset: Timestamp offset {0}", offset));

            return offset;
        }
    }
}
