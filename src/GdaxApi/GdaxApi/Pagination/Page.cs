using System;
using System.Collections;

namespace GdaxApi.Pagination
{
    public class Page<T, U>: IEnumerable
    {
        public T[] Items { get; internal set; }

        public U Before { get; internal set; }

        public U After { get; internal set; }

        private class PageEnumerator: IEnumerator
        {
            private readonly T[] items;
            private int position = -1;

            public PageEnumerator(T[] list)
            {
                items = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < items.Length);
            }

            public void Reset() { position = -1; }

            public object Current
            {
                get
                {
                    try
                    {
                        return items[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new PageEnumerator(Items);
        }
    }
}
