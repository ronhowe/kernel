using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class UnitTest1 : Test1Base
    {
        [TestMethod]
        public async Task Development()
        {
            await Task.Run(() => Tag.Where("Development"));

            var packet = PacketFactory.Create(PacketColor.Red);

            await LocalStorageService.IO(packet);

            Assert.IsTrue(packet.Sent);
            Assert.IsTrue(packet.Received);
            Assert.AreEqual<PacketColor>(PacketColor.Red, packet.Color);
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
    }
}