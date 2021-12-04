using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ncrunch
{
    [TestClass]
    public class IntegrationTests
    {
        private readonly Uri baseAddress = new("https://localhost:9999");

        [TestMethod]
        public void Unauthorized()
        {
            using var app = new ApiProgram();

            using var client = app.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = baseAddress

            });

            Trace.TraceInformation("Client Calling");
            using var response = client.GetAsync("/weatherforecast");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.Result.StatusCode);
        }

        [TestMethod]
        public void Authorized()
        {
            using var app = new ApiProgram();

            var client = app.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = baseAddress
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            Trace.TraceInformation("Client Calling");

            Trace.WriteLine("// Hard-Coded Comment ");

            var response = client.GetAsync("/weatherforecast");

            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test User"), new Claim("scp", "access_as_user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    public class ApiProgram : WebApplicationFactory<Program>
    {
        private readonly string _environment;

        public ApiProgram(string environment = "Development")
        {
            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);

            return base.CreateHost(builder);
        }
    }
}
