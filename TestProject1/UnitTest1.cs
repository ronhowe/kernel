using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
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
        public async Task Atheist()
        {
            Trace.TraceInformation("@Atheist()");

            await Task.Run(() => Trace.WriteLine("@TODO @Debug"));
        }

        [TestMethod]
        public async Task Agnostic()
        {
            Trace.TraceInformation("@Agnostic()");

            // Arrange
            Trace.TraceInformation("@Arrange");

            using var server = new Server();

            #region Mock Authorization

            var client = server.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    Trace.TraceInformation("@AddAuthentication @Mock");

                    services.AddAuthentication("MockScheme")
                        .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(
                            "MockScheme", options => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = baseAddress
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("MockScheme");

            #endregion Mock Authorization

            // Act
            Trace.TraceInformation("@Act");

            var response = await client.GetAsync(authenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Trace.TraceInformation($"@response.Headers.Count={response.Headers.Count()}");
            Trace.TraceInformation($"@response.Content=\n{await response.Content.ReadAsStringAsync()}");
        }

        [TestMethod]
        public async Task Authenticated()
        {
            Trace.TraceInformation("@Authenticated()");

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

            using var response = await client.GetAsync(authenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Authorized()
        {
            Trace.TraceInformation("@Authorized()");

            // Arrange
            Trace.TraceInformation("@Arrange");

            // Act
            Trace.TraceInformation("@Act");

            // Assert
            Trace.TraceInformation("@Assert");

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await RunAsync());
        }

        [TestMethod]
        public async Task Anonymous()
        {
            Trace.TraceInformation("@Anonymous()");

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

            using var response = await client.GetAsync(unauthenticatedEndpoint);

            // Assert
            Trace.TraceInformation("@Assert");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [Ignore]
        public async Task PrototypeGetAzureAppConfigurationWithVisualStudioCredential()
        {
            Trace.TraceInformation("@PrototypeGetAzureAppConfigurationWithVisualStudioCredential()");

            try
            {
                var options = new DefaultAzureCredentialOptions
                {
                    ExcludeAzureCliCredential = true,
                    ExcludeEnvironmentCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeManagedIdentityCredential = true,
                    ExcludeSharedTokenCacheCredential = true,
                    ExcludeVisualStudioCodeCredential = true,
                    ExcludeVisualStudioCredential = false
                };

                var credential = new DefaultAzureCredential(options);

                ConfigurationClient client = new(new Uri("https://ronhoweorg.azconfig.io"), credential);

                try
                {
                    Trace.TraceInformation("@PreGetConfigurationSetting @ExternalDependency");
                    ConfigurationSetting setting = await client.GetConfigurationSettingAsync("configuration");
                    Trace.TraceInformation("@PostGetConfigurationSetting @ExternalDependency");

                    Assert.IsNotNull(setting);
                    Assert.IsFalse(String.IsNullOrEmpty(setting.Value));

                    Trace.TraceInformation($"@setting.Value={setting.Value}");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);

                    Assert.Fail(ex.Message);
                }

            }
            catch (CredentialUnavailableException e)
            {
                // Handle errors with loading the Managed Identity
            }
            catch (RequestFailedException)
            {
                // Handle errors with fetching the secret
            }
            catch (Exception e)
            {
                // Handle generic errors
                Assert.Fail();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task PrototypeGetAzureKeyVaultSecretWithVisualStudioCredential()
        {
            Trace.TraceInformation("@PrototypeGetAzureKeyVaultSecretWithVisualStudioCredential()");

            try
            {
                var options = new DefaultAzureCredentialOptions
                {
                    ExcludeAzureCliCredential = true,
                    ExcludeEnvironmentCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeManagedIdentityCredential = true,
                    ExcludeSharedTokenCacheCredential = true,
                    ExcludeVisualStudioCodeCredential = true,
                    ExcludeVisualStudioCredential = false
                };

                var credential = new DefaultAzureCredential(options);

                SecretClient client = new(new Uri("https://ronhoweorg.vault.azure.net/"), credential);

                KeyVaultSecret secret;

                try
                {
                    Trace.TraceInformation("@PreGetSecretAsync @ExternalDependency");
                    secret = (await client.GetSecretAsync("secret", cancellationToken: new CancellationToken())).Value;
                    Trace.TraceInformation("@PostGetSecretAsync @ExternalDependency");

                    Assert.IsNotNull(secret);
                    Assert.IsFalse(String.IsNullOrEmpty(secret.Value));

                    Trace.TraceInformation($"@secret.Name={secret.Name} @secret.Value={secret.Value}");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);

                    Assert.Fail(ex.Message);
                }

            }
            catch (CredentialUnavailableException e)
            {
                // Handle errors with loading the Managed Identity
            }
            catch (RequestFailedException)
            {
                // Handle errors with fetching the secret
            }
            catch (Exception e)
            {
                // Handle generic errors
                Assert.Fail();
            }
        }

        private static async Task RunAsync()
        {
            Trace.TraceInformation("@RunAsync()");

            Trace.TraceWarning("@TODO @RefactorClientAuthentication");

            Trace.TraceInformation("@PreClientAuthentication");

            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("secrets.json");

            Trace.TraceInformation($"@AuthenticationConfig=\n{config}");

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
                Trace.TraceInformation("@PreAcquireTokenForClient @ExternalDependency");

                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                Trace.TraceInformation("@PostAcquireTokenForClient @ExternalDependency");
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
                var apiCaller = new PrivateEndpointCallHelper(client);
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
                    Trace.TraceInformation($"\n@child.Name={child.Name}\n@child.Value={child.Value}\n");
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
                Trace.TraceError("@MissingConfiguration @ClientSecret @Or @Certificate");
                throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json or secrets.json file.");
            }
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            Trace.TraceInformation("@ReadCertificate()");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                Trace.TraceError("@MissingConfiguration @CertificateName @Empty");
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json or secrets.json file.", "certificateName");
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            return certificateDescription.Certificate;
        }
    }

    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Trace.TraceInformation("@HandleAuthenticateAsync()");

            var claims = new[] { new Claim("roles", "DaemonAppRole"), new Claim("roles", "DataWriterRole") };

            var identity = new ClaimsIdentity(claims, "MockIdentity");

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, "MockScheme");

            var result = AuthenticateResult.Success(ticket);

            Trace.TraceInformation("@Authenticated @Mock");

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

    /// <summary>
    /// Helper class to call a protected API and process its result
    /// </summary>
    public class PrivateEndpointCallHelper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient used to call the protected API</param>
        public PrivateEndpointCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Calls the protected web API and processes the result
        /// </summary>
        /// <param name="webApiUrl">URL of the web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the web API</param>
        /// <param name="processResult">Callback used to process the result of the call to the web API</param>
        public async Task CallWebApiAndProcessResultASync(string webApiUrl, string accessToken, Action<IEnumerable<JObject>> processResult)
        {
            Trace.TraceInformation("@CallWebApiAndProcessResultASync()");

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                Trace.TraceInformation("@PreGetAsync() @ExternalDependency");
                HttpResponseMessage response = await HttpClient.GetAsync(webApiUrl);
                Trace.TraceInformation("@PostGetAsync() @ExternalDependency");

                if (response.IsSuccessStatusCode)
                {
                    Trace.TraceInformation("@Response @IsSuccessStatusCode @True");

                    string json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<List<JObject>>(json);

                    processResult(result);
                }
                else
                {
                    // Note that if you got reponse.Code == 403 and reponse.content.code == "Authorization_RequestDenied"
                    // this is because the tenant admin as not granted consent for the application to call the Web API

                    Trace.TraceError("@Response @IsSuccessStatusCode @False");

                    string content = await response.Content.ReadAsStringAsync();

                    Trace.WriteLine($"@Content {content}");
                }

                Trace.TraceInformation($"@Response @StatusCode @{response.StatusCode}");
            }
        }
    }

    /// <summary>
    /// Description of the configuration of an AzureAD public client application (desktop/mobile application). This should
    /// match the application registration done in the Azure portal
    /// </summary>
    public class AuthenticationConfig
    {
        /// <summary>
        /// instance of Azure AD, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";

        /// <summary>
        /// The Tenant is:
        /// - either the tenant ID of the Azure AD tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            }
        }

        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by the CertificateName property belows)
        /// <remarks> 
        public string ClientSecret { get; set; }

        /// <summary>
        /// Name of a certificate in the user certificate store
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: the property above)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by this CertificateName property)
        /// <remarks> 
        public string CertificateName { get; set; }

        /// <summary>
        /// Web Api base URL
        /// </summary>
        public string TodoListBaseAddress { get; set; }

        /// <summary>
        /// Web Api scope. With client credentials flows, the scopes is ALWAYS of the shape "resource/.default"
        /// </summary>
        public string TodoListScope { get; set; }

        /// <summary>
        /// Reads the configuration from a json file
        /// </summary>
        /// <param name="path">Path to the configuration json file</param>
        /// <returns>AuthenticationConfig read from the json file</returns>
        public static AuthenticationConfig ReadFromJsonFile(string path)
        {
            IConfigurationRoot configuration;

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path);

            configuration = builder.Build();
            return configuration.Get<AuthenticationConfig>();
        }

        public override string ToString()
        {
            return $"\n@Instance={this.Instance}\n@Tenant={this.Tenant}\n@ClientId={this.ClientId}\n@ClientSecret={this.ClientSecret}\n@CertificateName={this.CertificateName}\n@TodoListBaseAddress={this.TodoListBaseAddress}\n@TodoListScope={this.TodoListScope}\n";
        }
    }
}
