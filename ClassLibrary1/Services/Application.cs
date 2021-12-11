using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System.Text.Json;
using ClassLibrary1.Infrastructure;

namespace ClassLibrary1.Services
{
    public class Application
    {
        public async Task Run(PacketColor color)
        {
            Tag.How("Application");

            Tag.Where("Run");

            Tag.Why("ApplicationStart");

            Tag.Why("PreClientAuthentication");

            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.secrets.json");

            Tag.What($"config.Instance={config.Instance}");
            Tag.What($"config.Tenant={config.Tenant}");
            Tag.What($"config.ClientId={config.ClientId}");
            Tag.What($"config.Authority={config.Authority}");
            Tag.Secret($"config.ClientSecret={config.ClientSecret}");
            Tag.What($"config.CertificateName={config.CertificateName}");
            Tag.What($"config.TodoListBaseAddress={config.TodoListBaseAddress}");
            Tag.What($"config.TodoListScope={config.TodoListScope}");

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

            AuthenticationResult? result = null;

            try
            {
                Tag.Why("PreAcquireTokenForClient");

                result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                Tag.Why("PostAcquireTokenForClient");
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                Tag.What("ScopeProvidedNotSupported");

                Tag.ToDo("NotImplementedException");
                throw new NotImplementedException();
            }

            Tag.Why("PostClientAuthentication");

            Tag.What($"result.AccessToken={result.AccessToken}");

            HttpClient client = new() { BaseAddress = new Uri(config.TodoListBaseAddress) };

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                var defaultRequestHeaders = client.DefaultRequestHeaders;

                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                Tag.Why("PreGetAsync");

                HttpResponseMessage response = await client.GetAsync(Endpoints.BIOS);

                Tag.Why("PostGetAsync");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    //result = JsonSerializer.Deserialize<Packet>(json);

                    Tag.What($"result={result}");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
            }
            //PacketService service = new();

            //Tag.Why("PreIOCall");

            //await service.IO(packet);

            //Tag.Why("PostIOCall");

            //Tag.What(packet.ToString());

            Tag.Why("ApplicationComplete");
        }

        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        private bool AppUsesClientSecret(AuthenticationConfig config)
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

        private X509Certificate2 ReadCertificate(string certificateName)
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