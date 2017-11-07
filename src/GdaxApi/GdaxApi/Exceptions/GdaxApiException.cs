namespace GdaxApi.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class GdaxApiException : Exception
    {
        public GdaxApiException()
        { }

        public GdaxApiException(string message)
            : base(message)
        { }

        public GdaxApiException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected GdaxApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
