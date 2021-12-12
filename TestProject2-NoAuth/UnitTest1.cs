using ClassLibrary1.Common;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Main()
        {
            Tag.Where("Main");

            var application = new NewApplication();

            var color = PacketColor.Green;

            await application.Run(color);

            Tag.Line(FiggleFonts.Standard.Render(color));
        }
    }
}