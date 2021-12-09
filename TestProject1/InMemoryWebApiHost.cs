using ClassLibrary1.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace TestProject1
{
    public class InMemoryWebApiHost : WebApplicationFactory<Program>
    {
        private readonly string _environment;

        public InMemoryWebApiHost(string environment = "Development")
        {
            Tag.Where("InMemoryWebApiHost");

            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Tag.Where("CreateHost");

            builder.UseEnvironment(_environment);

            return base.CreateHost(builder);
        }
    }
}
