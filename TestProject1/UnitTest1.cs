using ClassLibrary1.Common;
using ClassLibrary1.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            await Task.Run(() => Tag.Where("TestInitialize"));
        }

        [TestMethod]
        [Ignore]
        public async Task Debug()
        {
            await Task.Run(() => Tag.Where("Debug"));
        }

        [TestMethod]
        [Ignore]
        public async Task PostEndpoint()
        {
            Tag.Where("Post");

            await EndpointCallHelper.RunAsync(Endpoints.POST, false);
        }

        [TestMethod]
        [Ignore]
        public async Task IOEndpoint()
        {
            Tag.Where("IOEndpoint");

            await EndpointCallHelper.RunAsync(Endpoints.BIOS, true);
        }
    }
}
