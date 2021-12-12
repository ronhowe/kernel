using ClassLibrary1.Common;
using ClassLibrary1.Contants;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class IntegrationTest1 : Test1Base
    {
        [TestMethod]
        public async Task Production()
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