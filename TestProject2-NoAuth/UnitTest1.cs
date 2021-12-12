using ClassLibrary1.Common;
using ClassLibrary1.Contants;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Entities;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class UnitTest1
    {
        private string TagFileName()
        {
            var stackTrace = new StackTrace(true);
            var fileName = stackTrace.GetFrame(0).GetFileName();
            return fileName;
        }

        [TestInitialize()]
        public async Task TestInitialize()
        {
            Tag.How(TagFileName());

            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        public async Task Debug()
        {
            await Task.Run(() => Tag.Where("Debug"));

            var packet = PacketFactory.Create(PacketColor.Red);

            await LocalStorageService.IO(packet);

            Assert.IsTrue(packet.Sent);
            Assert.IsTrue(packet.Received);
            Assert.AreEqual<PacketColor>(PacketColor.Red, packet.Color);
        }

        [TestMethod]
        public async Task Live()
        {
            Tag.Where("Live");

            var application = new Application();

            var color = PacketColor.Green;

            Tag.Why("PreRunCall");

            await application.Run(Constant.ApiEndpoint, color);

            Tag.Why("PostRunCall");

            Tag.Line(FiggleFonts.Standard.Render(color));
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