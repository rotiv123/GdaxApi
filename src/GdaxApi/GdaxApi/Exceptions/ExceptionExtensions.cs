namespace GdaxApi.Exceptions
{
    using System;
    using System.Collections.Generic;

    static class ExceptionExtensions
    {
        public static Exception Wrap(this Exception ex, string message)
        {
            return ex is GdaxApiException ? ex : new GdaxApiException(message, ex);
        }

        public static AggregateException Wrap(this AggregateException aex, string message)
        {
            return new AggregateException(message, WrapInnerExceptions(aex, message));
        }

        private static IEnumerable<Exception> WrapInnerExceptions(AggregateException aex, string message)
        {
            foreach (var ex in aex.InnerExceptions)
            {
                if (ex is AggregateException innerAex)
                {
                    foreach (var innerEx in WrapInnerExceptions(innerAex, message))
                    {
                        yield return innerEx;
                    }
                }
                else
                {
                    yield return ex.Wrap(message);
                }
            }
        }
    }
}
