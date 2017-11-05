# GdaxApi

A .NET client for the GDAX REST API https://docs.gdax.com/#api

## Quick Start

```cs
var credentials = new GdaxApi.Authentication.GdaxCredentials
					{
						ApiKey = "...",
						Passphrase = "...",
						Secret = "..."
					};
using (var api = new GdaxApiClient(new GdaxApi.Utils.Serializer(), credentials))
{
    var orders = await api.GetAllOrders().Limit(3).SendAsync();
    
	// ...
}
```
