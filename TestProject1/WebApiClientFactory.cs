using ClassLibrary1.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace TestProject1
{
    public static class WebApiClientFactory
    {
        private static readonly Uri Address = new("https://localhost:7107");

        public static HttpClient CreateClient(InMemoryWebApiHost server)
        {
            Tag.Where("CreateClient");

            return server.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = Address
            });
        }
    }
}
