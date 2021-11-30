using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace ncrunch;

internal class Application : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public Application(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        return base.CreateHost(builder);
    }
}
