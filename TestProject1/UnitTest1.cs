using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Debug()
        {
            //////////////////////////////////////////////////////////////////////////////////////////
            #region Debug

            // ARRANGE
            var packet = new Packet()
            {
                Id = Guid.NewGuid(),
                ReferenceId = Guid.NewGuid(),
                Sent = false,
                Received = false,
                Color = PacketColor.Green
            };

            Tag.What($"packet={packet}");
            Assert.IsFalse(packet.Sent);
            Assert.IsFalse(packet.Received);
            Assert.AreEqual<PacketColor>(PacketColor.Green, packet.Color);

            // ACT
            var result = await IO(packet);

            Tag.What($"packet={result}");
            Assert.IsTrue(result.Sent);
            Assert.IsTrue(result.Received);
            Assert.AreEqual(PacketColor.Green, result.Color);

            #region ASSERT OLD
            // ASSERT
            //Tag.What($"sentPacket.Sent={sentPacket.Sent}");
            //Tag.What($"receivedPacket.Sent={receivedPacket.Sent}");
            //Assert.IsTrue(receivedPacket.Sent);

            //Tag.What($"sentPacket.Received={sentPacket.Received}");
            //Tag.What($"receivedPacket.Received={receivedPacket.Received}");
            //Assert.IsTrue(receivedPacket.Received);

            //Tag.What($"sentPacket.Id={sentPacket.Id}");
            //Tag.What($"receivedPacket.Id={receivedPacket.Id}");
            //Assert.AreEqual<Guid>(sentPacket.Id, receivedPacket.Id);

            //Tag.What($"sentPacket.ReferenceId={sentPacket.ReferenceId}");
            //Tag.What($"receivedPacket.ReferenceId={receivedPacket.ReferenceId}");
            //Assert.AreEqual<Guid>(sentPacket.ReferenceId, receivedPacket.ReferenceId);

            //Tag.What($"sentPacket.Color={sentPacket.Color}");
            //Tag.What($"receivedPacket.Color={receivedPacket.Color}");
            //Assert.AreEqual<PacketColor>(sentPacket.Color, receivedPacket.Color);
            //Assert.AreNotEqual<PacketColor>(PacketColor.Black, receivedPacket.Color);
            //Assert.AreNotEqual<PacketColor>(sentPacket.Color, PacketColor.Black);
            #endregion ASSERT OLD

            #endregion Debug
            //////////////////////////////////////////////////////////////////////////////////////////
        }

        private async Task<Packet> IO(Packet packet)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInput");

            Tag.What($"packet={packet}");

            await Input(packet);

            Tag.Why("PostInput");

            packet.Sent = true;

            Tag.ToDo("FixPreprocessorDirectiveBug12345");

#if false
            // Only Shows In Debug
            await ReadTextAsync($"{packet.Id}.json");
#endif

            Tag.Why("PreOuput");

            var receivedPacket = await Output(packet.Id);

            Tag.Line($"receivedPacket={receivedPacket}");

            Tag.Why("PostOuput");

            packet.Received = true;

            packet.Color = receivedPacket.Color;

            Tag.Why("IOComplete");

            return packet;
        }

        public async Task Input(Packet packet)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            string fileName = $"{packet.Id}.json";

            Tag.What($"fileName={fileName}");

            using FileStream createStream = File.Create(fileName);

            Tag.What($"createStream.Name={createStream.Name}");

            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            };

            JsonSerializerOptions optionsCopy = new(options);

            Tag.Why("PreSerializeAsync");

            await JsonSerializer.SerializeAsync(createStream, packet, optionsCopy);

            Tag.Why("PostSerializeAsync");

            await createStream.DisposeAsync();

            Tag.ToDo("RemovePostSerializeAsync");
            await ReadTextAsync(fileName);

            Tag.Why("InputComplete");
        }

        public async Task<Packet> Output(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            string fileName = $"{id}.json";

            Tag.What($"fileName={fileName}");

            Tag.ToDo("RemovePreDeserializeAsyncRead");
            await ReadTextAsync(fileName);

            using FileStream openStream = File.OpenRead(fileName);

            Tag.What($"openStream.Name={openStream.Name}");

            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            };

            JsonSerializerOptions optionsCopy = new(options);

            Tag.Why("PreDeserializeAsync");

            Packet deserializedPacket = await JsonSerializer.DeserializeAsync<Packet>(openStream, options);

            Tag.Why("PostDeserializeAsync");

            Tag.ToDo("SimulateReadCorruptionFeature12345");

            if (false)
            {
                deserializedPacket.Id = Guid.NewGuid();
            }

            Tag.What($"deserializedPacket={deserializedPacket}");

            Tag.Why("OutputComplete");

            return deserializedPacket;
        }

        public Task<string> ReadTextAsync(string filePath)
        {
            var task = new Task<string>(() =>
            {
                using (FileStream sourceStream = new FileStream(filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true))
                {
                    StringBuilder sb = new StringBuilder();

                    byte[] buffer = new byte[0x1000];
                    int numRead;

                    while ((numRead = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string text = Encoding.ASCII.GetString(buffer, 0, numRead);
                        sb.Append(text);
                    }

                    Tag.ToDo("RefactorTagBlockFunctionalityFeature12345");

                    if (true)
                    {
                        Tag.Line($"{sb}");
                    }

                    return sb.ToString();
                }
            });

            task.Start();

            return task;
        }

        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.How("UnitTest1"));
            await Task.Run(() => Tag.Where("TestInitialize"));
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
