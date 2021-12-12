using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using ClassLibrary1.Infrastructure;
using Figgle;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ClassLibrary1.Services
{
    public class Application
    {
        public async Task Run(string uriString, PacketColor color)
        {
            Tag.Where("Run");

            Tag.What($"uriString={uriString}");
            Tag.What($"color={color}");

            HttpClient httpClient = new() { BaseAddress = new Uri(uriString) };

            if (true)
            {
                Tag.Why("PreClientAuthentication");

                // @TODO @ReadFromKeyVault
                AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.secrets.json");

                // Useful, but spammy.
                //Tag.What($"config.Instance={config.Instance}");
                //Tag.What($"config.Tenant={config.Tenant}");
                //Tag.What($"config.ClientId={config.ClientId}");
                //Tag.What($"config.Authority={config.Authority}");
                //Tag.Secret($"config.ClientSecret={config.ClientSecret}");
                //Tag.What($"config.CertificateName={config.CertificateName}");
                //Tag.What($"config.TodoListBaseAddress={config.TodoListBaseAddress}");
                //Tag.What($"config.TodoListScope={config.TodoListScope}");
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

                    var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                    Tag.Why("PostAcquireTokenForClient");
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(result.AccessToken))
                        {
                            var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

                            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                            {
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            }

                            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                        }
                    }
                }
                catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
                {
                    Tag.Error("ScopeProvidedNotSupported");
                }

                Tag.Why("PostClientAuthentication");
            }

            Packet sentPacket = PacketFactory.Create(color);

            try
            {
                Tag.Why("PrePostAsJsonAsyncCall");

                var httpResponse = await httpClient.PostAsJsonAsync(Endpoints.BIOS, sentPacket);

                Tag.Why("PostPostAsJsonAsyncCall");

                Tag.Line(FiggleFonts.Standard.Render(httpResponse.StatusCode.ToString()));

                Tag.Why("PreGetFromJsonAsyncCall");

                var receivedPacket = await httpClient.GetFromJsonAsync<Packet>($"{Endpoints.BIOS}?id={sentPacket.Id}");

                Tag.Why("PostGetFromJsonAsyncCall");

                Tag.ToDo("ImplementSentAndReceivedProperties");

                Tag.What($"sentPacket={sentPacket}");
                Tag.What($"receivedPacket={receivedPacket}");
            }
            catch (HttpRequestException ex)
            {
                Tag.Error("HttpRequestException");
                Tag.Error(ex.Message);
                Tag.Comment("Runbook12345"); // Runbook stating to turn on website.
                throw ex;
            }
            catch (NotSupportedException ex)
            {
                Tag.Error("ContentTypeNotSupported");
                Tag.Error(ex.Message);
                throw ex;
            }
            catch (JsonException ex)
            {
                Tag.Error("InvalidJson");
                Tag.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Tag.Error("UnknownException");
                Tag.Error(ex.Message);
                throw;
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