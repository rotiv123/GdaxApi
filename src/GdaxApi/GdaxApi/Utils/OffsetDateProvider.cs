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
        /// <para>To allow the client time to more closely match the GDAX API web servers time reducing the likelyhood of a "request timestamp expired" exception,</para>
        /// <para>use <see cref="RealOffset"/> to get the real offset and use <see cref="Refresh"/> to refresh the stored offset.</para>
        /// <para>Note that the stored vs real offset can change over time, periodically check that the stored vs real value does not exceed
        /// ±30 seconds and refresh when it does, or you will start to recieve "request timestamp expired" exceptions.</para>
        /// </summary>
        public double StoredOffset { get; private set; } = 0d;

        public DateTimeOffset Now => DateTimeOffset.Now.AddSeconds(StoredOffset);

        public double UnixTimestamp => DateTime.UtcNow.ToUnixTimestamp() + StoredOffset;

        /// <summary>
        /// Returns the real time offset between the GDAX API web servers time and the clients local time in seconds.
        /// <para>To allow the client time to more closely match the GDAX API web servers time reducing the likelyhood of a "request timestamp expired" exception,</para>
        /// <para>use <see cref="StoredOffset"/> to get the stored offset and use <see cref="Refresh"/> to refresh the stored offset.</para>
        /// <para>Note that the real vs stored offset can change over time, periodically check that the real vs stored value does not exceed
        /// ±30 seconds and refresh when it does, or you will start to recieve "request timestamp expired" exceptions.</para>
        /// </summary>
        public double RealOffset(ApiTime exchangetime)
        {
            if (exchangetime == null)
            {
                throw new ArgumentNullException(nameof(exchangetime));
            }

            return exchangetime.UnixTimestamp - DateTime.UtcNow.ToUnixTimestamp();
        }

        /// <summary>
        /// Refreshes the StoredOffset
        /// </summary>
        public void Refresh(ApiTime exchangetime)
        {
            if (exchangetime == null)
            {
                throw new ArgumentNullException(nameof(exchangetime));
            }

            this.StoredOffset = exchangetime.UnixTimestamp - DateTime.UtcNow.ToUnixTimestamp();
        }
    }
}
