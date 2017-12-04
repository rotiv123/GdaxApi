namespace GdaxApi.Products
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class AggregatedBook
    {
        public AggregatedBook()
        {
            this.Bids = new Entry[0];
            this.Asks = new Entry[0];
        }

        internal AggregatedBook(JObject data)
        {
            this.Bids = data["bids"].Select(
                x => new Entry
                {
                    Price = x[0].Value<decimal>(),
                    Size = x[1].Value<decimal>(),
                    NumerOfOrders = x[2].Value<int>()
                }).ToArray();

            this.Asks = data["asks"].Select(
                x => new Entry
                {
                    Price = x[0].Value<decimal>(),
                    Size = x[1].Value<decimal>(),
                    NumerOfOrders = x[2].Value<int>()
                }).ToArray();
        }

        public long Sequence { get; set; }

        public Entry[] Bids { get; set; }

        public Entry[] Asks { get; set; }

        public class Entry
        {
            public decimal Price { get; set; }

            public decimal Size { get; set; }

            public int NumerOfOrders { get; set; }
        }
    }
}
