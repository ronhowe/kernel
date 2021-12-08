using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
            Trace.WriteLine("@UnitTest.cs1");

            Trace.WriteLine("@TestInitialize()");
        }

        [TestMethod]
        public async Task Debug()
        {
            await Task.Run(() => Trace.WriteLine("@Debug()"));
        }

        [TestMethod]
        [Ignore]
        public async Task AppConfiguration()
        {
            Trace.WriteLine("@AppConfiguration()");

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
                    Trace.WriteLine("@PreGetConfigurationSetting");
                    ConfigurationSetting setting = await client.GetConfigurationSettingAsync("configuration");
                    Trace.WriteLine("@PostGetConfigurationSetting");

                    Assert.IsNotNull(setting);
                    Assert.IsFalse(String.IsNullOrEmpty(setting.Value));

                    Trace.WriteLine($"@setting.Value={setting.Value}");
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
                    Trace.WriteLine("@PreGetSecretAsync");
                    secret = (await client.GetSecretAsync("secret", cancellationToken: new CancellationToken())).Value;
                    Trace.WriteLine("@PostGetSecretAsync");

                    Assert.IsNotNull(secret);
                    Assert.IsFalse(String.IsNullOrEmpty(secret.Value));

                    Trace.WriteLine($"@secret.Name={secret.Name} @secret.Value={secret.Value}");
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
