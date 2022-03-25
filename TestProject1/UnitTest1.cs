using ClassLibrary1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            StackTrace? stackTrace = new(true);
            if (stackTrace is not null)
            {
                var frame = stackTrace.GetFrame(0);

                if (frame is not null)
                {
                    var fileName = frame.GetFileName();
                    if (fileName is not null)
                    {
                        Tag.How(fileName);
                    }
                }

                //Tag.What($"BuildConfiguration={Constant.BuildConfiguration}");

                await Task.Run(() => Tag.Where("TestInitialize"));
            }
        }

        [TestMethod]
        public async Task Development()
        {
            await Task.Run(() => Tag.Where("Development"));
        }

        [TestMethod]
        [DataRow("https://localhost:9999")]
        public async Task WebApplication1(string host)
        {
            // Match this to appsettings.json for consistency.
            const string outputTemplate = "**CLIENT** [{MachineName}] {Timestamp:HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .CreateLogger();

            //Log.Debug("**Log.Debug()**");
            //Log.Information("**Log.Information()**");
            //Log.ForContext("SourceContext", "**SourceContext**").Information("**Log.ForContext().Information()**");

            //Log.Information("WebApplication1".TagWhere());

            //Tag.What($"host={host}");

            using var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });

            using var client = application.CreateClient(new() { BaseAddress = new Uri(host) });

            var color = Color.Green;

            //Tag.Why("PreRunCall");

            await Run(client, color);

            //Tag.Why("PostRunCall");

            //Tag.Shout($"OK {color}");
        }

        private static async Task Run(HttpClient client, Color color)
        {
            //Log.Information("***********************".TagWhere());
            //Tag.Where("Run");

            //Tag.What($"color={color}");

            #region Client Authentication

            //Tag.ToDo("RefactorClientAuthentication");
            if (true)
            {
                //Tag.Why("PreClientAuthentication");

                // @TODO @ReadFromKeyVault
                AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.secrets.json");

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
                    //Tag.Why("PreAcquireTokenForClient");

                    var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                    //Tag.Why("PostAcquireTokenForClient");
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(result.AccessToken))
                        {
                            var defaultRequestHeaders = client.DefaultRequestHeaders;

                            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                            {
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            }

                            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                        }
                    }
                }
                catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
                {
                    Tag.Error("ScopeProvidedNotSupported");
                }

                //Tag.Why("PostClientAuthentication");
            }

            #endregion Client Authentication

            #region Send/Write

            Photon sentPhoton = PhotonFactory.Create(color);

            //Tag.Why("PrePostAsJsonAsyncCall");

            try
            {
                HttpResponseMessage? response = await client.PostAsJsonAsync(ApplicationEndpoint.BasicInputOutputService, sentPhoton);
            }
            catch (HttpRequestException ex)
            {
                //Tag.Error("HttpRequestException");
                //Tag.Error(ex.Message);
                // TODO Example how to codify remediations.
                //Tag.ToDo("CreateRunbook12345");
                //Tag.Comment("Runbook12345");
                throw ex;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (NotSupportedException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Tag.Error("ContentTypeNotSupported");
                //Tag.Error(ex.Message);
                throw new ApplicationException();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (JsonException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Tag.Error("InvalidJson");
                //Tag.Error(ex.Message);
                throw new ApplicationException();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Tag.Error("UnknownException");
                //Tag.Error(ex.Message);
                throw new ApplicationException();
            }

            //Tag.Why("PostPostAsJsonAsyncCall");

            #endregion Send/Write

            #region Read/Receive

            Photon? receivedPhoton;

            //Tag.Why("PreGetFromJsonAsyncCall");

            string uri = $"{ApplicationEndpoint.BasicInputOutputService}?id={sentPhoton.Id}";
            //Tag.What($"uri={uri}");

            try
            {
                receivedPhoton = await client.GetFromJsonAsync<Photon>(uri);
            }
            // TODO This is likely NOT the exception thrown, but shows we can handle different kinds differently.
#pragma warning disable CS0168 // Variable is declared but never used
            catch (JsonException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Tag.Error("InvalidJson");
                //Tag.Error(ex.Message);
                throw new ApplicationException();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Tag.Error("UnknownException");
                //Tag.Error(ex.Message);
                throw new ApplicationException();
            }

            //Tag.Why("PostGetFromJsonAsyncCall");

            #endregion Read/Receive

            //Tag.What($"sentPhoton={sentPhoton}");
            //Tag.What($"receivedPhoton={receivedPhoton}");

            //Tag.ToDo("ImplementSentAndReceivedProperties");

            Assert.IsNotNull(receivedPhoton);
            Assert.AreEqual(sentPhoton.Id, receivedPhoton.Id);
            Assert.AreEqual(sentPhoton.Color, receivedPhoton.Color);
        }

        private static bool AppUsesClientSecret(AuthenticationConfig config)
        {
            //Tag.Where("AppUsesClientSecret");

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
                //Tag.What("IsNullOrWhiteSpace");
                throw new Exception("IsNullOrWhiteSpace".TagError());
            }
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            //Tag.Where("ReadCertificate");

            if (string.IsNullOrWhiteSpace(certificateName))
            {
                //Tag.Error("IsNullOrWhiteSpace");
                throw new ArgumentException("IsNullOrWhiteSpace".TagError());
            }

            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);

            DefaultCertificateLoader defaultCertificateLoader = new();

            defaultCertificateLoader.LoadIfNeeded(certificateDescription);

            if (certificateDescription.Certificate == null)
            {
                //Tag.Error("null");
                throw new Exception("null".TagError());
            }

            return certificateDescription.Certificate;
        }
    }
}