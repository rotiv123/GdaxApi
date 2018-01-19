namespace GdaxApi.Currencies
{
    public class Currency
    {
        public Currency()
        {
        }

        internal Currency(dynamic data)
        {
            this.Id = data.id;
            this.Name = data.name;
            this.MinSize = data.min_size;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public double MinSize { get; set; }
    }
}
