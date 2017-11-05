namespace GdaxApi.Pagination
{
    using System;

    public class Page<T>
    {
        public T[] Items { get; internal set; }

        public DateTimeOffset Before { get; internal set; }

        public DateTimeOffset After { get; internal set; }
    }
}
