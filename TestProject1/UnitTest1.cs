using ClassLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        [TestMethod]
        public async Task Development()
        {
            await Task.Run(() => Tag.Where("Development"));

            var photon = PhotonFactory.Create(Color.Red);

            await NullStorageService.IO(photon);
            await NullStorageService<Photon>.IO(photon);
            //await FileStorageService.IO(photon);
            //await TableStorageService.IO(photon);

            Assert.IsTrue(photon.Sent);
            Assert.IsTrue(photon.Received);
            Assert.AreEqual<Color>(Color.Red, photon.Color);
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
