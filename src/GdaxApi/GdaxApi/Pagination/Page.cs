namespace GdaxApi.Pagination
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Page<T, U>: IEnumerable<T>
    {
        public T[] Items { get; internal set; }

        public U Before { get; internal set; }

        public U After { get; internal set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Array.AsReadOnly(this.Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}
