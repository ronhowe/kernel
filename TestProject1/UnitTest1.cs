using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.How("UnitTest1"));

            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        public async Task Debug()
        {
            await Task.Run(() => Tag.Where("Debug"));
        }

        [TestMethod]
        public async Task Tags()
        {
            await Task.Run(() => Tag.Who("Who"));
            await Task.Run(() => Tag.What("What"));
            await Task.Run(() => Tag.Where("Where"));
            await Task.Run(() => Tag.When("When"));
            await Task.Run(() => Tag.Why("Why"));
            await Task.Run(() => Tag.How("How"));
            await Task.Run(() => Tag.Warning("Warning"));
            await Task.Run(() => Tag.Error("Error"));
            await Task.Run(() => Tag.Secret("Secret"));
            await Task.Run(() => Tag.Comment("Comment"));
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
