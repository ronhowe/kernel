using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace TestProject1
{
    public class InMemoryWebApiHost : WebApplicationFactory<Program>
    {
        private readonly string _environment;

        public InMemoryWebApiHost(string environment = "Development")
        {
            Trace.WriteLine("@InMemoryWebApiHost()");

            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Trace.WriteLine("@CreateHost()");

            builder.UseEnvironment(_environment);

            return base.CreateHost(builder);
        }
    }
}
