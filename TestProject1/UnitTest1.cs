using ClassLibrary1;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
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

                await Task.Run(() => Tag.Where("TestInitialize"));
            }
        }

        [TestMethod]
        public async Task Development()
        {
            await Task.Run(() => Tag.Where("Development"));

            var photon = PhotonFactory.Create(Color.Red);

            await VirtualStorageService.IO(photon);
            //await FileStorageService.IO(photon);
            //await TableStorageService.IO(photon);

            Assert.IsTrue(photon.Sent);
            Assert.IsTrue(photon.Received);
            Assert.AreEqual<Color>(Color.Red, photon.Color);
        }

        [TestMethod]
        public async Task Production()
        {
            Tag.Where("Production");

            var color = Color.Green;

            Tag.Why("PreRunCall");

            await Application.Run(Constant.ApiEndpoint, color);

            Tag.Why("PostRunCall");

            Tag.Shout($"IO {color}");
        }

        [TestMethod]
        public async Task Tags()
        {
            await Task.Run(() => Tag.Comment("Comment"));
            await Task.Run(() => Tag.Error("Error"));
            await Task.Run(() => Tag.How("How"));
            await Task.Run(() => Tag.Secret("Secret"));
            await Task.Run(() => Tag.Warning("Warning"));
            await Task.Run(() => Tag.What("What"));
            await Task.Run(() => Tag.When("When"));
            await Task.Run(() => Tag.Where("Where"));
            await Task.Run(() => Tag.Who("Who"));
            await Task.Run(() => Tag.Why("Why"));
            await Task.Run(() => Tag.Shout($"Shout"));
        }
    }
}
