namespace GdaxApi.Pagination
{
    public class Page<T, U>
    {
        public T[] Items { get; internal set; }

        public U Before { get; internal set; }

        public U After { get; internal set; }
    }
}
