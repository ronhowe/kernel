using ClassLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class IntegrationTest1 : TestBase
    {
        [TestMethod]
        public async Task LocalHost()
        {
            Tag.Where("LocalHost");

            Tag.Shout($"{Constant.LocalApiEndpoint}");

            var color = Color.Green;

            Tag.Why("PreRunCall");

            await Application.Run(Constant.LocalApiEndpoint, color);

            Tag.Why("PostRunCall");

            Tag.Shout($"IO {color}");
        }

        [TestMethod]
        [Ignore]
        public async Task RemoteHost()
        {
            Tag.Where("RemoteHost");

            Tag.Shout(Constant.RemoteApiEndpoint);

            var color = Color.Green;

            Tag.Why("PreRunCall");

            await Application.Run(Constant.RemoteApiEndpoint, color);

            Tag.Why("PostRunCall");

            Tag.Shout($"IO {color}");
        }
    }
}