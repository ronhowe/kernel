using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Tag.How("UnitTest1.cs1");

            Tag.Where("TestInitialize");
        }

        [TestMethod]
        public async Task Debug()
        {
            await Task.Run(() => Tag.Who("Who"));
            await Task.Run(() => Tag.What("What"));
            await Task.Run(() => Tag.Where("Where"));
            await Task.Run(() => Tag.When("When"));
            await Task.Run(() => Tag.Why("Why"));
            await Task.Run(() => Tag.How("How"));

            await Task.Run(() => Tag.Warning("Warning"));
            await Task.Run(() => Tag.Error("Error"));

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

                    ConfigurationSetting setting = await client.GetConfigurationSettingAsync("configuration");

                    Tag.Why("PostGetConfigurationSetting");

                    Assert.IsNotNull(setting);
                    Assert.IsFalse(String.IsNullOrEmpty(setting.Value));

                    Tag.What($"setting.Value={setting.Value}");
                }
                catch (Exception ex)
                {
                    Tag.What($"ex.Message={ex.Message}");

                    Assert.Fail(ex.Message);
                }

            }
            catch (CredentialUnavailableException)
            {
                // Handle errors with loading the Managed Identity
            }
            catch (RequestFailedException)
            {
                // Handle errors with fetching the secret
            }
            catch (Exception)
            {
                // Handle generic errors
                Assert.Fail();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task KeyVault()
        {
            Trace.TraceInformation("@KeyVault()");

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
                    Trace.TraceInformation("@PreGetSecretAsync");
                    secret = (await client.GetSecretAsync("secret", cancellationToken: new CancellationToken())).Value;
                    Trace.TraceInformation("@PostGetSecretAsync");

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
            catch (CredentialUnavailableException)
            {
                // Handle errors with loading the Managed Identity
            }
            catch (RequestFailedException)
            {
                // Handle errors with fetching the secret
            }
            catch (Exception)
            {
                // Handle generic errors
                Assert.Fail();
            }
        }
    }
}
