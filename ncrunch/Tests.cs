using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace ncrunch
{
    [TestClass]
    public class Tests
    {
        private readonly Uri baseAddress = new("https://localhost:9999");
        private readonly string authenticatedEndpoint = "/authenticatedEndpoint";
        private readonly string unauthenticatedEndpoint = "/unauthenticatedEndpoint";

        [TestInitialize()]
        public void TestInitialize()
        {
            Trace.TraceInformation("@TestInitialize()");

            Trace.TraceInformation("@Test.cs");
        }

        [TestMethod]
        public void Debug()
        {
            Trace.TraceInformation("@Debug()");

            Trace.WriteLine("@TODO @Debug");
        }

        [TestMethod]
        public void Authorized()
        {
            Trace.TraceInformation("@Authorized()");

            // Arrange
            Trace.TraceWarning("@TODO @Arrange");

            // Act
            Trace.TraceWarning("@TODO @Act");

            RunAsync().GetAwaiter().GetResult();

            // Assert
            Trace.TraceWarning("@TODO @Assert");

            //Assert.AreEqual(HttpStatusCode.OK, HttpStatusCode.OK);
        }

        [TestMethod]
        public void Unauthenticated()
        {
            Trace.TraceInformation("@Unauthenticated()");

            // Arrange
            Trace.TraceInformation("@Arrange");

            using var server = new Server();

            using var client = server.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = baseAddress
            });

            // Act
            Trace.TraceInformation("@Act");

            using var response = client.GetAsync(unauthenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }

        [TestMethod]
        public void Unauthorized()
        {
            Trace.TraceInformation("@Unauthorized()");

            // Arrange
            Trace.TraceInformation("@Arrange");

            using var server = new Server();

            using var client = server.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = baseAddress
            });

            // Act
            Trace.TraceInformation("@Act");

            using var response = client.GetAsync(authenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.Result.StatusCode);
        }

        [TestMethod]
        public void MockAuthorized()
        {
            Trace.TraceInformation("@MockAuthorized()");

            Trace.TraceWarning("@TODO @RenameToApplication");

            // Arrange
            Trace.TraceInformation("@Arrange");

            using var server = new Server();

            #region Mock Authorization

            var client = server.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    Trace.TraceInformation("@AddAuthenticationMock");

                    services.AddAuthentication("Mock")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Mock", options => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = baseAddress
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            #endregion Mock Authorization

            // Act
            Trace.TraceInformation("@Act");

            var response = client.GetAsync(authenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }

        private static async Task RunAsync()
        {
            Trace.TraceInformation("@RunAsync()");

            Trace.TraceWarning("@TODO @RefactorClientAuthentication");

            Trace.TraceInformation("@PreClientAuthentication");

            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.json");

            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = AppUsesClientSecret(config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }
            else
            {
                X509Certificate2 certificate = ReadCertificate(config.CertificateName);
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            app.AddInMemoryTokenCache();

            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator
            string[] scopes = new string[] { config.TodoListScope };

            AuthenticationResult result = null;
            try
            {

                Trace.TraceInformation("@PreAcquireTokenForClient");

                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                Trace.TraceInformation("@PostAcquireTokenForClient @TokenAcquired");
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                Trace.TraceError("@CatchAcquireTokenForClient @ScopeProvidedNotSupported");
            }

            Trace.TraceInformation("@PostClientAuthentication");

            Trace.TraceWarning("@TODO @RefactorServerCall");

            if (result != null)
            {
                Trace.TraceInformation("@PreServerCall");

                Uri baseAddress = new("https://localhost:9999");
                string authenticatedEndpoint = "/authenticatedEndpoint";
                //string unauthenticatedEndpoint = "/unauthenticatedEndpoint";

                using var server = new Server();

                using var client = server.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    BaseAddress = baseAddress
                });

                //using var response = client.GetAsync(unauthenticatedEndpoint);
                var apiCaller = new ProtectedApiCallHelper(client);
                await apiCaller.CallWebApiAndProcessResultASync(authenticatedEndpoint, result.AccessToken, TraceJObject);

                Trace.TraceInformation("@PostServerCall");
            }
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to trace</param>
        private static void TraceJObject(IEnumerable<JObject> result)
        {
            Trace.TraceInformation("@TraceJObject()");

            foreach (var item in result)
            {
                foreach (JProperty child in item.Properties().Where(p => !p.Name.StartsWith("@")))
                {
                    Trace.WriteLine($"{child.Name} = {child.Value}");
                }
            }
        }

        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        private static bool AppUsesClientSecret(AuthenticationConfig config)
        {
            Trace.TraceInformation("@AppUsesClientSecret()");

            string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";
            string certificatePlaceholderValue = "[Or instead of client secret: Enter here the name of a certificate (from the user cert store) as registered with your application]";

            if (!String.IsNullOrWhiteSpace(config.ClientSecret) && config.ClientSecret != clientSecretPlaceholderValue)
            {
                return true;
            }

            else if (!String.IsNullOrWhiteSpace(config.CertificateName) && config.CertificateName != certificatePlaceholderValue)
            {
                return false;
            }

            else
            {
                throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json file.");
            }
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            Trace.TraceInformation("@ReadCertificate()");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json", "certificateName");
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            return certificateDescription.Certificate;
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
            Trace.TraceInformation("@HandleAuthenticateAsync()");

            var claims = new[] { new Claim(ClaimTypes.Name, "MockName"), new Claim("roles", "DaemonAppRole") };

            var identity = new ClaimsIdentity(claims, "MockIdentity");

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, "Mock");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    public class Server : WebApplicationFactory<Program>
    {
        private readonly string _environment;

        public Server(string environment = "Development")
        {
            Trace.TraceInformation("@Server()");

            _environment = environment;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Trace.TraceInformation("@CreateHost()");

            builder.UseEnvironment(_environment);

            return base.CreateHost(builder);
        }
    }
}
