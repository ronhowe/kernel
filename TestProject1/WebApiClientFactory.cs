using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace TestProject1
{
    public static class WebApiClientFactory
    {
        private static readonly Uri Address = new("https://localhost:9999");

        public static HttpClient CreateClient(InMemoryWebApiHost server)
        {
            Trace.WriteLine("@CreateClient()");

            return server.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = Address
            });
        }
    }
}
