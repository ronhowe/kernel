using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Trace.WriteLine("@RunAsync()");

            AuthenticationResult? result = null;

            if (authenticated)
            {
                Trace.TraceInformation("@PreClientAuthentication");

                // @TODO @ReadFromKeyVault
                AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.secrets.json");

                Trace.WriteLine($"@ClientId={config.ClientId}");

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
                    Trace.TraceInformation("@PreAcquireTokenForClient");

                    result = await app.AcquireTokenForClient(scopes)
                        .ExecuteAsync();

                    Trace.TraceInformation("@PostAcquireTokenForClient");
                }
                catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
                {
                    Trace.TraceError("@ScopeProvidedNotSupported");
                }

                Trace.TraceInformation("@PostClientAuthentication");
            }

            // TODO @RefactorServerCall

            if (result != null)
            {
                Trace.TraceInformation("@PreEndpointCall");

                HttpClient client = WebApiClientFactory.CreateClient(new InMemoryWebApiHost());

                var apiCaller = new PrivateEndpointCallHelper(client);

                await apiCaller.GetResultAsync(requestUri, result.AccessToken, TraceJObject);

                Trace.TraceInformation("@PostEndpointCall");
            }
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to trace</param>
        private static void TraceJObject(IEnumerable<JObject> result)
        {
            Trace.WriteLine("@TraceJObject()");

            foreach (var item in result)
            {
                foreach (JProperty child in item.Properties().Where(p => !p.Name.StartsWith("@")))
                {
                    Trace.WriteLine($"@{child.Name}={child.Value}");
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
                Trace.TraceError("@IsNullOrWhiteSpace");
                throw new Exception("@IsNullOrWhiteSpace");
            }
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            Trace.WriteLine("@ReadCertificate()");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                Trace.TraceError("@IsNullOrWhiteSpace");
                throw new ArgumentException("@IsNullOrWhiteSpace");
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            if (certificateDescription.Certificate == null)
            {
                Trace.TraceError("@null");
                throw new Exception("@null");
            }

            return certificateDescription.Certificate;
        }
    }
}