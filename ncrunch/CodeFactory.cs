using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ncrunch
{
    [TestClass]
    public class CodeFactory
    {
        [TestMethod]
        public void Post()
        {
            WriteTestHeader("Post");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Run()
        {
            WriteTestHeader("Run");
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void Unauthorized()
        {
            WriteTestHeader("Unauthorized");

            using var sut = new SystemUnderTest();
            using var client = sut.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:9999")

            });
            using var response = client.GetAsync("/weatherforecast");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.Result.StatusCode);
        }


        [TestMethod]
        public void Authorized()
        {
            WriteTestHeader("Authorized");

            using var sut = new SystemUnderTest();

            var client = sut.WithWebHostBuilder(builder =>
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
                BaseAddress = new Uri("https://localhost:9999")
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            var response = client.GetAsync("/weatherforecast");

            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }


        private static void WriteTestHeader(string tag)
        {
            Trace.WriteLine(String.Format("{0} @ {1}", tag, DateTime.Now));
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
}
