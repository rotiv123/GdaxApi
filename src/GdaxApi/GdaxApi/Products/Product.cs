namespace GdaxApi.Products
{
	using System;

	public class Product
	{
		public Product ()
		{
		}

		internal Product (dynamic data)
		{
			this.Id = data.id;
			this.BaseCurrency = data.base_currency;
			this.QuoteCurrency = data.quote_currency;
			this.BaseMinSize = data.base_min_size;
			this.BaseMaxSize = data.base_max_size;
			this.QuoteIncrement = data.quote_increment;
		}

		public string Id { get; set; }

		public string BaseCurrency { get; set; }

		public string QuoteCurrency { get; set; }

		public string BaseMinSize { get; set; }

		public string BaseMaxSize { get; set; }

		public string QuoteIncrement { get; set; }
	}
}