using ClassLibrary1;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Trace.WriteLine("@TestInitialize()");

        }

        [TestMethod]
        public async Task HealthCheck()
        {
            Trace.WriteLine("@WebApiHealthCheck()");

            // Arrange
            Trace.WriteLine("@Arrange");

            HttpClient client = WebApiClientFactory.CreateClient(new InMemoryWebApiHost());

            // Act
            Trace.WriteLine("@Act");

            using var response = await client.GetAsync(EndpointMap.HealthCheckEndpoint);

            // Assert
            Trace.WriteLine("@Assert");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Read()
        {
            Trace.WriteLine("@WebApiRead()");

            // Arrange
            Trace.WriteLine("@Arrange");

            HttpClient client = WebApiClientFactory.CreateClient(new InMemoryWebApiHost());

            // Act
            Trace.WriteLine("@Act");

            using var response = await client.GetAsync(EndpointMap.IoEndpoint);

            // Assert
            Trace.WriteLine("@Assert");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Write()
        {
            Trace.WriteLine("@WebApiWrite()");

            // Arrange
            Trace.WriteLine("@Arrange");

            // Act
            Trace.WriteLine("@Act");

            Trace.TraceWarning("@TODO @RefactorRunAsync");

            await RunAsync();

            // Assert
            Trace.WriteLine("@Assert");

            Trace.TraceWarning("@TODO");
        }

        private static async Task RunAsync()
        {
            Trace.WriteLine("@RunAsync()");

            Trace.WriteLine("@PreClientAuthentication");

            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("secrets.json");

            Trace.WriteLine($"@AuthenticationConfig=\n{config}");

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

            AuthenticationResult? result = null;
            try
            {
                Trace.WriteLine("@PreAcquireTokenForClient");

                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                Trace.WriteLine("@PostAcquireTokenForClient");
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                Trace.TraceError("@ScopeProvidedNotSupported");
            }

            Trace.WriteLine("@PostClientAuthentication");

            // TODO @RefactorServerCall

            if (result != null)
            {
                Trace.WriteLine("@PreEndpointCall");

                HttpClient client = WebApiClientFactory.CreateClient(new InMemoryWebApiHost());

                var apiCaller = new PrivateEndpointCallHelper(client);

                await apiCaller.GetResultAsync(EndpointMap.IoEndpoint, result.AccessToken, TraceJObject);

                Trace.WriteLine("@PostEndpointCall");
            }
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to trace</param>
        private static void TraceJObject(IEnumerable<JObject> result)
        {
            //Trace.WriteLine("@TraceJObject()");

            foreach (var item in result)
            {
                foreach (JProperty child in item.Properties().Where(p => !p.Name.StartsWith("@")))
                {
                    Trace.WriteLine($"@child.Name={child.Name}\n@child.Value={child.Value}");
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
            Trace.WriteLine("@AppUsesClientSecret()");

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
            Trace.WriteLine("@ReadCertificate()");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                Trace.TraceError("@MissingConfiguration @CertificateName @Empty");
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json or secrets.json file.");
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            if (certificateDescription.Certificate == null)
            {
                throw new Exception("@CertificateNotFound");
            }

            return certificateDescription.Certificate;
        }
    }
}
