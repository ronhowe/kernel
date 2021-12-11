using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Infrastructure;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.How("IntegrationTest1"));

            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        [Ignore]
        public async Task Main()
        {
            Tag.Why("Main");

            var application = new Application();

            Tag.Why("PreRunCall");

            await application.Run(PacketColor.Green);

            Tag.Why("PostRunCall");

            Tag.Line(FiggleFonts.Standard.Render(PacketColor.Green));
        }

        [TestMethod]
        public async Task PostEndpoint()
        {
            Tag.Where("Post");

            await EndpointCallHelper.RunAsync(Endpoints.POST, false);
        }

        [TestMethod]
        public async Task IOEndpoint()
        {
            Tag.Where("IOEndpoint");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);
        }


        [TestMethod]
        [Ignore]
        public async Task AppConfiguration()
        {
            Tag.Where("AppConfiguration");

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
                    Tag.Why("PreGetConfigurationSetting");

                    ConfigurationSetting setting = await client.GetConfigurationSettingAsync("Enabled");

                    Tag.Why("PostGetConfigurationSetting");

                    Tag.What($"setting.Value={setting.Value}");
                }
                catch (Exception ex)
                {
                    Tag.Error($"ex.Message={ex.Message}");
                }

            }
            catch (CredentialUnavailableException ex)
            {
                // Handle errors with loading the Managed Identity
                Tag.Error(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                // Handle errors with fetching the secret
                Tag.Error(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle generic errors
                Tag.Error(ex.Message);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task KeyVault()
        {
            Tag.Where("KeyVault");

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
                    Tag.Why("PreGetSecretAsync");

                    secret = (await client.GetSecretAsync("secret", cancellationToken: new CancellationToken())).Value;

                    Tag.Why("PostGetSecretAsync");

                    Tag.What($"secret.Name={secret.Name}");
                    Tag.Secret($"secret.Value={secret.Value}");
                }
                catch (Exception ex)
                {
                    Tag.Error(ex.Message);
                }

            }
            catch (CredentialUnavailableException ex)
            {
                // Handle errors with loading the Managed Identity
                Tag.Error(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                // Handle errors with fetching the secret
                Tag.Error(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle generic errors
                Tag.Error(ex.Message);
            }
        }
    }
}