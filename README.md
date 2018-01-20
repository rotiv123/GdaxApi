# GdaxApi

A .NET client for the GDAX REST API https://docs.gdax.com/#api

[![Build](https://ci.appveyor.com/api/projects/status/ldgoxa26q4vt94hs?svg=true)](https://ci.appveyor.com/project/rotiv123/gdaxapi) [![NuGet](https://img.shields.io/nuget/v/GdaxApi.svg)](https://www.nuget.org/packages/GdaxApi/)

## Quick Start

```cs
var credentials = new GdaxApi.Authentication.GdaxCredentials
					{
						ApiKey = "...",
						Passphrase = "...",
						Secret = "..."
					};
using (var api = new GdaxApiClient(credentials))
{
    var orders = await api.GetAllOrders().Limit(3).SendAsync();
	// ...
}
```
