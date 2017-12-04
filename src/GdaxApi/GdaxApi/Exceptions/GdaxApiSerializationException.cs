namespace GdaxApi.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class GdaxApiSerializationException : GdaxApiException
    {
        public GdaxApiSerializationException()
        { }

        public GdaxApiSerializationException(string message)
            : base(message)
        { }

        public GdaxApiSerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected GdaxApiSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
