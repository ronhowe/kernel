using ClassLibrary1.Common;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TestProject1
{
    public static class EndpointCallHelper
    {
        public static async Task RunAsync(string requestUri, bool authenticated = false)
        {
            Tag.Where("RunAsync");

            AuthenticationResult? result = null;

            if (authenticated)
            {
                Tag.Why("PreClientAuthentication");

                // @TODO @ReadFromKeyVault
                AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.secrets.json");

                Tag.What($"config.ClientId={config.ClientId}");

                bool isUsingClientSecret = AppUsesClientSecret(config);

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
                // application permissions need to be set statically (in the portal or by PowerShell), and then
                // granted b ya tenant administrator.
                string[] scopes = new string[] { config.TodoListScope };

                try
                {
                    Tag.Why("PreAcquireTokenForClient");

                    result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                    Tag.Why("PostAcquireTokenForClient");
                }
                catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
                {
                    Tag.What("ScopeProvidedNotSupported");
                }

                Tag.Why("PostClientAuthentication");
            }

            // TODO @RefactorServerCall

            if (result != null)
            {
                Tag.Why("PreEndpointCall");

                HttpClient client = WebApiClientFactory.CreateClient(new InMemoryWebApiHost());

                var apiCaller = new PrivateEndpointCallHelper(client);

                await apiCaller.GetResultAsync(requestUri, result.AccessToken, TraceJObject);

                Tag.Why("PostEndpointCall");
            }
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to trace</param>
        private static void TraceJObject(IEnumerable<JObject> result)
        {
            Tag.Where("TraceJObject");

            foreach (var item in result)
            {
                foreach (JProperty property in item.Properties().Where(p => !p.Name.StartsWith("@")))
                {
                    Tag.What($"property.Name={property.Name}");
                    Tag.What($"property.Value={property.Value}");
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
            Tag.Where("AppUsesClientSecret");

            const string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";
            const string certificatePlaceholderValue = "[Or instead of client secret: Enter here the name of a certificate (from the user cert store) as registered with your application]";

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
                Tag.What("IsNullOrWhiteSpace");
                throw new Exception("IsNullOrWhiteSpace".TagError());
            }
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            Tag.Where("ReadCertificate");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                Tag.Error("IsNullOrWhiteSpace");
                throw new ArgumentException("IsNullOrWhiteSpace".TagError());
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            if (certificateDescription.Certificate == null)
            {
                Tag.Error("null");
                throw new Exception("null".TagError());
            }

            return certificateDescription.Certificate;
        }
    }
}