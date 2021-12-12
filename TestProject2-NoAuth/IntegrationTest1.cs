using ClassLibrary1.Common;
using ClassLibrary1.Contants;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class IntegrationTest1
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
    }
}