using System;
using System.Collections;

namespace GdaxApi.Pagination
{
    public class Page<T, U>: IEnumerable
    {
        public T[] Items { get; internal set; }

        public U Before { get; internal set; }

        public U After { get; internal set; }


        //private enumerator class
        private class PageEnumerator: IEnumerator
        {
            private T[] Items;
            private int position = -1;

            //constructor
            public PageEnumerator(T[] list)
            {
                Items = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < Items.Length);
            }

            public void Reset() { position = -1; }

            public object Current
            {
                get
                {
                    try
                    {
                        return Items[position];
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
